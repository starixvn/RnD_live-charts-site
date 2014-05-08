// --------------------------------------------------------------------------
// <copyright file="InsertResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.21">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Insert Result.
    /// </summary>
    public class InsertResult
    {
        public int InsertedId { get; set; }

        public ICommandResult CommandResult { get; set; }
    }
}