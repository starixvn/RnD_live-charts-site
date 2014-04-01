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
    using System.Web;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Repositories;
    using Fanex.LiveCharts.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatService : IChatService
    {
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
            insertChatMessageCriteria.CreateTime = DateTime.Now;

            var insertResult = _chatRepository.ModifyData(insertChatMessageCriteria, "ChatMessageId");

            return insertResult;
        }

        public InsertResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria)
        {
            insertChatRoomCriteria.CreateTime = DateTime.Now;
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
            insertchatUserCriteria.CreateTime = DateTime.Now;
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
            updateChatUserCriteria.ModifiedTime = DateTime.Now;
            var updateResult = _chatRepository.ModifyData(updateChatUserCriteria);

            return updateResult;
        }

        public ChatRoom GetChatRoomByName(GetChatRoomByNameCriteria getChatRoomByName)
        {
            var chatRoom = _chatRepository.Get<ChatRoom>(getChatRoomByName);

            return chatRoom;
        }

        public IEnumerable<ChatUser> GetChatRoomUsers(GetChatRoomUsersCriteria getChatRoomUsersCriteria)
        {
            var chatUsers = _chatRepository.Fetch<ChatUser>(getChatRoomUsersCriteria);

            return chatUsers;
        }

        public InsertResult InsertChatUserInRoom(InsertChatUserInRoomCriteria insertChatUserInRoomCriteria)
        {
            insertChatUserInRoomCriteria.LastOnlineTime = DateTime.Now;
            var insertResult = _chatRepository.ModifyData(insertChatUserInRoomCriteria, "ChatUserInRoomId");

            return insertResult;
        }

        public ICommandResult UpdateChatUserInRoom(UpdateChatUserInRoomCriteria updateChatUserInRoomCriteria)
        {
            updateChatUserInRoomCriteria.LastOnlineTime = DateTime.Now;
            var updateResult = _chatRepository.ModifyData(updateChatUserInRoomCriteria);

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
                    ChatRoomId = getChatUserInRoomCriteria.ChatRoomId,
                    ChatUserId = getChatUserInRoomCriteria.ChatUserId
                };
            }

            return chatUserInRoom;
        }

        public ChatUserInRoom GetChatUserInRoomByConnectionId(GetChatUserInRoomByConnectionIdCriteria getChatUserInRoomByConnectionIdCriteria)
        {
            var chatUserInRoom = _chatRepository.Get<ChatUserInRoom>(getChatUserInRoomByConnectionIdCriteria);

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
    }
}