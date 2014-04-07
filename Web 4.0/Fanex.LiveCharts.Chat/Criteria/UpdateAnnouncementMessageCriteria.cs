// --------------------------------------------------------------------------
// <copyright file="UpdateAnnouncementMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.7">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateAnnouncementMessageCriteria : CriteriaBase
    {
        public string AnnouncementMessage { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Announcement_Message";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(AnnouncementMessage);
        }
    }
}