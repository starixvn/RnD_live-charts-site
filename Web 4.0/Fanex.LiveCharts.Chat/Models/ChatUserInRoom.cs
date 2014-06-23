// --------------------------------------------------------------------------
// <copyright file="ChatUserInRoom.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.25">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Models
{
    using System;

    /// <summary>
    /// Current chat user information in a room.
    /// </summary>
    public class ChatUserInRoom
    {
        public int ChatUserInRoomId { get; set; }

        public DateTime LastOnlineTime { get; set; }

        public bool IsLeft { get; set; }

        public string ConnectionId { get; set; }

        public bool KickedOut { get; set; }

        public bool Muted { get; set; }

        public int MutePeriod { get; set; }

        public int CurrentMuteMinutes { get; set; }

        public ChatUser ChatUser { get; set; }

        public ChatRoom ChatRoom { get; set; }
    }
}