// --------------------------------------------------------------------------
// <copyright file="KickOutChatUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KickOutChatUserCriteria : CriteriaBase
    {
        public int ChatUserId { get; set; }

        public int ChatRoomId { get; set; }

        public bool IsKickOut { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Kick_Out_Chat_User";
        }

        public override bool IsValid()
        {
            return ChatUserId > 0 && ChatRoomId > 0;
        }
    }
}