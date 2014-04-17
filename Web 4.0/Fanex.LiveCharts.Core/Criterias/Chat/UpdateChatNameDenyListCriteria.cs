// --------------------------------------------------------------------------
// <copyright file="UpdateChatNameDenyListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.17">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChatNameDenyListCriteria : CriteriaBase
    {
        public string ChatNameDenyList { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_Name_Deny_List";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ChatNameDenyList);
        }
    }
}
