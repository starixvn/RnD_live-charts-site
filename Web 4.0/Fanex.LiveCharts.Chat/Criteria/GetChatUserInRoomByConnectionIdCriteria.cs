// --------------------------------------------------------------------------
// <copyright file="GetChatUserInRoomByConnectionIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.25">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get User in a room with user connection id.
    /// </summary>
    public class GetChatUserInRoomByConnectionIdCriteria : CriteriaBase
    {
        public string ConnectionId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_User_In_Room_By_ConnectionId";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ConnectionId);
        }
    }
}