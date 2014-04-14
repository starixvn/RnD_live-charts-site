// --------------------------------------------------------------------------
// <copyright file="ChatService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.20">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Hubs;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Repositories;
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.NAP.Framework.Data;
    using Microsoft.AspNet.SignalR;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatService : IChatService
    {
        private const string KickOutCommandCacheKey = "Chat.Services.ChatService.KickOutCommandCacheKey";
        private const string DeleteMessageCommandCacheKey = "Chat.Services.ChatService.DeleteMessageCommandCacheKey";
        private const string BanChatUserCacheKey = "Chat.Services.ChatService.BanChatUserCacheKey";
        private const string DeleteMessageByUserCommandCacheKey = "Chat.Services.ChatService.DeleteMessageByUserCommandCacheKey";
        private readonly IChatRepository _chatRepository;

        public ChatService()
            : this(null)
        {
        }

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? new ChatRepository();
        }

        public IEnumerable<ChatMessage> GetChatMessagesByTime(GetChatMessageByTimeCriteria getChatMessageCriteria)
        {
            var chatMessage = _chatRepository.GetChatMessages(getChatMessageCriteria);

            return chatMessage;
        }

        public IEnumerable<ChatMessage> GetChatMessagesByUser(GetChatMessageByUserIdCriteria getChatMessageCriteria)
        {
            var chatMessage = _chatRepository.Fetch<ChatMessage>(getChatMessageCriteria);

            return chatMessage;
        }

        /// <summary>
        /// Get chat user information from member id.
        /// </summary>
        /// <param name="getChatUserCriteria">Criteria for get data.</param>
        /// <returns>Chat user information.</returns>
        public ChatUser GetChatUser(GetChatUserCriteria getChatUserCriteria)
        {
            var chatUser = _chatRepository.Get<ChatUser>(getChatUserCriteria);
            if (chatUser == null)
            {
                chatUser = new ChatUser
                {
                    MemberId = getChatUserCriteria.MemberId
                };
            }

            return chatUser;
        }

        /// <summary>
        /// Insert chat message to database.
        /// </summary>
        /// <param name="insertChatMessageCriteria">Insert chat message criteria.</param>
        /// <returns>Status of insert command.</returns>
        public InsertResult InsertChatMessage(InsertChatMessageCriteria insertChatMessageCriteria)
        {
            ////insertChatMessageCriteria.CreateTime = DateTime.Now;

            var insertResult = _chatRepository.ModifyData(insertChatMessageCriteria, "ChatMessageId");

            return insertResult;
        }

        public InsertResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria)
        {
            ////insertChatRoomCriteria.CreateTime = DateTime.Now;
            var insertResult = _chatRepository.ModifyData(insertChatRoomCriteria, "ChatRoomId");

            return insertResult;
        }

        /// <summary>
        /// Insert chat user.
        /// </summary>
        /// <param name="insertchatUserCriteria">Data for insert.</param>
        /// <returns>Insert command result.</returns>
        public InsertResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria)
        {
            ////insertchatUserCriteria.CreateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(insertchatUserCriteria.ChatName))
            {
                insertchatUserCriteria.ChatName = HttpUtility.HtmlEncode(insertchatUserCriteria.ChatName);
            }

            var insertResult = _chatRepository.ModifyData(insertchatUserCriteria, "ChatUserId");

            return insertResult;
        }

        public ICommandResult UpdateChatMessage(UpdateChatMessageCriteria updateChatMessageCriteria)
        {
            var updateResult = _chatRepository.ModifyData(updateChatMessageCriteria);

            return updateResult;
        }

        public ICommandResult UpdateChatMessageByChatUser(UpdateChatMessageByChatUserCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            return updateResult;
        }

        public ICommandResult UpdateChatRoom(UpdateChatRoomCriteria updateChatRoomCriteria)
        {
            if (!string.IsNullOrEmpty(updateChatRoomCriteria.WelcomeMessage))
            {
                updateChatRoomCriteria.WelcomeMessage = HttpUtility.HtmlEncode(updateChatRoomCriteria.WelcomeMessage);
            }

            var updateResult = _chatRepository.ModifyData(updateChatRoomCriteria);

            return updateResult;
        }

        public ICommandResult UpdateChatUser(UpdateChatUserCriteria updateChatUserCriteria)
        {
            ////updateChatUserCriteria.ModifiedTime = DateTime.Now;
            var updateResult = _chatRepository.ModifyData(updateChatUserCriteria);

            return updateResult;
        }

        public ChatRoom GetChatRoomByName(GetChatRoomByNameCriteria getChatRoomByName)
        {
            var chatRoom = _chatRepository.Get<ChatRoom>(getChatRoomByName);

            return chatRoom;
        }

        public IEnumerable<ChatUserInRoom> GetChatRoomUsers(GetChatRoomUsersCriteria getChatRoomUsersCriteria)
        {
            var chatUsers = _chatRepository.GetUsersInRoom(getChatRoomUsersCriteria);

            return chatUsers;
        }

        public InsertResult InsertChatUserInRoom(InsertChatUserInRoomCriteria insertChatUserInRoomCriteria)
        {
            ////insertChatUserInRoomCriteria.LastOnlineTime = DateTime.Now;
            var insertResult = _chatRepository.ModifyData(insertChatUserInRoomCriteria, "ChatUserInRoomId");

            return insertResult;
        }

        public ICommandResult UpdateChatUserInRoom(UpdateChatUserInRoomCriteria updateChatUserInRoomCriteria)
        {
            ////updateChatUserInRoomCriteria.LastOnlineTime = DateTime.Now;
            var updateResult = _chatRepository.ModifyData(updateChatUserInRoomCriteria);

            return updateResult;
        }

        public ICommandResult UpdateKickOutChatUser(UpdateKickOutChatUserCriteria updateKickOutChatUserCriteria)
        {
            var updateResult = _chatRepository.ModifyData(updateKickOutChatUserCriteria);

            return updateResult;
        }

        /// <summary>
        /// Get chat user in room from chat user id and chat room.
        /// </summary>
        /// <param name="getChatUserInRoomCriteria">Criteria for getting data.</param>
        /// <returns>Current information of chat user in room.</returns>
        public ChatUserInRoom GetChatUserInRoom(GetChatUserInRoomCriteria getChatUserInRoomCriteria)
        {
            var chatUserInRoom = _chatRepository.Get<ChatUserInRoom>(getChatUserInRoomCriteria);

            if (chatUserInRoom == null)
            {
                chatUserInRoom = new ChatUserInRoom
                {
                    ChatRoom = new ChatRoom
                    {
                        ChatRoomId = getChatUserInRoomCriteria.ChatRoomId
                    },
                    ChatUser = new ChatUser
                    {
                        ChatUserId = getChatUserInRoomCriteria.ChatUserId
                    }
                };
            }

            return chatUserInRoom;
        }

        public ChatUserInRoom GetChatUserInRoomByConnectionId(GetChatUserInRoomByConnectionIdCriteria getChatUserInRoomByConnectionIdCriteria)
        {
            var chatUserInRoom = _chatRepository.GetUsersInRoom(getChatUserInRoomByConnectionIdCriteria).FirstOrDefault();

            return chatUserInRoom;
        }

        public IEnumerable<ChatMessage> GetChatMessageByRoom(GetChatMessageByRoomCriteria getChatMessageByRoomCriteria)
        {
            var chatMessages = _chatRepository.GetChatMessages(getChatMessageByRoomCriteria);

            return chatMessages;
        }

        public IEnumerable<ChatRoom> GetChatRoomByMatchId(GetChatRoomByMatchIdCriteria getChatRoomByMatchIdCriteria)
        {
            var chatRooms = _chatRepository.Fetch<ChatRoom>(getChatRoomByMatchIdCriteria);

            return chatRooms;
        }

        /// <summary>
        /// Kick out chat user online on member site.
        /// </summary>
        /// <param name="kickOutChatUser">View model contains all information of kick out chat user.</param>
        /// <returns>Status of kick out command.</returns>
        public ICommandResult KickOutChatUserOnMemberSite(KickOutChatUserViewModel kickOutChatUser)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(KickOutCommandCacheKey + "_" + kickOutChatUser.TokenName, kickOutChatUser.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.Client(kickOutChatUser.ConnectionId).kickOutChatUser(kickOutChatUser.ChatUserId);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = 11,
                    Message = ex.Message
                };
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
            };
        }

        /// <summary>
        /// Delete chat message on member site.
        /// </summary>
        /// <param name="deleteChatMessage">Model contains information for delete chat message on member site.</param>
        /// <returns>Delete chat message result.</returns>
        public ICommandResult DeleteChatMessageOnMemberSite(DeleteChatMessageViewModel deleteChatMessage)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(DeleteMessageCommandCacheKey + "_" + deleteChatMessage.TokenName, deleteChatMessage.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.Group(deleteChatMessage.ChatRoomName).deleteChatMessage(deleteChatMessage.ChatMessageId);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = 11,
                    Message = ex.Message
                };
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
            };
        }

        /// <summary>
        /// Delete group of chat message from chat user.
        /// </summary>
        /// <param name="deleteMessageByChatUser">Model contains information for deleting message on member site.</param>
        /// <returns>Status of delete command.</returns>
        public ICommandResult DeleteChatMessageByChatUserIdOnMemberSite(DeleteChatMessageByChatUserViewModel deleteMessageByChatUser)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(
                    DeleteMessageByUserCommandCacheKey + "_" + deleteMessageByChatUser.TokenName, 
                    deleteMessageByChatUser.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.Group(deleteMessageByChatUser.ChatRoomName).deleteChatMessageByChatUser(deleteMessageByChatUser.ChatUserId);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = 11,
                    Message = ex.Message
                };
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
            };
        }

        /// <summary>
        /// Kick out chat user on admin site, just update database and then call member site.
        /// </summary>
        /// <param name="chatUserId">Chat user id for kicking out.</param>
        /// <param name="chatRoomId">Chat room id for get connection id.</param>
        /// <returns>Kick out information.</returns>
        public KickOutChatUserViewModel KickOutChatUserOnAdminSite(int chatUserId, int chatRoomId)
        {
            ICommandResult kickOutResult = UpdateKickOutChatUser(new UpdateKickOutChatUserCriteria
            {
                ChatUserId = chatUserId,
                ChatRoomId = chatRoomId,
                IsKickOut = true
            });

            var kickOutChatUserStatus = new KickOutChatUserViewModel
            {
                CommandResult = kickOutResult
            };

            if (kickOutResult.IsSuccessful)
            {
                ChatUserInRoom chatUserInRoom = _chatRepository.Get<ChatUserInRoom>(new GetChatUserInRoomCriteria
                {
                    ChatRoomId = chatRoomId,
                    ChatUserId = chatUserId
                });

                if (chatUserInRoom != null)
                {
                    kickOutChatUserStatus.NeedToSendRequestKickOut = !chatUserInRoom.IsLeft;
                    kickOutChatUserStatus.ChatUserId = chatUserId;
                    kickOutChatUserStatus.ConnectionId = chatUserInRoom.ConnectionId;
                    SetKickOutToken(kickOutChatUserStatus, chatRoomId);
                }
            }

            return kickOutChatUserStatus;
        }

        /// <summary>
        /// Delete chat message on admin site, just update database and then call member site.
        /// </summary>
        /// <param name="chatMessageId">Chat message id for update.</param>
        /// <param name="chatRoomName">Chat room name for broadcast delete command.</param>
        /// <param name="deleteBy">Delete by admin id.</param>
        /// <returns>Delete chat message information.</returns>
        public DeleteChatMessageViewModel DeleteChatMessageOnAdminSite(int chatMessageId, string chatRoomName, int deleteBy)
        {
            ICommandResult deleteMessageResult = UpdateChatMessage(new UpdateChatMessageCriteria
            {
                ChatMessageId = chatMessageId,
                IsDelete = true,
                DeletedBy = deleteBy
            });

            var deleteMessageViewModel = new DeleteChatMessageViewModel
            {
                CommandResult = deleteMessageResult
            };

            if (deleteMessageResult.IsSuccessful)
            {
                deleteMessageViewModel.ChatMessageId = chatMessageId;
                deleteMessageViewModel.ChatRoomName = chatRoomName;
                SetDeleleMessageToken(deleteMessageViewModel);
            }

            return deleteMessageViewModel;
        }

        /// <summary>
        /// Delete Chat Message on admin site, and then remove that message on client site.
        /// </summary>
        /// <param name="chatUserId">Chat User id need removed message.</param>
        /// <param name="chatRoomName">Chat room name need remove.</param>
        /// <param name="deleteBy">Which admin delete.</param>
        /// <returns>Delete status.</returns>
        public DeleteChatMessageByChatUserViewModel DeleteChatMessageByChatUserOnAdminSite(int chatUserId, string chatRoomName, int deleteBy)
        {
            ICommandResult deleteMessageResult = UpdateChatMessageByChatUser(new UpdateChatMessageByChatUserCriteria
                {
                    ChatUserId = chatUserId,
                    DeletedBy = deleteBy,
                    IsDelete = true
                });

            var deleteMessageViewModel = new DeleteChatMessageByChatUserViewModel
            {
                CommandResult = deleteMessageResult,
                ChatUserId = chatUserId,
                ChatRoomName = chatRoomName
            };

            if (deleteMessageResult.IsSuccessful)
            {
                SetDeleleMessageByChatUserToken(deleteMessageViewModel);
            }

            return deleteMessageViewModel;
        }

        /// <summary>
        /// Ban chat user on admin site.
        /// </summary>
        /// <param name="chatUserId">Chat user be banned.</param>
        /// <returns>Ban information for remove chat user at member site.</returns>
        public BanChatUserViewModel BanChatUserOnAdminSite(int chatUserId)
        {
            ICommandResult updateChatUserStatus = UpdateChatUser(new UpdateChatUserCriteria
            {
                ChatUserId = chatUserId,
                IsBanned = true
            });

            var banChatUserViewModel = new BanChatUserViewModel
            {
                CommandResult = updateChatUserStatus,
                ChatUserId = chatUserId
            };

            if (updateChatUserStatus.IsSuccessful)
            {
                SetBanChatUserToken(banChatUserViewModel);
            }

            return banChatUserViewModel;
        }

        /// <summary>
        /// Kick out chat user after banned in member site.
        /// </summary>
        /// <param name="banChatUser">Chat user information for kicking out.</param>
        /// <returns>Kick out status.</returns>
        public ICommandResult BanChatUserOnMemberSite(BanChatUserViewModel banChatUser)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(BanChatUserCacheKey + "_" + banChatUser.TokenName, banChatUser.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.All.banChatUser(banChatUser.ChatUserId);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = 11,
                    Message = ex.Message
                };
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
            };
        }

        public IEnumerable<ChatMessage> SearchChatMessage(SearchChatMessageCriteria criteria)
        {
            if (criteria.SearchText == null)
            {
                criteria.SearchText = string.Empty;
            }

            var chatMessages = _chatRepository.GetChatMessages(criteria);

            return chatMessages;
        }

        #region Export Excel

        /// <summary>
        /// Export chat conversation.
        /// </summary>
        /// <param name="chatMessages">Message list for export excel file.</param>
        /// <param name="match">Current match information for export excel.</param>
        /// <param name="criteria">Chat room information.</param>
        /// <param name="templeteFilePath">Template file path for reading style.</param>
        /// <returns>Workbook details.</returns>
        public HSSFWorkbook ExportChatConversation(
            IEnumerable<ChatMessage> chatMessages,
            Match match,
            SearchChatMessageCriteria criteria,
            string templeteFilePath)
        {
            if (chatMessages == null)
            {
                chatMessages = SearchChatMessage(criteria);
            }

            FileStream file = new FileStream(templeteFilePath, FileMode.Open, FileAccess.Read);

            var hssfWorkBook = new HSSFWorkbook(file);
            ISheet templeteSheet = hssfWorkBook.GetSheetAt(0);
            ExportChatConversationHeader(templeteSheet, match);
            ExportChatConversationBody(templeteSheet, chatMessages);

            return hssfWorkBook;
        }

        /// <summary>
        /// Export conversation body data.
        /// </summary>
        /// <param name="chatConversationSheet">Sheet for export excel.</param>
        /// <param name="chatMessages">List chat message for exporting excel.</param>
        private void ExportChatConversationBody(
            ISheet chatConversationSheet,
            IEnumerable<ChatMessage> chatMessages)
        {
            var fromIndex = 3;
            IRow templateRow = chatConversationSheet.GetRow(fromIndex);
            if (chatMessages.Any())
            {
                var timeCellStyle = templateRow.GetCell(0).CellStyle;
                var contentCellStyle = templateRow.GetCell(3).CellStyle;

                foreach (var chatMessage in chatMessages)
                {
                    var messageRow = chatConversationSheet.CreateRow(fromIndex++);
                    var timeCell = messageRow.CreateCell(0);
                    timeCell.SetCellValue(chatMessage.CreateTime.ToGMT8());
                    timeCell.CellStyle = timeCellStyle;
                    var usernameCell = messageRow.CreateCell(1);
                    usernameCell.SetCellValue(chatMessage.ChatUser.ChatUserId);
                    usernameCell.CellStyle = timeCellStyle;
                    var chatNameCell = messageRow.CreateCell(2);
                    chatNameCell.SetCellValue(chatMessage.ChatUser.ChatName);
                    chatNameCell.CellStyle = timeCellStyle;

                    var contentCell = messageRow.CreateCell(3);
                    contentCell.SetCellValue(chatMessage.Content);
                    contentCell.CellStyle = contentCellStyle;
                }
            }
            else
            {
                templateRow.GetCell(0).SetCellValue(AdminSiteLang.NoInfoAvailable);
                templateRow.GetCell(0).CellStyle.Alignment = HorizontalAlignment.Center;
                chatConversationSheet.AddMergedRegion(new CellRangeAddress(fromIndex, fromIndex, 0, 3));
            }
        }

        /// <summary>
        /// Export header data of Chat Conversation.
        /// </summary>
        /// <param name="chatConversationSheet">Chat conversation sheet contains data.</param>
        /// <param name="match">Current match information.</param>
        private void ExportChatConversationHeader(ISheet chatConversationSheet, Match match)
        {
            var headerRow = chatConversationSheet.GetRow(0);
            var chatRoomInformation = string.Format(
                "{0} -vs- {1}, {2}(GMT+8): {3}",
                match.HomeName,
                match.AwayName,
                AdminSiteLang.KickOffTime,
                match.KickOffTimeGMT8);
            headerRow.GetCell(0).SetCellValue(chatRoomInformation);
            var exportTimeRow = chatConversationSheet.GetRow(1);
            exportTimeRow.GetCell(0).SetCellValue(DateTime.Now.ToGMT8());
        }

        #endregion Export Excel

        #region Private Functions

        /// <summary>
        /// Check chat command is valid or not.
        /// </summary>
        /// <param name="tokenName">Token name for authentication.</param>
        /// <param name="tokenKey">Token key for authentication.</param>
        /// <returns>Command valid or not.</returns>
        private ICommandResult CheckChatCommandTokenInValid(string tokenName, string tokenKey)
        {
            var cachedToken = CacheManager.Instance.Get<string>(tokenName);

            if (string.IsNullOrEmpty(cachedToken) || cachedToken != tokenKey)
            {
                return new CommandResult
                {
                    Code = 10,
                    Message = AdminSiteLang.InvalidToken
                };
            }

            return null;
        }

        private void SetKickOutToken(KickOutChatUserViewModel kickOutViewModel, int chatRoomId)
        {
            kickOutViewModel.TokenName = string.Format("{0}_{1}", kickOutViewModel.ChatUserId, chatRoomId);
            kickOutViewModel.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                KickOutCommandCacheKey + "_" + kickOutViewModel.TokenName,
                kickOutViewModel.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        private void SetDeleleMessageToken(DeleteChatMessageViewModel deleteMessageViewModel)
        {
            deleteMessageViewModel.TokenName = string.Format("{0}_{1}", deleteMessageViewModel.ChatMessageId, deleteMessageViewModel.ChatRoomName);
            deleteMessageViewModel.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                DeleteMessageCommandCacheKey + "_" + deleteMessageViewModel.TokenName,
                deleteMessageViewModel.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        private void SetDeleleMessageByChatUserToken(DeleteChatMessageByChatUserViewModel deleteMessageViewModel)
        {
            deleteMessageViewModel.TokenName = string.Format("{0}_{1}", deleteMessageViewModel.ChatUserId, deleteMessageViewModel.ChatRoomName);
            deleteMessageViewModel.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                DeleteMessageByUserCommandCacheKey + "_" + deleteMessageViewModel.TokenName,
                deleteMessageViewModel.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        private void SetBanChatUserToken(BanChatUserViewModel banChatUserViewModel)
        {
            banChatUserViewModel.TokenName = string.Format("{0}", banChatUserViewModel.ChatUserId);
            banChatUserViewModel.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                BanChatUserCacheKey + "_" + banChatUserViewModel.TokenName,
                banChatUserViewModel.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        #endregion Private Functions
    }
}