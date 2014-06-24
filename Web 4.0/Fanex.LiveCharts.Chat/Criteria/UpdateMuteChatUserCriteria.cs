// --------------------------------------------------------------------------
// <copyright file="UpdateMuteChatUserCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Update Mute Chat User Criteria.
    /// </summary>
    public class UpdateMuteChatUserCriteria : CriteriaBase
    {
        public int ChatUserId { get; set; }

        public int ChatRoomId { get; set; }

        [SpParam(Ignored = true)]
        public string ChatRoomName { get; set; }

        public bool IsMute { get; set; }

        public int MutePeriod { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Mute_Chat_User";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0 && ChatUserId > 0;
        }
    }
}