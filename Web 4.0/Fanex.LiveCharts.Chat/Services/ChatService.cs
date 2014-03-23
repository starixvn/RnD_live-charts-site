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
    using System.Collections.Generic;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Repositories;
    using Fanex.NAP.Framework.Data;
    using Fanex.LiveCharts.Models;

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
            var chatMessage = _chatRepository.Fetch<ChatMessage>(getChatMessageCriteria);

            return chatMessage;
        }

        public IEnumerable<ChatMessage> GetChatMessagesByUser(GetChatMessageByUserIdCriteria getChatMessageCriteria)
        {
            var chatMessage = _chatRepository.Fetch<ChatMessage>(getChatMessageCriteria);

            return chatMessage;
        }

        public ChatUser GetChatUser(GetChatUserCriteria getChatUserCriteria)
        {
            var chatUser = _chatRepository.Get<ChatUser>(getChatUserCriteria);
            if (chatUser == null)
            {
                chatUser = new ChatUser { 
                    MemberId = getChatUserCriteria.MemberId
                };
            }
            return chatUser;
        }

        public InsertResult InsertChatMessage(InsertChatMessageCriteria insertChatMessageCriteria)
        {
            var insertResult = _chatRepository.ModifyData(insertChatMessageCriteria, "ChatMessageId");

            return insertResult;
        }

        public InsertResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria)
        {
            var insertResult = _chatRepository.ModifyData(insertChatRoomCriteria, "ChatRoomId");

            return insertResult;
        }

        public InsertResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria)
        {
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
            var updateResult = _chatRepository.ModifyData(updateChatRoomCriteria);

            return updateResult;
        }

        public ICommandResult UpdateChatUser(UpdateChatUserCriteria updateChatUserCriteria)
        {
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
            var insertResult = _chatRepository.ModifyData(insertChatUserInRoomCriteria, "ChatUserInRoomId");

            return insertResult;
        }

        public ICommandResult UpdateChatUserInRoom(UpdateChatUserInRoomCriteria updateChatUserInRoomCriteria)
        {
            var updateResult = _chatRepository.ModifyData(updateChatUserInRoomCriteria);

            return updateResult;
        }

        public ChatUserInRoom GetChatUserInRoom(GetChatUserInRoomCriteria getChatUserInRoomCriteria)
        {
            var chatUserInRoom = _chatRepository.Get<ChatUserInRoom>(getChatUserInRoomCriteria);

            if(chatUserInRoom == null)
            {
                chatUserInRoom = new ChatUserInRoom { 
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
    }
}