// --------------------------------------------------------------------------
// <copyright file="InsertTokenCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.30">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InsertTokenCriteria : CriteriaBase
    {
        public int MemberId { get; set; }

        public string Token { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Insert_Token";
        }

        public override bool IsValid()
        {
            if (MemberId == 0)
            {
                throw new Exception("Member id cannot be empty.");
            }

            return true;
        }
    }
}
