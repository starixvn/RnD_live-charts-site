// --------------------------------------------------------------------------
// <copyright file="GetChatMessageByRoomCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Chat Message By Room Criteria.
    /// </summary>
    public class GetChatMessageByRoomCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public bool IsDeleted { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_Message_By_Room";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0;
        }
    }
}