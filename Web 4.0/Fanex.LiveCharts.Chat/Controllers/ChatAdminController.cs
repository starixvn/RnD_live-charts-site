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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Controllers;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.Web;
    using Fanex.LiveCharts.Web.CustomActionResult;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Chat Admin Controller.
    /// </summary>
    [NapSuperAuth]
    public class ChatAdminController : BaseController
    {
        private const string ChatMonitorSessionKey = "Chat.Controllers.ChatAdminController.ChatMonitorSessionKey_{0}";
        private const string DefaultLanguage = "en-US";
        private readonly IChatService _chatService;
        private readonly IChartService _chartService;
        private readonly IMatchService _matchService;

        public ChatAdminController()
            : this(null, null, null)
        {
        }

        public ChatAdminController(IChatService chatService, IChartService chartService, IMatchService matchService)
        {
            _chatService = chatService ?? new ChatService();
            _chartService = chartService ?? new ChartService();
            _matchService = matchService ?? new MatchService();
        }

        /// <summary>
        /// Chat Admin monitor page.
        /// </summary>
        /// <param name="matchId">Current match id for viewing chat.</param>
        /// <returns>Chat monitor page.</returns>
        public ActionResult ChatMonitor(int matchId)
        {
            var chatMonitorViewModel = GetChatMonitorViewModel(matchId);

            return View("~/Areas/Admin/Views/Chat/ChatMonitor.cshtml", chatMonitorViewModel);
        }

        public ActionResult BanWordManagement()
        {
            return View("~/Areas/Admin/Views/Chat/BanWordManagement.cshtml");
        }

        /// <summary>
        /// Search chat messages.
        /// </summary>
        /// <param name="criteria">Search message criteria.</param>
        /// <returns>Message List.</returns>
        [HttpPost]
        public JsonResult GetChatMessages(SearchChatMessageCriteria criteria)
        {
            criteria.IncludingDeleted = true;
            IEnumerable<ChatMessage> chatMessages = _chatService.SearchChatMessage(criteria);

            IEnumerable<ChatUserInRoom> chatUsers = _chatService.AdminGetChatRoomUsers(new AdminGetChatRoomUsersCriteria
            {
                ChatRoomId = criteria.ChatRoomId,
                IncludingBanned = true,
                IncludingLeft = true,
                IncludingKickedOut = true
            });

            ChatRoom chatRoomInformation = _chatService.GetChatRoomByName(new GetChatRoomByNameCriteria 
            { 
                ChatRoomName = criteria.ChatRoomName,
                MatchId = criteria.MatchId
            });

            var sessionKey = string.Format(ChatMonitorSessionKey, criteria.ChatRoomId);
            HttpContext.Session[sessionKey] = chatMessages;
            return Json(new
            {
                ChatMessages = chatMessages,
                ChatUsers = chatUsers,
                ChatRoom = chatRoomInformation
            });
        }

        /// <summary>
        /// Export message to excel file.
        /// </summary>
        /// <param name="matchId">Current match id.</param>
        /// <param name="chatUserId">Current chat user id need to export.</param>
        /// <param name="searchText">Message for searching.</param>
        /// <returns>Excel result file.</returns>
        [HttpGet]
        public ExcelResult ExportExcel(
            int matchId,
            int chatUserId = 0,
            string searchText = "")
        {
            var templeteFilePath = Server.MapPath(@"~/App_Data/ExcelTemplate/MonitorChatConversationExportExcel.xls");
            var chatMonitorViewModel = GetChatMonitorViewModel(matchId);
            var sessionKey = string.Format(ChatMonitorSessionKey, chatMonitorViewModel.ChatRoom.ChatRoomId);
            var chatMessages = HttpContext.Session[sessionKey] as IEnumerable<ChatMessage>;
            var hssfWorkBook = _chatService.ExportChatConversation(
                chatMessages,
                chatMonitorViewModel.Match,
                new SearchChatMessageCriteria
                {
                    ChatRoomId = chatMonitorViewModel.ChatRoom.ChatRoomId,
                    SearchText = searchText,
                    ChatUserId = chatUserId,
                    IncludingDeleted = true
                },
                templeteFilePath);
            var exportFileName = string.Format(
                "{0}_{1}_vs_{2}_{3}",
                "Chat_Conversation",
                chatMonitorViewModel.Match.HomeName,
                chatMonitorViewModel.Match.AwayName,
                DateTime.Now.ToShortDateString());

            return new ExcelResult(hssfWorkBook, exportFileName);
        }

        [HttpPost]
        public JsonResult KickOutChatUser(int chatUserId, int chatRoomId)
        {
            return Json(_chatService.KickOutChatUserOnAdminSite(chatUserId, chatRoomId));
        }

        [HttpPost]
        public JsonResult BanChatUser(int chatUserId)
        {
            return Json(_chatService.BanChatUserOnAdminSite(chatUserId));
        }

        [HttpPost]
        public JsonResult UpdateMuteChatUser(UpdateMuteChatUserCriteria updateMuteChatUserCriteria)
        {
            return Json(_chatService.UpdateMuteChatUserOnAdminSite(updateMuteChatUserCriteria));
        }

        [HttpPost]
        public JsonResult UpdateChatRoomStatusByMatchId(UpdateChatRoomStatusByMatchIdCriteria criteria)
        {
            criteria.ChatRoomName = string.Format("{0}_{1}", criteria.MatchId, DefaultLanguage.Replace('-', '_'));
            return Json(_chatService.UpdateChatRoomStatusByMatchIdOnAdminSite(criteria));
        }

        [HttpPost]
        public JsonResult UpdateMuteChatRoom(UpdateMuteChatRoomCriteria updateMuteChatRoom)
        {
            return Json(_chatService.UpdateMuteChatRoomOnAdminSite(updateMuteChatRoom));
        }

        [HttpPost]
        public JsonResult RecoverBannedChatUser(int chatUserId)
        {
            return Json(_chatService.UpdateChatUser(new UpdateChatUserCriteria
            {
                ChatUserId = chatUserId,
                IsBanned = false,
            }));
        }

        [HttpPost]
        public JsonResult UpdateChatMessage(UpdateChatMessageCriteria updateChatCriteria)
        {
            updateChatCriteria.DeletedBy = SessionManager.CurrentSession.AdminId;
            return Json(_chatService.UpdateChatMessageOnAdminSite(updateChatCriteria));
        }

        [HttpPost]
        public JsonResult UpdateAllChatMessageByChatUser(UpdateChatMessageOfChatUserCriteria updateChatByChatUserCriteria)
        {
            updateChatByChatUserCriteria.DeletedBy = SessionManager.CurrentSession.AdminId;
            return Json(_chatService.UpdateChatMessageByChatUserOnAdminSite(updateChatByChatUserCriteria));
        }

        [HttpPost]
        public JsonResult RecoverChatUser(int chatUserId, int chatRoomId)
        {
            var updateResult = _chatService.UpdateKickOutChatUser(new UpdateKickOutChatUserCriteria
            {
                ChatRoomId = chatRoomId,
                ChatUserId = chatUserId,
                IsKickOut = false
            });
            return Json(updateResult);
        }

        [HttpPost]
        public JsonResult GetBanWords(GetBanWordsCriteria criteria)
        {
            return Json(_chatService.GetBanWords(criteria));
        }

        [HttpPost]
        public JsonResult InsertBanWords(InsertBanWordsCriteria criteria)
        {
            return Json(_chatService.InsertBanWords(criteria));
        }

        [HttpPost]
        public JsonResult UpdateBanWordText(UpdateBanWordTextCriteria criteria)
        {
            return Json(_chatService.UpdateBanWordText(criteria));
        }

        [HttpPost]
        public JsonResult UpdateBanWordStatus(UpdateBanWordStatusCriteria criteria)
        {
            return Json(_chatService.UpdateBanWordStatus(criteria));
        }

        #region Private actions

        /// <summary>
        /// Get Chat monitor view model.
        /// </summary>
        /// <param name="matchId">Current match need get chart.</param>
        /// <param name="language">Current language of room.</param>
        /// <returns>Chat monitor view model.</returns>
        private ChatMonitorViewModel GetChatMonitorViewModel(int matchId, string language = "en-US")
        {
            var chatRoom = _chatService.GetChatRoomByName(new GetChatRoomByNameCriteria
            {
                ChatRoomName = string.Format("{0}_{1}", matchId, language.Replace('-', '_')),
                MatchId = matchId
            });

            var match = _chartService.GetMatchDetail(new GetMatchDetailCriteria
            {
                MatchId = matchId
            });

            return new ChatMonitorViewModel
            {
                Match = match,
                ChatRoom = chatRoom,
                IsMatchCompleted = _matchService.IsMatchCompleted(new GetMatchStatusCriteria
                {
                    MatchId = matchId
                })
            };
        }

        #endregion Private actions
    }
}