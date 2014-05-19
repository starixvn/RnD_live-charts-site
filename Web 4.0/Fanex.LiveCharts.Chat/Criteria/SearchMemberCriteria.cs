// --------------------------------------------------------------------------
// <copyright file="SearchMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.17">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchMemberCriteria : CriteriaBase
    {
        public string MemberName { get; set; }

        public int InDatabase { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Search_Member";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(MemberName) && InDatabase >= 0;
        }
    }
}