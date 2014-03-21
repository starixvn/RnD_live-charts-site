// --------------------------------------------------------------------------
// <copyright file="InsertChatMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InsertChatMessageCriteria : CriteriaBase
    {
        public int ChatUserId { get; set; }

        public string Content { get; set; }

        public int ChatRoomId { get; set; }

        public int ChatMessageId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Insert_Chat_Message";
        }

        public override bool IsValid()
        {
            return ChatUserId > 0 && ChatRoomId > 0;
        }
    }
}