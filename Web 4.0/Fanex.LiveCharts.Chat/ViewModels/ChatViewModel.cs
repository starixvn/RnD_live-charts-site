// --------------------------------------------------------------------------
// <copyright file="ChatViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.20">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// Chat View Model.
    /// </summary>
    public class ChatViewModel
    {
        public ChatRoom ChatRoom { get; set; }

        public ChatUser ChatUser { get; set; }

        public ChatUserInRoom ChatUserInRoom { get; set; }

        public SettingItem AnnoucementMessage { get; set; }

        public SettingItem WelcomeMessage { get; set; }
    }
}