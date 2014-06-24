// --------------------------------------------------------------------------
// <copyright file="ChatUserStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.6.24">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum ChatUserStatus : int
    {
        /// <summary>
        /// Online status.
        /// </summary>
        Online = 1,
        
        /// <summary>
        /// Off line status.
        /// </summary>
        OffLine = 2,
        
        /// <summary>
        /// Muted status.
        /// </summary>
        Muted = 3,
        
        /// <summary>
        /// Kicked Out Status.
        /// </summary>
        KickedOut = 4,

        /// <summary>
        /// Banned status.
        /// </summary>
        Banned = 5
    }
}