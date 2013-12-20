// --------------------------------------------------------------------------
// <copyright file="UserCheckInCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// User check in criteria.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class UserCheckInCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string NetSessionId { get; set; }

        public int OnUserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_User_CheckIn";
        }

        public override bool IsValid()
        {
            if (UserId == 0)
            {
                throw new Exception("Member Id cannot be empty!");
            }

            return true;
        }
    }
}