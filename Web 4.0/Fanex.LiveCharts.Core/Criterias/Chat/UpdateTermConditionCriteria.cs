// --------------------------------------------------------------------------
// <copyright file="UpdateTermConditionCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class UpdateTermConditionCriteria : CriteriaBase
    {
        public string TermCondition { get; set; }

        public bool Disabled { get; set; }
        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Term_Condition";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(TermCondition);
        }
    }
}