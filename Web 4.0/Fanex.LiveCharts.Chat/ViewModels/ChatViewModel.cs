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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatViewModel
    {
        public ChatRoom ChatRoom { get; set; }

        public ChatUser ChatUser { get; set; }

        public ChatUserInRoom ChatUserInRoom { get; set; }

        public string AnnoucementMessage { get; set; }

        public string WelcomeMessage { get; set; }
    }
}