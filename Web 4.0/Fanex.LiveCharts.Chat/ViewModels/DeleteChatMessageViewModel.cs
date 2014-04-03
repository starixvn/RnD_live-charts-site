// --------------------------------------------------------------------------
// <copyright file="DeleteChatMessageViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DeleteChatMessageViewModel
    {
        public ICommandResult CommandResult { get; set; }

        public int ChatMessageId { get; set; }

        public string ChatRoomName { get; set; }

        public string TokenName { get; set; }

        public string TokenKey { get; set; }
    }
}