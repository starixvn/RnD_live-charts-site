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
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Controllers;
    using Fanex.LiveCharts.Web;

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

        /// <summary>
        /// Return index page of chat portal.
        /// </summary>
        /// <param name="matchId">Current match id.</param>
        /// <returns>View of index page.</returns>
        public ActionResult Index(int matchId)
        {
            var chatUser = _chatService.GetChatUser(new GetChatUserCriteria
                {
                    MemberId = SessionManager.CurrentSession.MemberId
                });

            var chatRoom = _chatService.GetChatRoomByName(new GetChatRoomByNameCriteria
            {
                ChatRoomName = string.Format("{0}_{1}", matchId, SessionManager.CurrentSession.Language.Replace('-', '_').Trim()),
                MatchId = matchId
            });

            var chatUserInRoom = new ChatUserInRoom();
            if (chatUser.ChatUserId > 0)
            {
                chatUserInRoom = _chatService.GetChatUserInRoom(new GetChatUserInRoomCriteria
                    {
                        ChatUserId = chatUser.ChatUserId,
                        ChatRoomId = chatRoom.ChatRoomId
                    });
            }

            return View(
                new ChatViewModel
                {
                    ChatRoom = chatRoom,
                    ChatUser = chatUser,
                    ChatUserInRoom = chatUserInRoom
                });
        }

        public ActionResult YouAreDisconnect()
        { 
            return View();
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


        #region Actions for Admin Site access
        
        [HttpPost]
        public JsonResult KickOutChatUser(int chatUserId, string connectionId, string tokenName, string tokenKey)
        {
            return Json(_chatService.KickOutChatUserOnMemberSite(chatUserId, connectionId, tokenName, tokenKey));
        }

        [HttpPost]
        public JsonResult DeleteChatMessage(int chatMessageId, string chatRoomName, string tokenName, string tokenKey)
        {
            return Json(_chatService.DeleteChatMessageOnMemberSite(chatMessageId, chatRoomName, tokenName, tokenKey));
        }

        #endregion
    }
}