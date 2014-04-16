// --------------------------------------------------------------------------
// <copyright file="AdminGetChatRoomUsersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.16">Created</change>
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
    public class AdminGetChatRoomUsersCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public bool IncludingBanned { get; set; }

        public bool IncludingKickedOut { get; set; }

        public bool IncludingLeft { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Admin_Get_Chat_Room_Users";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0;
        }
    }
}
