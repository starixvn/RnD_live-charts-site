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
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Web.CustomActionResult;
    using Fanex.LiveCharts.Resources;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.LiveCharts.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
    public class ChatAdminController : Controller
    {
        private IChatService _chatService;
        private IChartService _chartService;
        private const string ChatMonitorSessionKey = "Chat.Controllers.ChatAdminController.ChatMonitorSessionKey_{0}";

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
            var chatMonitorViewModel = GetChatMonitorViewMode(matchId);

            return View("~/Areas/Admin/Views/Chat/ChatMonitor.cshtml", chatMonitorViewModel);
        }

        [HttpPost]
        public JsonResult GetChatMessages(int chatRoomId)
        {
            IEnumerable<ChatMessage> chatMessages = _chatService.GetChatMessageByRoom(new GetChatMessageByRoomCriteria
            {
                ChatRoomId = chatRoomId,
                IsDeleted = true
            });

            IEnumerable<ChatUserInRoom> chatUsers = _chatService.GetChatRoomUsers(new GetChatRoomUsersCriteria { 
                ChatRoomId = chatRoomId,
                IncludingBanned = true,
                IncludingLeft = true,
                IncludingKickedOut = true
            });

            var sessionKey = string.Format(ChatMonitorSessionKey, chatRoomId);
            HttpContext.Session[sessionKey] = chatMessages;
            return Json(new
            {
                ChatMessages = chatMessages,
                ChatUsers = chatUsers
                
            });
        }

        [HttpGet]
        public ExcelResult ExportExcel(int matchId) {
            var templeteFilePath = Server.MapPath(@"~/App_Data/ExcelTemplate/MonitorChatConversationExportExcel.xls");  
            var chatMonitorViewModel = GetChatMonitorViewMode(matchId);
            var sessionKey = string.Format(ChatMonitorSessionKey, chatMonitorViewModel.ChatRoom.ChatRoomId);
            var chatMessages = HttpContext.Session[sessionKey] as IEnumerable<ChatMessage>;
            var hssfWorkBook = _chatService.ExportChatConversation(chatMessages, chatMonitorViewModel.Match, chatMonitorViewModel.ChatRoom, templeteFilePath);
            var exportFileName = string.Format("{0}_{1}_vs_{2}_{3}",
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
        public JsonResult DeleteChatMessage(int chatMessageId, string chatRoomName)
        {
            return Json(_chatService.DeleteChatMessageOnAdminSite(chatMessageId, chatRoomName, SessionManager.CurrentSession.AdminId));
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
        public JsonResult RecoverChatMessage(int chatMessageId)
        {
            var updateResult = _chatService.UpdateChatMessage(new UpdateChatMessageCriteria
            {
                ChatMessageId = chatMessageId,
                IsDelete = false,
                DeletedBy = SessionManager.CurrentSession.AdminId
            });
            return Json(updateResult);
        }

        #region Private actions
        private ChatMonitorViewModel GetChatMonitorViewMode(int matchId) { 
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

            var match = _chartService.GetMatchDetail(new GetMatchDetailCriteria { 
                MatchId = matchId
            });

            return new ChatMonitorViewModel { 
                Match = match,
                ChatRoom = defaultChatRoom
            };
        }

        #endregion

    }
}