// --------------------------------------------------------------------------
// <copyright file="GetChatRoomByMatchIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.1">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Chat Room By Match Id Criteria.
    /// </summary>
    public class GetChatRoomByMatchIdCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_Room_By_Match_Id";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}