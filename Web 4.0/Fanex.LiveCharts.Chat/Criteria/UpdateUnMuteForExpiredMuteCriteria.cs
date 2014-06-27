// --------------------------------------------------------------------------
// <copyright file="UpdateUnMuteForExpiredMuteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.6.25">Created</change>
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
    public class UpdateUnMuteForExpiredMuteCriteria : CriteriaBase
    {
        public int UpdateType { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_UnMute_For_Expired_Mute";
        }

        public override bool IsValid()
        {
            return UpdateType >= 0;
        }
    }
}
