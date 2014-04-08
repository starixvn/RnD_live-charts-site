// --------------------------------------------------------------------------
// <copyright file="UpdateChatUserCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class UpdateChatUserCriteria : CriteriaBase
    {
        public int ChatUserId { get; set; }

        public bool IsBanned { get; set; }

        ////public DateTime ModifiedTime { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_User";
        }

        public override bool IsValid()
        {
            return ChatUserId > 0;
        }
    }
}