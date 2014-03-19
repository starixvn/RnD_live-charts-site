// --------------------------------------------------------------------------
// <copyright file="IChatHub.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Criteria;
    using System.Threading.Tasks;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IChatHub
    {

        void SendMessage(ChatMessage chatMessage);

        /// <summary>
        /// When a new client connects
        /// </summary>
        Task OnConnected();

        /// <summary>
        /// When a client disconnects
        /// </summary>
        Task OnDisconnected();
    }
}
