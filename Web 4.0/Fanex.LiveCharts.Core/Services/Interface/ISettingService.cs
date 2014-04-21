// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.18">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Interface
{
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// I Setting Service.
    /// </summary>
    public interface ISettingService
    {
        SettingItem GetSetting(ICriteria criteria);

        ICommandResult UpdateSetting(ICriteria criteria);
    }
}