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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Microsoft.AspNet.SignalR;

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

        public void SendMessage(ChatMessage chatMessage)
        {
            var commandResult = _chatService.InsertChatMessage(new InsertChatMessageCriteria
            {
                ChatRoomId = chatMessage.ChatRoom.ChatRoomId,
                ChatUserId = chatMessage.ChatUser.ChatUserId,
                Content = chatMessage.Content
            });

            if (commandResult.CommandResult.IsSuccessful)
            {
                chatMessage.ChatMessageId = commandResult.InsertedId;
            }

            Clients.Group(chatMessage.ChatRoom.ChatRoomName).addMessage(chatMessage);
        }

        public IEnumerable<ChatUser> GetUserList(ChatRoom chatRoom)
        {
            var chatUsers = _chatService.GetChatRoomUsers(new GetChatRoomUsersCriteria
            {
                ChatRoomId = chatRoom.ChatRoomId
            });

            return chatUsers;
        }

        public Task JoinRoom(ChatUser chatUser, ChatRoom chatRoom, ChatUserInRoom chatUserInRoom)
        {
            _chatService.InsertChatUserInRoom(new InsertChatUserInRoomCriteria
                {
                    ChatRoomId = chatRoom.ChatRoomId,
                    ChatUserId = chatUser.ChatUserId,
                    ChatUserInRoomId = chatUserInRoom.ChatUserInRoomId,
                    ConnectionId = Context.ConnectionId
                });
            return Groups.Add(Context.ConnectionId, chatRoom.ChatRoomName).ContinueWith(jr =>
                {
                    NotifyUserJoinedRoom(chatUser, chatRoom);
                });
        }

        public Task LeaveRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            NotifyUserLeftRoom(chatUser, chatRoom);
            return Groups.Add(Context.ConnectionId, chatRoom.ChatRoomName);
        }

        public void NotifyUserLeftRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            Clients.Group(chatRoom.ChatRoomName).notifyUserLeftRoom(chatUser, GetCurrentUserList(chatRoom.ChatRoomId));
        }

        public void NotifyUserJoinedRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            Clients.Group(chatRoom.ChatRoomName).notifyUserJoinedRoom(chatUser, GetCurrentUserList(chatRoom.ChatRoomId));
        }

        public void AnnounceGeneralMessage(ChatMessage chatMessage, ChatRoom chatRoom)
        {
            Clients.Group(chatRoom.ChatRoomName).addAnnounceGeneralMessage(chatMessage);
        }

        public override Task OnDisconnected()
        {
            var currentChatUserInRoom = _chatService.GetChatUserInRoomByConnectionId(
                new GetChatUserInRoomByConnectionIdCriteria
                {
                    ConnectionId = Context.ConnectionId
                });

            if (currentChatUserInRoom != null)
            {
                var updateResult = _chatService.UpdateChatUserInRoom(new UpdateChatUserInRoomCriteria
                {
                    ChatUserInRoomId = currentChatUserInRoom.ChatUserInRoomId,
                    ConnectionId = Context.ConnectionId,
                    IsLeft = true
                });

                if (updateResult.IsSuccessful)
                {
                    Clients.Group(currentChatUserInRoom.ChatRoomName).notifyUserLeftRoom(currentChatUserInRoom, GetCurrentUserList(currentChatUserInRoom.ChatRoomId));
                }
                //// Clients.All.notifyUserLeftRoom();
            }

            return base.OnDisconnected();
        }

        private IEnumerable<ChatUser> GetCurrentUserList(int chatRoomId, bool isLeft = false)
        {
            var currentUserList = _chatService.GetChatRoomUsers(new GetChatRoomUsersCriteria
            {
                ChatRoomId = chatRoomId,
                IsLeft = isLeft
            });

            return currentUserList;
        }
    }
}