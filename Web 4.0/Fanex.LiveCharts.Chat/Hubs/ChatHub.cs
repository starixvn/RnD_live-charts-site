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
            Clients.Group(chatRoom.ChatRoomName).addMessage(chatMessage);
        }

        public Task JoinRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            return Groups.Add(Context.ConnectionId, chatRoom.ChatRoomName);
        }

        public Task LeaveRoom(ChatUser chatUser, ChatRoom chatRoom)
        {
            return Groups.Add(Context.ConnectionId, chatRoom.ChatRoomName);
        }

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
    }
}