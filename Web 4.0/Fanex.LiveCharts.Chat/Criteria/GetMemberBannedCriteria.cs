// --------------------------------------------------------------------------
// <copyright file="GetMemberBannedCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get member banned information.
    /// </summary>
    public class GetMemberBannedCriteria : CriteriaBase
    {
        public int MemberId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Member_Banned";
        }

        public override bool IsValid()
        {
            return MemberId > 0;
        }
    }
}