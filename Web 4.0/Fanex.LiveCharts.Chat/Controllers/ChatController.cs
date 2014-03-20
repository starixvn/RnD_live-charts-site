// --------------------------------------------------------------------------
// <copyright file="ChatController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Controllers
{
    using System.Web.Mvc;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Services;
    using Fanex.LiveCharts.Controllers;
    using Fanex.LiveCharts.Web;
    using Fanex.LiveCharts.Chat.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;

        public ChatController()
            : this(null)
        {
        }

        public ChatController(IChatService chatService)
        {
            _chatService = chatService ?? new ChatService();
        }

        public ActionResult Index(int matchId)
        {
            var chatUser = _chatService.GetChatUser(new GetChatUserCriteria
                {
                     MemberId = SessionManager.CurrentSession.MemberId
                });
            
            return View(new ChatViewModel { 
                MatchId = matchId,
                ChatUser = chatUser
            });
        }

        [HttpPost]
        public JsonResult GetChatMessageByTime(GetChatMessageByTimeCriteria getChatMessageByTimeCriteria)
        {
            var result = _chatService.GetChatMessagesByTime(getChatMessageByTimeCriteria);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetChatMessageByUser(GetChatMessageByUserIdCriteria getChatMessageByUserCriteria)
        {
            var result = _chatService.GetChatMessagesByUser(getChatMessageByUserCriteria);

            return Json(result);
        }

        [HttpPost]
        public JsonResult InsertChatUser(InsertChatUserCriteria insertChatUserCriteria)
        {
            var commandResult = _chatService.InsertChatUser(insertChatUserCriteria);

            return Json(commandResult);
        }
    }
}