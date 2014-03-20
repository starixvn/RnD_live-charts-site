// --------------------------------------------------------------------------
// <copyright file="UpdateChatRoomCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChatRoomCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public string WelcomeMessage { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_Room";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}