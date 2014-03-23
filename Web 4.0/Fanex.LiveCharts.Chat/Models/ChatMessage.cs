// --------------------------------------------------------------------------
// <copyright file="ChatMessage.cs" company="Nexcel Solutions Vietnam">
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
    public class ChatMessage
    {
        public ChatMessage()
        {
            ChatUser = new ChatUser();
        }

        public int ChatMessageId { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int DeletedBy { get; set; }

        public ChatUser ChatUser { get; set; }

        public ChatRoom ChatRoom { get; set; }
    }
}