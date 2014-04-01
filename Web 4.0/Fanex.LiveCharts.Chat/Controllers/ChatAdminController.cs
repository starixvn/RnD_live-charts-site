// --------------------------------------------------------------------------
// <copyright file="ChatAdminController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.1">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
    public class ChatAdminController : Controller
    {
        private IChatService _chatService;

        public ChatAdminController()
            : this(null)
        {
        }

        public ChatAdminController(IChatService chatService)
        {
            _chatService = chatService ?? new ChatService();
        }

        /// <summary>
        /// Chat Admin monitor page.
        /// </summary>
        /// <param name="matchId">Current match id for viewing chat.</param>
        /// <returns>Chat monitor page.</returns>
        public ActionResult ChatMonitor(int matchId)
        {
            var chatRooms = _chatService.GetChatRoomByMatchId(new GetChatRoomByMatchIdCriteria
            {
                MatchId = matchId
            });

            var defaultChatRoom = new ChatRoom();
            if (chatRooms.Any())
            {
                defaultChatRoom = chatRooms.ElementAt(0);
            }

            return View("~/Areas/Admin/Views/Chat/ChatMonitor.cshtml", defaultChatRoom);
        }

        [HttpPost]
        public JsonResult GetChatMessages(int chatRoomId)
        {
            return Json(_chatService.GetChatMessageByRoom(new GetChatMessageByRoomCriteria
            {
                ChatRoomId = chatRoomId
            }));
        }
    }
}