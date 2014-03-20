// --------------------------------------------------------------------------
// <copyright file="IChatHub.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IChatHub
    {
        void SendMessage(ChatMessage chatMessage, string chatRoom);

        Task JoinRoom(ChatUser chatUser, ChatRoom chatRoom);

        Task LeaveRoom(ChatUser chatUser, ChatRoom chatRoom);

        /// <summary>
        /// When a new client connects
        /// </summary>
        Task OnConnected();

        /// <summary>
        /// When a client disconnects
        /// </summary>
        Task OnDisconnected();
    }
}