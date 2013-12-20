// --------------------------------------------------------------------------
// <copyright file="UserReCheckInCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// User recheck in criteria.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class UserReCheckInCriteria : CriteriaBase
    {
        public int MemberId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_User_ReCheckIn";
        }

        public override bool IsValid()
        {
            if (MemberId == 0)
            {
                throw new Exception("Member Id cannot be empty!");
            }

            return true;
        }
    }
}