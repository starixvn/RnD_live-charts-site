// --------------------------------------------------------------------------
// <copyright file="UpdateBanWordStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.30">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Ban Word Status Criteria.
    /// </summary>
    public class UpdateBanWordStatusCriteria : CriteriaBase
    {
        public int BanWordId { get; set; }

        public bool Delete { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Ban_Word_Status";
        }

        public override bool IsValid()
        {
            return BanWordId > 0;
        }
    }
}