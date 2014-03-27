// --------------------------------------------------------------------------
// <copyright file="InsertChatUserCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class InsertChatUserCriteria : CriteriaBase
    {
        public int MemberId { get; set; }

        public string ChatName { get; set; }

        public bool IsBanned { get; set; }

        public int ChatUserId { get; set; }

        public DateTime CreateTime { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Insert_Chat_User";
        }

        public override bool IsValid()
        {
            return MemberId > 0 && !string.IsNullOrEmpty(ChatName);
        }
    }
}