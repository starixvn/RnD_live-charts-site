// --------------------------------------------------------------------------
// <copyright file="ChatSettingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.8">Created</change>
// </history>

namespace Fanex.LiveCharts.ViewModel.Chat
{
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatSettingViewModel
    {
        public SettingItem TermCondition { get; set; }

        public SettingItem WelcomeMessage { get; set; }

        public SettingItem ChatNameDenyList { get; set; }
    }
}