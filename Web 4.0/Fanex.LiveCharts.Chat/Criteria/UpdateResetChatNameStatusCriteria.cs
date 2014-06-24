// --------------------------------------------------------------------------
// <copyright file="UpdateResetChatNameStatusCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class UpdateResetChatNameStatusCriteria : CriteriaBase
    {
        public string ChatUserIds { get; set; }

        public bool IsResetChatName { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Reset_Chat_Name_Status";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ChatUserIds);
        }
    }
}