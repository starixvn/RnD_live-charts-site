// --------------------------------------------------------------------------
// <copyright file="ChatMonitorViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.2">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// Chat Monitor View Model.
    /// </summary>
    public class ChatMonitorViewModel
    {
        public Match Match { get; set; }

        public ChatRoom ChatRoom { get; set; }
    }
}