// --------------------------------------------------------------------------
// <copyright file="CheckUserTokenCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Check user token criteria.
    /// </summary>
    public class CheckUserTokenCriteria : CriteriaBase
    {
        public int MemberId { get; set; }

        public string Token { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Check_Token";
        }

        public override bool IsValid()
        {
            if (MemberId == 0 || string.IsNullOrEmpty(Token))
            {
                throw new Exception("Member Id or Token cannot be empty!");
            }

            return true;
        }
    }
}