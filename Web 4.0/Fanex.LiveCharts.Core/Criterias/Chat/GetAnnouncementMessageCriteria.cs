// --------------------------------------------------------------------------
// <copyright file="GetAnnouncementMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.7">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias.Chat
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetAnnouncementMessageCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Announcement_Message";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}