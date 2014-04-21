// --------------------------------------------------------------------------
// <copyright file="UpdateChatMessageViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Chat Message View Model.
    /// </summary>
    public class UpdateChatMessageViewModel
    {
        public CommandResult CommandResult { get; set; }

        public bool IsDeleted { get; set; }

        public int ChatMessageId { get; set; }

        public string ChatRoomName { get; set; }

        public string TokenName { get; set; }

        public string TokenKey { get; set; }
    }
}