// --------------------------------------------------------------------------
// <copyright file="GetTermConditionCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Term Condition Criteria.
    /// </summary>
    public class GetTermConditionCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Term_Condition";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}