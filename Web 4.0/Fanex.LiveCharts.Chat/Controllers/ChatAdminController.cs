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
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.Web;
    using Fanex.LiveCharts.Web.CustomActionResult;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
    public class ChatAdminController : Controller
    {
        private const string ChatMonitorSessionKey = "Chat.Controllers.ChatAdminController.ChatMonitorSessionKey_{0}";
        private IChatService _chatService;
        private IChartService _chartService;

        public ChatAdminController()
            : this(null, null)
        {
        }

        public ChatAdminController(IChatService chatService, IChartService chartService)
        {
            _chatService = chatService ?? new ChatService();
            _chartService = chartService ?? new ChartService();
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

            IEnumerable<ChatUserInRoom> chatUsers = _chatService.GetChatRoomUsers(new GetChatRoomUsersCriteria
            {
                ChatRoomId = criteria.ChatRoomId,
                IncludingBanned = true,
                IncludingLeft = true,
                IncludingKickedOut = true
            });

            var sessionKey = string.Format(ChatMonitorSessionKey, criteria.ChatRoomId);
            HttpContext.Session[sessionKey] = chatMessages;
            return Json(new
            {
                ChatMessages = chatMessages,
                ChatUsers = chatUsers
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
        public JsonResult UpdateAllChatMessageByChatUser(UpdateChatMessageByChatUserCriteria updateChatByChatUserCriteria)
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

        #region Private actions

        /// <summary>
        /// Get Chat monitor view model.
        /// </summary>
        /// <param name="matchId">Current match need get chart.</param>
        /// <returns>Chat monitor view model.</returns>
        private ChatMonitorViewModel GetChatMonitorViewModel(int matchId)
        {
            var chatRooms = _chatService.GetChatRoomByMatchId(new GetChatRoomByMatchIdCriteria
            {
                MatchId = matchId
            });

            //// If there are many rooms, then get first room.
            var defaultChatRoom = new ChatRoom();
            if (chatRooms.Any())
            {
                defaultChatRoom = chatRooms.ElementAt(0);
            }

            var match = _chartService.GetMatchDetail(new GetMatchDetailCriteria
            {
                MatchId = matchId
            });

            return new ChatMonitorViewModel
            {
                Match = match,
                ChatRoom = defaultChatRoom
            };
        }

        #endregion Private actions
    }
}