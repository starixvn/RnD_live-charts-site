// --------------------------------------------------------------------------
// <copyright file="UpdateBanChatUsersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.17">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateBanChatUsersCriteria : CriteriaBase
    {
        public string ChatUserIds { get; set; }

        public bool IsBanned { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Ban_Chat_Users";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ChatUserIds);
        }
    }
}
