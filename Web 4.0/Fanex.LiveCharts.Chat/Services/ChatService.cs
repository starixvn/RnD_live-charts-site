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
    using Fanex.Common;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Hubs;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Repositories;
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.NAP.Framework.Command;
    using Microsoft.AspNet.SignalR;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Chat Service.
    /// </summary>
    public class ChatService : IChatService
    {
        private const string KickOutCommandCacheKey = "Chat.Services.ChatService.KickOutCommandCacheKey";
        private const string UpdateBanListUserCacheKey = "Chat.Services.ChatService.UpdateBanListUserCacheKey";
        private const string UpdateMessageCommandCacheKey = "Chat.Services.ChatService.DeleteMessageCommandCacheKey";
        private const string BanChatUserCacheKey = "Chat.Services.ChatService.BanChatUserCacheKey";
        private const string UpdateMessageByUserCommandCacheKey = "Chat.Services.ChatService.DeleteMessageByUserCommandCacheKey";
        private const string UpdateMuteChatUserCommandCacheKey = "Chat.Services.ChatService.UpdateMuteChatUserCommandCacheKey";
        private const string UpdateMuteChatRoomCommandCacheKey = "Chat.Services.ChatService.UpdateMuteChatRoomCommandCacheKey";
        private const string UpdateChatRoomStatusCommandCacheKey = "Chat.Services.ChatService.UpdateChatRoomStatusCommandCacheKey";
        private const int CodeFail = 10;
        private readonly IChatRepository _chatRepository;

        public ChatService()
            : this(null)
        {
        }

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? new ChatRepository();
        }

        #region Get Data

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
            else
            {
                chatUser.ChatName = HttpUtility.HtmlDecode(chatUser.ChatName);
            }

            return chatUser;
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

        public IEnumerable<ChatUserInRoom> AdminGetChatRoomUsers(AdminGetChatRoomUsersCriteria adminGetChatRoomUsersCriteria)
        {
            var chatUsers = _chatRepository.GetUsersInRoom(adminGetChatRoomUsersCriteria);

            return chatUsers;
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

        public IEnumerable<ChatUser> SearchChatUser(SearchChatUserCriteria criteria)
        {
            var chatUsers = _chatRepository.Fetch<ChatUser>(criteria);

            return chatUsers;
        }

        public IEnumerable<MemberInfo> SearchMember(SearchMemberCriteria criteria)
        {
            var members = _chatRepository.Fetch<MemberInfo>(criteria);

            return members;
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

        public MemberInfo GetMemberBannedInformation(GetMemberBannedCriteria criteria)
        {
            var memberInfo = _chatRepository.Get<MemberInfo>(criteria);
            return memberInfo;
        }

        #endregion Get Data

        #region Modify Data

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
                    Code = CodeFail,
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
        /// <param name="updateChatMessage">Model contains information for delete chat message on member site.</param>
        /// <returns>Delete chat message result.</returns>
        public ICommandResult UpdateChatMessageOnMemberSite(UpdateChatMessageViewModel updateChatMessage)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(UpdateMessageCommandCacheKey + "_" + updateChatMessage.TokenName, updateChatMessage.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.Group(updateChatMessage.ChatRoomName).updateChatMessage(updateChatMessage.ChatMessageId, updateChatMessage.IsDeleted);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = CodeFail,
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
        /// <param name="updateMessageByChatUser">Model contains information for deleting message on member site.</param>
        /// <returns>Status of delete command.</returns>
        public ICommandResult UpdateChatMessageByChatUserIdOnMemberSite(UpdateChatMessageByChatUserViewModel updateMessageByChatUser)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(
                    UpdateMessageByUserCommandCacheKey + "_" + updateMessageByChatUser.TokenName,
                    updateMessageByChatUser.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.Group(updateMessageByChatUser.ChatRoomName)
                        .updateAllChatMessageByChatUser(updateMessageByChatUser.ChatUserId, updateMessageByChatUser.IsDeleted);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = CodeFail,
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
                CommandResult = (CommandResult)kickOutResult
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
        /// <param name="updateChatMessageCriteria">Chat message id for update.</param>
        /// <returns>Delete chat message information.</returns>
        public UpdateChatMessageViewModel UpdateChatMessageOnAdminSite(UpdateChatMessageCriteria updateChatMessageCriteria)
        {
            ICommandResult deleteMessageResult = UpdateChatMessage(updateChatMessageCriteria);

            var deleteMessageViewModel = new UpdateChatMessageViewModel
            {
                CommandResult = (CommandResult)deleteMessageResult,
                IsDeleted = updateChatMessageCriteria.IsDelete
            };

            if (deleteMessageResult.IsSuccessful)
            {
                deleteMessageViewModel.ChatMessageId = updateChatMessageCriteria.ChatMessageId;
                deleteMessageViewModel.ChatRoomName = updateChatMessageCriteria.ChatRoomName;
                SetDeleleMessageToken(deleteMessageViewModel);
            }

            return deleteMessageViewModel;
        }

        /// <summary>
        /// Delete Chat Message on admin site, and then remove that message on client site.
        /// </summary>
        /// <param name="updateChatMessageByChatUserCriteria">Chat User id need removed message.</param>
        /// <returns>Delete status.</returns>
        public UpdateChatMessageByChatUserViewModel UpdateChatMessageByChatUserOnAdminSite(UpdateChatMessageOfChatUserCriteria updateChatMessageByChatUserCriteria)
        {
            ICommandResult deleteMessageResult = UpdateChatMessageByChatUser(updateChatMessageByChatUserCriteria);

            var deleteMessageViewModel = new UpdateChatMessageByChatUserViewModel
            {
                CommandResult = (CommandResult)deleteMessageResult,
                ChatUserId = updateChatMessageByChatUserCriteria.ChatUserId,
                ChatRoomName = updateChatMessageByChatUserCriteria.ChatRoomName,
                IsDeleted = updateChatMessageByChatUserCriteria.IsDelete
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
                CommandResult = (CommandResult)updateChatUserStatus,
                ChatUserId = chatUserId
            };

            if (updateChatUserStatus.IsSuccessful)
            {
                SetBanChatUserToken(banChatUserViewModel);
            }

            return banChatUserViewModel;
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
                    Code = CodeFail,
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
        /// Update mute or not for chat user in member site.
        /// </summary>
        /// <param name="muteChatUser">Mute chat user information.</param>
        /// <returns>Status of command.</returns>
        public ICommandResult UpdateMuteChatUserOnMemberSite(UpdateMuteChatUserViewModel muteChatUser)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(UpdateMuteChatUserCommandCacheKey + "_" + muteChatUser.TokenName, muteChatUser.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.Group(muteChatUser.ChatRoomName).updateMuteChatUser(muteChatUser.ChatUserId, muteChatUser.Muted);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = CodeFail,
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
        /// Update mute or not for chat room in member site.
        /// </summary>
        /// <param name="muteChatRoom">Mute chat room information.</param>
        /// <returns>Status of command.</returns>
        public ICommandResult UpdateMuteChatRoomOnMemberSite(UpdateMuteChatRoomViewModel muteChatRoom)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(UpdateMuteChatRoomCommandCacheKey + "_" + muteChatRoom.TokenName, muteChatRoom.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    chatHub.Clients.Group(muteChatRoom.ChatRoomName).updateMuteChatRoom(muteChatRoom.ChatRoomId, muteChatRoom.Muted);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = CodeFail,
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
        /// Update kick out user banned on member site.
        /// </summary>
        /// <param name="banListUser">View model contains information.</param>
        /// <returns>Result of command.</returns>
        public ICommandResult UpdateBanListUsersOnMemberSite(UpdateBanListUserViewModel banListUser)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(UpdateBanListUserCacheKey + "_" + banListUser.TokenName, banListUser.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null)
                {
                    var userList = banListUser.ChatUserIds.Split(new char[] { ',' });
                    foreach (var userIdString in userList)
                    {
                        var userId = Converter.ToInt(userIdString, 0);
                        if (userId != 0)
                        {
                            chatHub.Clients.All.kickOutChatUser(userId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = CodeFail,
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
        /// Update Chat Room status on member site.
        /// </summary>
        /// <param name="chatRoomStatus">Current update status.</param>
        /// <returns>Command Status.</returns>
        public ICommandResult UpdateChatRoomStatusOnMemberSite(UpdateChatRoomStatusViewModel chatRoomStatus)
        {
            try
            {
                ICommandResult invalidTokenResult = CheckChatCommandTokenInValid(UpdateChatRoomStatusCommandCacheKey + "_" + chatRoomStatus.TokenName, chatRoomStatus.TokenKey);
                if (invalidTokenResult != null)
                {
                    return invalidTokenResult;
                }

                IHubContext chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                if (chatHub != null && chatRoomStatus.Disabled)
                {
                    chatHub.Clients.Group(chatRoomStatus.ChatRoomName).disableChatRoom(chatRoomStatus.ChatRoomId);
                }
            }
            catch (Exception ex)
            {
                ServiceBase.ExceptionHandler(ex);
                return new CommandResult
                {
                    Code = CodeFail,
                    Message = ex.Message
                };
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
            };
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
        /// Update Mute Chat user.
        /// </summary>
        /// <param name="criteria">Mute information.</param>
        /// <returns>Update mute information for using in member site.</returns>
        public UpdateMuteChatUserViewModel UpdateMuteChatUserOnAdminSite(UpdateMuteChatUserCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            var updateMuteChatUserViewModel = new UpdateMuteChatUserViewModel
            {
                CommandResult = (CommandResult)updateResult,
                ChatUserId = criteria.ChatUserId,
                ChatRoomId = criteria.ChatRoomId,
                ChatRoomName = criteria.ChatRoomName,
                Muted = criteria.IsMute
            };

            if (updateResult.IsSuccessful)
            {
                SetUpdateMuteChatUserToken(updateMuteChatUserViewModel);
            }

            return updateMuteChatUserViewModel;
        }

        /// <summary>
        /// Update mute chat room information.
        /// </summary>
        /// <param name="criteria">Mute information.</param>
        /// <returns>Mute information for using in member site.</returns>
        public UpdateMuteChatRoomViewModel UpdateMuteChatRoomOnAdminSite(UpdateMuteChatRoomCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            var updateMuteChatRoomViewModel = new UpdateMuteChatRoomViewModel
            {
                CommandResult = (CommandResult)updateResult,
                ChatRoomId = criteria.ChatRoomId,
                ChatRoomName = criteria.ChatRoomName,
                Muted = criteria.IsMute
            };

            if (updateResult.IsSuccessful)
            {
                SetUpdateMuteChatRoomToken(updateMuteChatRoomViewModel);
            }

            return updateMuteChatRoomViewModel;
        }

        public ICommandResult UpdateChatUserAgreedTerms(UpdateChatUserAgreeTermsCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            return updateResult;
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
        /// <param name="chatNameDenyList">Chat name deny list.</param>
        /// <returns>Insert command result.</returns>
        public InsertResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria, string chatNameDenyList)
        {
            if (CheckChatNameViolateRule(insertchatUserCriteria.ChatName, chatNameDenyList))
            {
                return new InsertResult
                {
                    CommandResult = new CommandResult
                    {
                        Code = CodeFail,
                        Message = Lang.ChatNameViolateMessage
                    }
                };
            }

            ////insertchatUserCriteria.CreateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(insertchatUserCriteria.ChatName))
            {
                insertchatUserCriteria.ChatName = HttpUtility.HtmlEncode(insertchatUserCriteria.ChatName);
            }

            var insertResult = _chatRepository.ModifyData(insertchatUserCriteria, "ChatUserId");

            switch ((int)insertResult.CommandResult.Code)
            {
                case 1001: //// Chat Name is not available.
                    insertResult.CommandResult = new CommandResult
                    {
                        Code = CodeFail,
                        Message = Lang.InsertUserErrorMessage1001
                    };
                    break;
                case 1002: //// Already have chat name.
                    new CommandResult
                    {
                        Code = CodeFail,
                        Message = Lang.InsertUserErrorMessage1002
                    };
                    break;
            }

            return insertResult;
        }

        public ICommandResult UpdateChatMessage(UpdateChatMessageCriteria updateChatMessageCriteria)
        {
            var updateResult = _chatRepository.ModifyData(updateChatMessageCriteria);

            return updateResult;
        }

        public ICommandResult UpdateChatMessageByChatUser(UpdateChatMessageOfChatUserCriteria criteria)
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

        public ICommandResult InsertMemberBanned(InsertMemberBannedCriteria criteria)
        {
            var result = _chatRepository.ModifyData(criteria);

            return result;
        }

        public ICommandResult UpdateMembersBanned(UpdateBanMembersCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            return updateResult;
        }

        /// <summary>
        /// Update list users banned on admin site.
        /// </summary>
        /// <param name="criteria">Criteria for update banned.</param>
        /// <returns>View model contains information for member site use.</returns>
        public UpdateBanListUserViewModel UpdateBanChatUsersOnAdminSite(UpdateBanChatUsersCriteria criteria)
        {
            var result = _chatRepository.ModifyData(criteria);

            var updateBanListUserViewModel = new UpdateBanListUserViewModel
            {
                CommandResult = (CommandResult)result,
                ChatUserIds = criteria.ChatUserIds
            };

            if (result.IsSuccessful && criteria.IsBanned)
            {
                SetUpdateBanListUserToken(updateBanListUserViewModel);
            }

            return updateBanListUserViewModel;
        }

        /// <summary>
        /// Update Chat Room Status by match id.
        /// </summary>
        /// <param name="criteria">Update match status information.</param>
        /// <returns>Result of update command.</returns>
        public UpdateChatRoomStatusViewModel UpdateChatRoomStatusByMatchIdOnAdminSite(UpdateChatRoomStatusByMatchIdCriteria criteria)
        {
            var currentMatchInfo = GetChatRoomByName(new GetChatRoomByNameCriteria
            {
                ChatRoomName = criteria.ChatRoomName,
                MatchId = criteria.MatchId
            });
            var updateChatRoomStatusViewModel = new UpdateChatRoomStatusViewModel
                {
                    CommandResult = new CommandResult
                    {
                        Code = CodeFail,
                        Message = AdminSiteLang.InvalidInput
                    }
                };

            if (currentMatchInfo != null)
            {
                var updateResult = _chatRepository.ModifyData(criteria);

                updateChatRoomStatusViewModel = new UpdateChatRoomStatusViewModel
                {
                    CommandResult = (CommandResult)updateResult,
                    ChatRoomName = criteria.ChatRoomName,
                    Disabled = criteria.Disable
                };

                if (updateResult.IsSuccessful)
                {
                    SetUpdateChatRoomStatusToken(updateChatRoomStatusViewModel);
                }
            }

            return updateChatRoomStatusViewModel;
        }

        #endregion Modify Data

        #region Ban Words Management

        public ICommandResult UpdateBanWordText(UpdateBanWordTextCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            return updateResult;
        }

        public ICommandResult UpdateBanWordStatus(UpdateBanWordStatusCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            return updateResult;
        }

        public ICommandResult InsertBanWords(InsertBanWordsCriteria criteria)
        {
            var updateResult = _chatRepository.ModifyData(criteria);

            return updateResult;
        }

        public IEnumerable<BanWord> GetBanWords(GetBanWordsCriteria criteria)
        {
            var banWords = _chatRepository.Fetch<BanWord>(criteria);

            return banWords;
        }

        #endregion

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
                    usernameCell.SetCellValue(chatMessage.ChatUser.MemberName);
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

        private void SetDeleleMessageToken(UpdateChatMessageViewModel deleteMessageViewModel)
        {
            deleteMessageViewModel.TokenName = string.Format("{0}_{1}", deleteMessageViewModel.ChatMessageId, deleteMessageViewModel.ChatRoomName);
            deleteMessageViewModel.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                UpdateMessageCommandCacheKey + "_" + deleteMessageViewModel.TokenName,
                deleteMessageViewModel.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        private void SetDeleleMessageByChatUserToken(UpdateChatMessageByChatUserViewModel deleteMessageViewModel)
        {
            deleteMessageViewModel.TokenName = string.Format("{0}_{1}", deleteMessageViewModel.ChatUserId, deleteMessageViewModel.ChatRoomName);
            deleteMessageViewModel.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                UpdateMessageByUserCommandCacheKey + "_" + deleteMessageViewModel.TokenName,
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

        private void SetUpdateMuteChatUserToken(UpdateMuteChatUserViewModel updateMuteChatUser)
        {
            updateMuteChatUser.TokenName = string.Format("{0}_{1}", updateMuteChatUser.ChatUserId, updateMuteChatUser.ChatRoomId);
            updateMuteChatUser.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                UpdateMuteChatUserCommandCacheKey + "_" + updateMuteChatUser.TokenName,
                updateMuteChatUser.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        private void SetUpdateMuteChatRoomToken(UpdateMuteChatRoomViewModel updateMuteChatRoom)
        {
            updateMuteChatRoom.TokenName = string.Format("{0}", updateMuteChatRoom.ChatRoomId);
            updateMuteChatRoom.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                UpdateMuteChatRoomCommandCacheKey + "_" + updateMuteChatRoom.TokenName,
                updateMuteChatRoom.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        private void SetUpdateChatRoomStatusToken(UpdateChatRoomStatusViewModel updateChatRoomStatus)
        {
            updateChatRoomStatus.TokenName = string.Format("{0}", updateChatRoomStatus.ChatRoomId);
            updateChatRoomStatus.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                UpdateChatRoomStatusCommandCacheKey + "_" + updateChatRoomStatus.TokenName,
                updateChatRoomStatus.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        /// <summary>
        /// Set Token for performance action in member site.
        /// </summary>
        /// <param name="updateBanListUserViewModel">Model contains information for ban list users.</param>
        private void SetUpdateBanListUserToken(UpdateBanListUserViewModel updateBanListUserViewModel)
        {
            var chatUserIds = updateBanListUserViewModel.ChatUserIds.Length > 20
                                ? updateBanListUserViewModel.ChatUserIds.Substring(0, 20)
                                : updateBanListUserViewModel.ChatUserIds;

            updateBanListUserViewModel.TokenName = string.Format("{0}", chatUserIds);
            updateBanListUserViewModel.TokenKey = Guid.NewGuid().ToString();

            CacheManager.Instance.Set<string>(
                UpdateBanListUserCacheKey + "_" + updateBanListUserViewModel.TokenName,
                updateBanListUserViewModel.TokenKey,
                new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));
        }

        /// <summary>
        /// Check current chat name violate rule or not.
        /// </summary>
        /// <param name="chatName">Current chat name.</param>
        /// <param name="chatNameDenyList">List name deny.</param>
        /// <returns>Violate or not.</returns>
        private bool CheckChatNameViolateRule(string chatName, string chatNameDenyList)
        {
            if (!string.IsNullOrEmpty(chatNameDenyList))
            {
                string[] chatNameDenies = chatNameDenyList.Split(new char[] { ',' });
                var nameLower = chatName.ToLower();
                foreach (string chatNameDeny in chatNameDenies)
                {
                    if (nameLower.Contains(chatNameDeny.ToLower().Trim()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Private Functions
    }
}