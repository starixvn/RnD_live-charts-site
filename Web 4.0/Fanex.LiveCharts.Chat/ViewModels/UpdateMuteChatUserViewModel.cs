// --------------------------------------------------------------------------
// <copyright file="UpdateMuteChatUserViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.15">Created</change>
//   <change who="Ricky.Nguyen" date="2014.6.26">Update Mute Period</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateMuteChatUserViewModel
    {
        public CommandResult CommandResult { get; set; }

        public bool Muted { get; set; }

        public int ChatRoomId { get; set; }

        public string ChatRoomName { get; set; }

        public int ChatUserId { get; set; }

        public int ContinueMuteInMinutes { get; set; }

        public string TokenName { get; set; }

        public string TokenKey { get; set; }
    }
}