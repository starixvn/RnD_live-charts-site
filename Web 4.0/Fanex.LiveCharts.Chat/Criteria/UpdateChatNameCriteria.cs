// --------------------------------------------------------------------------
// <copyright file="UpdateChatNameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.6.23">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChatNameCriteria : CriteriaBase
    {
        public int ChatUserId { get; set; }

        public string ChatName { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_Name";
        }

        public override bool IsValid()
        {
            return ChatUserId > 0;
        }
    }
}