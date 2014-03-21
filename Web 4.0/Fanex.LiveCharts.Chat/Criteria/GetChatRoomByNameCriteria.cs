// --------------------------------------------------------------------------
// <copyright file="GetChatRoomByNameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.21">Created</change>
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
    public class GetChatRoomByNameCriteria : CriteriaBase
    {
        public string ChatRoomName { get; set; }

        public int MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_Room_By_Name";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ChatRoomName);
        }
    }
}
