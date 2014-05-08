// --------------------------------------------------------------------------
// <copyright file="BanChatUserViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Ban chat user view model.
    /// </summary>
    public class BanChatUserViewModel
    {
        public int ChatUserId { get; set; }

        public string TokenName { get; set; }

        public string TokenKey { get; set; }

        public CommandResult CommandResult { get; set; }
    }
}