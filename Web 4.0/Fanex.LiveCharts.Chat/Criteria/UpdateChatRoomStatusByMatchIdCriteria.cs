// --------------------------------------------------------------------------
// <copyright file="UpdateChatRoomStatusByMatchIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.26">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChatRoomStatusByMatchIdCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public bool Disable { get; set; }

        [SpParam(Ignored = true)]
        public string ChatRoomName { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_Room_Status_By_MatchId";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}