// --------------------------------------------------------------------------
// <copyright file="UpdateChatUserInRoomCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.21">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChatUserInRoomCriteria : CriteriaBase
    {
        public int ChatUserInRoomId { get; set; }

        public string ConnectionId { get; set; }

        public bool IsLeft { get; set; }

        ////public DateTime LastOnlineTime { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_User_In_Room";
        }

        public override bool IsValid()
        {
            return ChatUserInRoomId > 0;
        }
    }
}