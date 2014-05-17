// --------------------------------------------------------------------------
// <copyright file="UpdateBanListUserViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.17">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateBanListUserViewModel
    {
        public string ChatUserIds { get; set; }

        public string TokenName { get; set; }

        public string TokenKey { get; set; }

        public CommandResult CommandResult { get; set; }
    }
}
