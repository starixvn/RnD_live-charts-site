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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Controllers;
    using Fanex.LiveCharts.Criterias.Chat;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.Web;
    using Fanex.LiveCharts.Web.Filters;

    /// <summary>
    /// Chat Controller.
    /// </summary>
    ////[LiveChartAuthentication]
    public class ChatController : BaseController
    {
        private const string DefaultLanguage = "en_US";
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

        /// <summary>
        /// Default page for chat.
        /// </summary>
        /// <param name="matchId">Match id for get chat room.</param>
        /// <returns>Chat page.</returns>
        [LiveChartAuthentication]
        public ActionResult Index(int matchId)
        {
            ViewBag.MatchId = matchId;
            ChatUser chatUser = _chatService.GetChatUser(new GetChatUserCriteria
            {
                MemberId = SessionManager.CurrentSession.MemberId
            });
            //// Check User ban by default or not.
            if (chatUser.ChatUserId <= 0)
            {
                var memberInfo = _chatService.GetMemberBannedInformation(new GetMemberBannedCriteria
                {
                    MemberId = chatUser.MemberId
                });

                if (memberInfo != null && memberInfo.IsBanned)
                {
                    return RedirectToAction("KickedOut", new { matchId = matchId, chatUserStatus = (int)ChatUserStatus.Banned });
                }
            }
            else
            {
                if (chatUser.IsBanned)
                {
                    return RedirectToAction("KickedOut", new { matchId = matchId, chatUserStatus = (int)ChatUserStatus.Banned });
                }
            }

            var chatViewModel = GetChatRoomInformation(matchId, chatUser);
            if (chatViewModel.ChatUserInRoom.KickedOut)
            {
                return RedirectToAction("KickedOut", new { matchId = matchId, chatUserStatus = (int)ChatUserStatus.KickedOut });
            }
            else if (chatViewModel.ChatRoom.Disabled)
            {
                return RedirectToAction("KickedOut", new { matchId = matchId, chatUserStatus = (int)ChatUserStatus.DisableChatRoom });
            }

            return View(chatViewModel);
        }

        public ActionResult YouAreDisconnect()
        {
            return View();
        }

        public ActionResult KickedOut(int matchId, int chatUserStatus = (int)ChatUserStatus.DisableChatRoom)
        {
            return View(new KickedOutViewModel
                {
                    MatchId = matchId,
                    ChatUserStatus = (ChatUserStatus)chatUserStatus
                });
        }

        public ActionResult ChatRoomCompleted()
        {
            return View();
        }

        [HttpPost, LiveChartAuthentication]
        public JsonResult GetChatMessageByTime(int chatRoomId, DateTime? latestTime)
        {
            var result = _chatService.GetChatMessagesByTime(new GetChatMessageByTimeCriteria
            {
                ChatRoomId = chatRoomId,
                LatestTime = latestTime ?? DateTime.Now.AddMinutes(5)
            });

            return Json(result);
        }

        [HttpPost, LiveChartAuthentication]
        public JsonResult GetChatMessageByUser(GetChatMessageByUserIdCriteria getChatMessageByUserCriteria)
        {
            var result = _chatService.GetChatMessagesByUser(getChatMessageByUserCriteria);

            return Json(result);
        }

        [HttpPost, LiveChartAuthentication]
        public JsonResult UpdateAgreeTerms(UpdateChatUserAgreeTermsCriteria criteria)
        {
            criteria.AgreedTerms = true;
            var result = _chatService.UpdateChatUserAgreedTerms(criteria);

            return Json(result);
        }

        [HttpPost, LiveChartAuthentication]
        public JsonResult InsertChatUser(InsertChatUserCriteria insertChatUserCriteria)
        {
            var chatNameDenyList = _settingService.GetSetting(new GetChatNameDenyListCriteria());

            var commandResult = _chatService.InsertChatUser(insertChatUserCriteria, chatNameDenyList.SettingValue);

            return Json(commandResult);
        }

        [HttpPost, LiveChartAuthentication]
        public JsonResult UpdateChatName(UpdateChatNameCriteria criteria)
        {
            var commandResult = _chatService.UpdateChatName(criteria);

            return Json(commandResult);
        }

        /// <summary>
        /// Get information for chat user can re-enter page.
        /// </summary>
        /// <returns>Re-enter information.</returns>
        [HttpPost, LiveChartAuthentication]
        public JsonResult GetInformationForReEnterPage()
        {
            ChatUser chatUser = _chatService.GetChatUser(new GetChatUserCriteria
            {
                MemberId = SessionManager.CurrentSession.MemberId
            });
            chatUser.MemberId = SessionManager.CurrentSession.MemberId;
            if (!IsValidChatUser(chatUser) || !IsRoomValidForUserEnter(chatUser.ChatUserId, SessionManager.CurrentSession.MatchId))
            {
                return Json(false);
            }

            return Json(true);
        }

        /// <summary>
        /// Get banned words for hiding it.
        /// </summary>
        /// <returns>Banned words array.</returns>
        [HttpPost, LiveChartAuthentication]
        public JsonResult GetBannedWords()
        {
            IEnumerable<BanWord> banWordObjects = _chatService.GetBanWords(new GetBanWordsCriteria
            {
                WordStatus = 1 //// Not Include Deleted
            });

            return Json(banWordObjects.Select<BanWord, string>(w =>
            {
                return w.BanWordText;
            }));
        }

        #region Actions for Admin Site access

        [HttpPost]
        public JsonResult KickOutChatUser(KickOutChatUserViewModel kickOutChatUserViewModel)
        {
            return Json(_chatService.KickOutChatUserOnMemberSite(kickOutChatUserViewModel));
        }

        [HttpPost]
        public JsonResult BanChatUser(BanChatUserViewModel banViewModel)
        {
            return Json(_chatService.BanChatUserOnMemberSite(banViewModel));
        }

        [HttpPost]
        public JsonResult BanListUsers(UpdateBanListUserViewModel banViewModel)
        {
            return Json(_chatService.UpdateBanListUsersOnMemberSite(banViewModel));
        }

        [HttpPost]
        public JsonResult UpdateChatMessage(UpdateChatMessageViewModel deleteChatMessageViewModel)
        {
            return Json(_chatService.UpdateChatMessageOnMemberSite(deleteChatMessageViewModel));
        }

        [HttpPost]
        public JsonResult UpdateAllChatMessagesByChatUser(UpdateChatMessageByChatUserViewModel updateMessageByChatUserViewModel)
        {
            return Json(_chatService.UpdateChatMessageByChatUserIdOnMemberSite(updateMessageByChatUserViewModel));
        }

        [HttpPost]
        public JsonResult UpdateMuteChatUser(UpdateMuteChatUserViewModel updateMuteChatUserViewModel)
        {
            return Json(_chatService.UpdateMuteChatUserOnMemberSite(updateMuteChatUserViewModel));
        }

        [HttpPost]
        public JsonResult UpdateMuteChatRoom(UpdateMuteChatRoomViewModel updateMuteChatRoomViewModel)
        {
            return Json(_chatService.UpdateMuteChatRoomOnMemberSite(updateMuteChatRoomViewModel));
        }

        [HttpPost]
        public JsonResult DisableChatRoom(UpdateChatRoomStatusViewModel updateChatRoomStatus)
        {
            return Json(_chatService.UpdateChatRoomStatusOnMemberSite(updateChatRoomStatus));
        }

        [HttpPost]
        public JsonResult GetCurrentRoom(GetChatRoomByNameCriteria criteria)
        {
            criteria.ChatRoomName = string.Format("{0}_" + DefaultLanguage, criteria.MatchId);
            return Json(_chatService.GetChatRoomByName(criteria));
        }

        #endregion Actions for Admin Site access

        #region Private Actions

        /// <summary>
        /// Get chat view model from match id.
        /// </summary>
        /// <param name="matchId">Current match id.</param>
        /// <param name="chatUser">Chat user information.</param>
        /// <returns>Chat view model.</returns>
        private ChatViewModel GetChatRoomInformation(int matchId, ChatUser chatUser)
        {
            ChatRoom chatRoom = _chatService.GetChatRoomByName(new GetChatRoomByNameCriteria
            {
                ////SessionManager.CurrentSession.Language.Replace('-', '_').Trim()
                ChatRoomName = string.Format("{0}_" + DefaultLanguage, matchId),
                MatchId = matchId
            });

            SettingItem annoucementMessage = _settingService.GetSetting(new GetTermConditionCriteria());
            SettingItem welcomeMessage = _settingService.GetSetting(new GetWelcomeMessageCriteria());

            var chatUserInRoom = new ChatUserInRoom();
            if (chatUser.ChatUserId > 0)
            {
                chatUserInRoom = _chatService.GetChatUserInRoom(new GetChatUserInRoomCriteria
                {
                    ChatUserId = chatUser.ChatUserId,
                    ChatRoomId = chatRoom.ChatRoomId
                });
                if (chatUser.IsResetChatName)
                {
                    welcomeMessage.SettingValue = Lang.WelcomeMessage;
                }
                else
                {
                    welcomeMessage.SettingValue = string.Format(Lang.WelcomeMessage, chatUser.ChatName);
                }
            }
            else
            {
                welcomeMessage.SettingValue = Lang.WelcomeMessage;
            }

            return new ChatViewModel
                {
                    ChatRoom = chatRoom,
                    ChatUser = chatUser,
                    ChatUserInRoom = chatUserInRoom,
                    AnnoucementMessage = annoucementMessage,
                    WelcomeMessage = welcomeMessage
                };
        }

        /// <summary>
        /// Check valid chat user or not.
        /// </summary>
        /// <param name="chatUser">Chat user information.</param>
        /// <returns>Is chat user valid join room or not.</returns>
        private bool IsValidChatUser(ChatUser chatUser)
        {
            if (chatUser == null)
            {
                return false;
            }
            else if (chatUser.IsBanned)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check room valid for user enter.
        /// </summary>
        /// <param name="chatUserId">Current chat user id need enter room.</param>
        /// <param name="matchId">Current match for get room information.</param>
        /// <returns>User can enter room or not.</returns>
        private bool IsRoomValidForUserEnter(int chatUserId, int matchId)
        {
            var chatRoom = GetChatRoomByName(matchId);

            if (chatRoom == null || chatRoom.Disabled)
            {
                return false;
            }

            var chatUserInRoom = _chatService.GetChatUserInRoom(new GetChatUserInRoomCriteria
            {
                ChatUserId = chatUserId,
                ChatRoomId = chatRoom.ChatRoomId
            });

            if (chatUserInRoom == null || chatUserInRoom.KickedOut)
            {
                return false;
            }

            return true;
        }

        private ChatRoom GetChatRoomByName(int matchId)
        {
            ChatRoom chatRoom = _chatService.GetChatRoomByName(new GetChatRoomByNameCriteria
            {
                ////SessionManager.CurrentSession.Language.Replace('-', '_').Trim()
                ChatRoomName = string.Format("{0}_" + DefaultLanguage, matchId),
                MatchId = matchId
            });

            return chatRoom;
        }

        #endregion Private Actions
    }
}