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

    public class ChatUserInRoom
    {
        public int ChatUserInRoomId { get; set; }

        public int ChatUserId { get; set; }

        public int ChatRoomId { get; set; }

        public DateTime LastOnlineTime { get; set; }

        public bool IsLeft { get; set; }

        public string ConnectionId { get; set; }

        public bool KickedOut { get; set; }

        public string ChatRoomName { get; set; }

        public string ChatName { get; set; }
    }
}