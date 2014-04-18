// --------------------------------------------------------------------------
// <copyright file="UpdateChatUserAgreeTermsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.18">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChatUserAgreeTermsCriteria : CriteriaBase
    {
        public int ChatUserId { get; set; }

        public bool AgreedTerms { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Chat_User_Agree_Terms";
        }

        public override bool IsValid()
        {
            return ChatUserId > 0;
        }
    }
}