// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.18">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService() : this(null)
        {
        }

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        public SettingItem GetClosePredictionMinuteSetting(ChartAdminGetMinuteForClosingPredictionCriteria getClosingMinuteCriteria)
        {
            var closePredictionMinuteSetting = _settingRepository.Get<SettingItem>(getClosingMinuteCriteria);

            return closePredictionMinuteSetting;
        }

        public ICommandResult UpdateClosePredictionMinuteSetting(ChartAdminUpdateMinuteForClosingPredictionCriteria updateClosingMinuteCriteria)
        {
            var result = _settingRepository.ModifyData(updateClosingMinuteCriteria);

            return result;
        }
    }
}