// --------------------------------------------------------------------------
// <copyright file="InsertChatRoomCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class InsertChatRoomCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public string ChatRoomName { get; set; }

        public string WelcomeMessage { get; set; }

        public int ChatRoomId { get; set; }

        public DateTime CreateTime { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Insert_Chat_Room";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
