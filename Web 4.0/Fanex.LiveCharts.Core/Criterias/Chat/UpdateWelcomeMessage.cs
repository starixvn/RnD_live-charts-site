// --------------------------------------------------------------------------
// <copyright file="UpdateWelcomeMessage.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.8">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias.Chat
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateWelcomeMessage : CriteriaBase
    {
        public string WelcomeMessage { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Welcome_Message";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(WelcomeMessage);
        }
    }
}