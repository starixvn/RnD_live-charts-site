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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Services;
    using Microsoft.AspNet.SignalR;
    using System.Web;

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

        /// <summary>
        /// Send a message to a group.
        /// </summary>
        /// <param name="chatMessage">Current message need be sent.</param>
        public void SendMessage(ChatMessage chatMessage)
        {
            chatMessage.Content = HttpUtility.HtmlEncode(chatMessage.Content);

            var commandResult = _chatService.InsertChatMessage(new InsertChatMessageCriteria
            {
                ChatRoomId = chatMessage.ChatRoom.ChatRoomId,
                ChatUserId = chatMessage.ChatUser.ChatUserId,
                Content = chatMessage.Content
            });
            chatMessage.CreateTime = DateTime.Now;
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

        /// <summary>
        /// Join a chat user to a room.
        /// </summary>
        /// <param name="chatUser">Current user need join room.</param>
        /// <param name="chatRoom">Current room.</param>
        /// <param name="chatUserInRoom">If current user already have information in this room, then just change connection id.</param>
        /// <returns>Notify other user in room.</returns>
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

        /// <summary>
        /// Update chat user status when chat user disconnected.
        /// </summary>
        /// <returns>Announce other user in room.</returns>
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
        
        /// <summary>
        /// Get chat message history when user chat just join in room.
        /// </summary>
        /// <param name="chatRoomId">Current chat room.</param>
        /// <param name="pastMinutes">How many minutes for getting data.</param>
        /// <returns>History messages.</returns>
        public IEnumerable<ChatMessage> GetChatMessageHistory(int chatRoomId, int pastMinutes)
        {
            GetChatMessageByTimeCriteria getChatMessageByTimeCriteria = new GetChatMessageByTimeCriteria
            {
                ChatRoomId = chatRoomId,
                ToTime = DateTime.Now,
                FromTime = DateTime.Now.Subtract(new TimeSpan(0, pastMinutes, 0))
            };

            var chatMessageList = _chatService.GetChatMessagesByTime(getChatMessageByTimeCriteria);

            return chatMessageList;
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