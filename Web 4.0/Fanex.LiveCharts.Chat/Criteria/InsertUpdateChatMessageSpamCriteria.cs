// --------------------------------------------------------------------------
// <copyright file="InsertUpdateChatMessageSpamCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.25">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InsertUpdateChatMessageSpamCriteria : CriteriaBase
    {
        public int ChatMessageId { get; set; }

        public int ReportByChatUserId { get; set; }

        public int ChatUserIdSpam { get; set; }

        public bool IsHandled { get; set; }

        public int ChatMessageSpamId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Insert_Update_Chat_Message_Spam";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}