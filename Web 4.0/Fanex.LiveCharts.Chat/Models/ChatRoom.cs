// --------------------------------------------------------------------------
// <copyright file="ChatRoom.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }

        public string ChatRoomName { get; set; }

        public int MatchId { get; set; }

        public DateTime CreateTime { get; set; }

        public string WelcomeMessage { get; set; }
    }
}