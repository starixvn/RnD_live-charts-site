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
    [Serializable]
    public class ChatUser
    {
        public int ChatUserId { get; set; }

        public string ChatName { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public bool IsBanned { get; set; }

        public bool AgreedTerms { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifiedTime { get; set; }

        public string ConnectionId { get; set; }
    }
}