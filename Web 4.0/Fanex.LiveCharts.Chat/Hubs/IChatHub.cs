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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.LiveCharts.Chat.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IChatHub
    {
        void SendMessage(ChatMessage chatMessage);

        void NotifyUserLeftRoom(ChatUser chatUser, ChatRoom chatRoom);

        void NotifyUserJoinedRoom(ChatUser chatUser, ChatRoom chatRoom);

        void AnnounceGeneralMessage(ChatMessage chatMessage, ChatRoom chatRoom);

        IEnumerable<ChatUser> GetUserList(ChatRoom chatRoom);

        Task JoinRoom(ChatUser chatUser, ChatRoom chatRoom, ChatUserInRoom chatUserInRoom);

        Task LeaveRoom(ChatUser chatUser, ChatRoom chatRoom);

        Task OnConnected();

        Task OnDisconnected();
    }
}