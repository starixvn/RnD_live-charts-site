// --------------------------------------------------------------------------
// <copyright file="KickOutChatUserViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Kick Out Chat User View Model.
    /// </summary>
    public class KickOutChatUserViewModel
    {
        public bool NeedToSendRequestKickOut { get; set; }

        public int ChatUserId { get; set; }

        public string ConnectionId { get; set; }

        public string TokenName { get; set; }

        public string TokenKey { get; set; }

        public CommandResult CommandResult { get; set; }
    }
}