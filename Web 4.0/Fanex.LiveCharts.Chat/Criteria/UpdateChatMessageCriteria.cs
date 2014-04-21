// --------------------------------------------------------------------------
// <copyright file="UpdateChatMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Chat Message Criteria.
    /// </summary>
    public class UpdateChatMessageCriteria : CriteriaBase
    {
        public int ChatMessageId { get; set; }

        public bool IsDelete { get; set; }

        public int DeletedBy { get; set; }

        [SpParam(Ignored = true)]
        public string ChatRoomName { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_Message";
        }

        public override bool IsValid()
        {
            return ChatMessageId > 0 && DeletedBy > 0;
        }
    }
}