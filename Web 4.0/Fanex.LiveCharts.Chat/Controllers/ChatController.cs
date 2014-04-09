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
    using Fanex.LiveCharts.Criterias.Chat;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly ISettingService _settingService;

        public ChatController()
            : this(null, null)
        {
        }

        public ChatController(IChatService chatService, ISettingService settingService)
        {
            _chatService = chatService ?? new ChatService();
            _settingService = settingService ?? new SettingService();
        }

        public ActionResult Index(int matchId)
        {
            var chatViewModel = GetChatViewModel(matchId);
            if (chatViewModel.ChatUserInRoom.KickedOut)
            {
                return RedirectToAction("KickedOut");
            }

            return View(GetChatViewModel(matchId));
        }

        public ActionResult YouAreDisconnect()
        {
            return View();
        }

        public ActionResult KickedOut()
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
        public JsonResult KickOutChatUser(KickOutChatUserViewModel kickOutChatUserViewModel)
        {
            return Json(_chatService.KickOutChatUserOnMemberSite(kickOutChatUserViewModel));
        }

        [HttpPost]
        public JsonResult DeleteChatMessage(DeleteChatMessageViewModel deleteChatMessageViewModel)
        {
            return Json(_chatService.DeleteChatMessageOnMemberSite(deleteChatMessageViewModel));
        }

        #endregion Actions for Admin Site access

        #region Private Actions

        /// <summary>
        /// Get chat view model from match id.
        /// </summary>
        /// <param name="matchId">Current match id.</param>
        /// <returns>Chat view model.</returns>
        private ChatViewModel GetChatViewModel(int matchId)
        {
            ChatUser chatUser = _chatService.GetChatUser(new GetChatUserCriteria
            {
                MemberId = SessionManager.CurrentSession.MemberId
            });

            ChatRoom chatRoom = _chatService.GetChatRoomByName(new GetChatRoomByNameCriteria
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

            SettingItem annoucementMessage = _settingService.GetSetting(new GetTermConditionCriteria());
            SettingItem welcomeMessage = _settingService.GetSetting(new GetWelcomeMessageCriteria());
            if (welcomeMessage.SettingValue.Contains("{0}"))
            {
                welcomeMessage.SettingValue = string.Format(welcomeMessage.SettingValue, chatUser.ChatName);
            }
            return new ChatViewModel
                {
                    ChatRoom = chatRoom,
                    ChatUser = chatUser,
                    ChatUserInRoom = chatUserInRoom,
                    AnnoucementMessage = annoucementMessage.SettingValue,
                    WelcomeMessage = welcomeMessage.SettingValue
                };
        }

        #endregion Private Actions
    }
}