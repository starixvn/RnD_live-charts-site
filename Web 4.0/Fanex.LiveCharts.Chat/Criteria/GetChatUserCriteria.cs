// --------------------------------------------------------------------------
// <copyright file="GetChatUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>


namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetChatUserCriteria : CriteriaBase
    {
        public int MemberId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_User";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
