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
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingService
    {
        SettingItem GetClosePredictionMinuteSetting(ChartAdminGetMinuteForClosingPredictionCriteria getClosingMinuteCriteria);

        ICommandResult UpdateClosePredictionMinuteSetting(ChartAdminUpdateMinuteForClosingPredictionCriteria updateClosingMinuteCriteria);
    }
}