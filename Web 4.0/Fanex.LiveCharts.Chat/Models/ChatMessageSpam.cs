// --------------------------------------------------------------------------
// <copyright file="ChatMessageSpam.cs" company="Nexcel Solutions Vietnam">
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
    /// Chat Message Spam.
    /// </summary>
    public class ChatMessageSpam
    {
        public int ChatMessageSpamId { get; set; }

        public int ChatMessageId { get; set; }

        public DateTime ReportTime { get; set; }

        public int ReportByChatUserId { get; set; }

        public int ChatUserIdSpam { get; set; }

        public bool IsHandled { get; set; }
    }
}