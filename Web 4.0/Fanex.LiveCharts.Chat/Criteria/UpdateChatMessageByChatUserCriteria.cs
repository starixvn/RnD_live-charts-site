// --------------------------------------------------------------------------
// <copyright file="UpdateChatMessageByChatUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChatMessageByChatUserCriteria : CriteriaBase
    {
        public int ChatUserId { get; set; }

        public bool IsDelete { get; set; }

        public int DeletedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_Message_By_Chat_User";
        }

        public override bool IsValid()
        {
            return ChatUserId > 0;
        }
    }
}