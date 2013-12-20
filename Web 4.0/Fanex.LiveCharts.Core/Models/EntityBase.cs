// --------------------------------------------------------------------------
// <copyright file="EntityBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using System;
    using Fanex.NAP.Framework.Domain;

    /// <summary>
    /// Entity base.
    /// </summary>
    [Serializable]
    public class EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}