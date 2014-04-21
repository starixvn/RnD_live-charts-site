// --------------------------------------------------------------------------
// <copyright file="SettingItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    /// <summary>
    /// Setting Item.
    /// </summary>
    public class SettingItem
    {
        public int SettingId { get; set; }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }

        public bool Disabled { get; set; }
    }
}