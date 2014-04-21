// --------------------------------------------------------------------------
// <copyright file="GetWelcomeMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.8">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias.Chat
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Welcome Message Criteria.
    /// </summary>
    public class GetWelcomeMessageCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Welcome_Message";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}