// --------------------------------------------------------------------------
// <copyright file="ChatUser.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Models
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatUser
    {
        public enum UserStatus : int
        {
            Online = 1,
            OffLine = 0,
        }

        public int ChatUserId { get; set; }

        public string ChatName { get; set; }

        public int MemberId { get; set; }

        public bool IsBanned { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastOnlineTime { get; set; }

        public string SignalRClient { get; set; }
    }
}