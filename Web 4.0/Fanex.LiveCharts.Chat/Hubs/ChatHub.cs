// --------------------------------------------------------------------------
// <copyright file="ChatHub.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Hubs
{
    using System.Threading.Tasks;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Microsoft.AspNet.SignalR;
    using Fanex.LiveCharts.Chat.Criteria;
using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatHub : Hub, IChatHub
    {
        private readonly IChatService _chatService;

        public ChatHub()
            : this(null)
        {
        }

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService ?? new ChatService();
        }

        public void SendMessage(ChatMessage chatMessage, ChatRoom chatRoom)
        {
            var commandResult = _chatService.InsertChatMessage(new InsertChatMessageCriteria
            {
                ChatRoomId = chatRoom.ChatRoomId,
                ChatUserId = chatMessage.ChatUser.ChatUserId,
                Content = chatMessage.Content
            });

            if (commandResult.CommandResult.IsSuccessful)
            {
                chatMessage.ChatMessageId = commandResult.InsertedId;
            }

            Clients.Group(chatRoom.ChatRoomName).addMessage(chatMessage);
        }

        public IEnumerable<ChatUser> GetUserList(ChatRoom chatRoom)
        {
            var chatUsers = _chatService.GetChatRoomUsers(new GetChatRoomUsersCriteria
            {
                ChatRoomId = chatRoom.ChatRoomId
            });


            return chatUsers;
        }

        public Task JoinRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            NotifyUserJoinedRoom(chatUser, chatRoom);
            return Groups.Add(Context.ConnectionId, chatRoom.ChatRoomName);
        }

        public Task LeaveRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            NotifyUserLeftRoom(chatUser, chatRoom);
            return Groups.Add(Context.ConnectionId, chatRoom.ChatRoomName);
        }

        public void NotifyUserLeftRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            Clients.Group(chatRoom.ChatRoomName).notifyUserJoinedRoom(chatUser);
        }

        public void NotifyUserJoinedRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            Clients.Group(chatRoom.ChatRoomName).notifyUserLeftRoom(chatUser);
        }

        public void AnnounceGeneralMessage(ChatMessage chatMessage, ChatRoom chatRoom)
        {
            Clients.Group(chatRoom.ChatRoomName).addAnnounceGeneralMessage(chatMessage);
        }
    }
}