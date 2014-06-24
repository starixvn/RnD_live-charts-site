// --------------------------------------------------------------------------
// <copyright file="UpdateMuteChatRoomCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.15">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update mute or not for chat room.
    /// </summary>
    public class UpdateMuteChatRoomCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public bool IsMute { get; set; }

        [SpParam(Ignored = true)]
        public string ChatRoomName { get; set; }

        public int MutePeriod { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Mute_Chat_Room";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0;
        }
    }
}