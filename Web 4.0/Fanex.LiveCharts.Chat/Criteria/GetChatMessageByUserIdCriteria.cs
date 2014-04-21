// --------------------------------------------------------------------------
// <copyright file="GetChatMessageByUserIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Chat Message By User Id Criteria.
    /// </summary>
    public class GetChatMessageByUserIdCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public int ChatUserId { get; set; }

        public bool IsDeleted { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_Message_By_User_Id";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}