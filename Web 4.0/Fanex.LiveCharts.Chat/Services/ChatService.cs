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
                chatUser = new ChatUser();
            }
            return chatUser;
        }

        public ICommandResult InsertChatMessage(InsertChatMessageCriteria insertChatMessageCriteria)
        {
            var commandResult = _chatRepository.ModifyData(insertChatMessageCriteria);

            return commandResult;
        }

        public ICommandResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria)
        {
            var commandResult = _chatRepository.ModifyData(insertChatRoomCriteria);

            return commandResult;
        }

        public ICommandResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria)
        {
            var commandResult = _chatRepository.ModifyData(insertchatUserCriteria);

            return commandResult;
        }

        public ICommandResult UpdateChatMessage(UpdateChatMessageCriteria updateChatMessageCriteria)
        {
            var commandResult = _chatRepository.ModifyData(updateChatMessageCriteria);

            return commandResult;
        }

        public ICommandResult UpdateChatRoom(UpdateChatRoomCriteria updateChatRoomCriteria)
        {
            var commandResult = _chatRepository.ModifyData(updateChatRoomCriteria);

            return commandResult;
        }

        public ICommandResult UpdateChatUser(UpdateChatUserCriteria updateChatUserCriteria)
        {
            var commandResult = _chatRepository.ModifyData(updateChatUserCriteria);

            return commandResult;
        }
    }
}