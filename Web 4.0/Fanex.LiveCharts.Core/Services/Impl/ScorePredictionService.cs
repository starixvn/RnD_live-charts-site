// --------------------------------------------------------------------------
// <copyright file="ScorePredictionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using System.Linq;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Models.ScorePrediction;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionService : ServiceBase, IScorePredictionService
    {
        private const string ScorePredictionTotalCacheKey = "Fanex.LiveCharts.Services.Impl.ScorePredictionService";
        private const int ReUpdateCode = 1000;
        private IScorePredictionRepository _scorePredictionRepository;
        private IChartService _chartService;

        public ScorePredictionService()
            : this(null, null)
        {
        }

        public ScorePredictionService(IScorePredictionRepository scorePredictionRepository, IChartService chartService)
        {
            _scorePredictionRepository = scorePredictionRepository ?? new ScorePredictionRepository();
            _chartService = chartService ?? new ChartService();
        }

        /// <summary>
        /// Get total score prediction for showing report.
        /// </summary>
        /// <param name="getAllScorePredictionCriteria">Parameters for query data.</param>
        /// <returns>Score prediction total.</returns>
        public ScorePredictionTotal GetTotalScorePrediction(
            GetAllScorePredictionCriteria getAllScorePredictionCriteria)
        {
            var cacheKey = string.Format("{0}_{1}", ScorePredictionTotalCacheKey, getAllScorePredictionCriteria.MatchId);
            var isUsingCache = AppSettings.ScorePredictionCacheTimeOutSeconds > 0;
            if (isUsingCache)
            {
                var scorePredictionCache = CacheManager.Instance.Get<ScorePredictionTotal>(cacheKey);
                if (scorePredictionCache != null)
                {
                    return scorePredictionCache;
                }
            }

            var scorePredictionTotal = new ScorePredictionTotal
            {
                ScorePredictionOptions = _scorePredictionRepository.Fetch<ScorePredictionOption>(getAllScorePredictionCriteria).ToList()
            };

            foreach (var option in scorePredictionTotal.ScorePredictionOptions)
            {
                //// If member site, then reset dummy count.
                if (!getAllScorePredictionCriteria.IsAdminSite)
                {
                    option.DummyCount = 0;
                    option.AdminId = 0;
                    option.AdminName = string.Empty;
                    option.UpdateDummyTime = null;
                }

                scorePredictionTotal.TotalPrediction += option.ScorePredictionCount;
            }

            if (getAllScorePredictionCriteria.IsAdminSite)
            {
                scorePredictionTotal.ScorePredictionOptions = scorePredictionTotal.ScorePredictionOptions.OrderByDescending(spo => spo.DummyCount).ToList();
            }

            if (isUsingCache)
            {
                CacheManager.Instance.Set<ScorePredictionTotal>(cacheKey, scorePredictionTotal);
            }

            return scorePredictionTotal;
        }

        /// <summary>
        /// Insert or update Score prediction item.
        /// </summary>
        /// <param name="insertOrUpdateScorePredictionItemCriteria">Parameters for updating.</param>
        /// <returns>Result of command.</returns>
        public ICommandResult InsertOrUpdateScorePredictionItem(
            InsertOrUpdateScorePredictionItemCriteria insertOrUpdateScorePredictionItemCriteria)
        {
            var updateResult = _scorePredictionRepository.ModifyData(insertOrUpdateScorePredictionItemCriteria);
            if (updateResult.Code == ReUpdateCode)
            {
                return new CommandResult
                {
                    Code = 0,
                    Message = Lang.UpdateSuccessfullyMessage
                };
            }

            return updateResult;
        }

        /// <summary>
        /// Get user prediction, if not, then get Score of match to set default score.
        /// </summary>
        /// <param name="getUserPredictionCriteria">Get user prediction criteria.</param>
        /// <returns>Current user prediction or default score.</returns>
        public ScorePredictionItem GetUserPrediction(
            GetUserPredictionCriteria getUserPredictionCriteria)
        {
            var currentUserPrediction = _scorePredictionRepository.Get<ScorePredictionItem>(getUserPredictionCriteria);

            if (currentUserPrediction == null)
            {
                var currentMatchStatus = _chartService.GetCurrentMatchTime(new GetCurrentMatchTimeCriteria
                {
                    MatchId = getUserPredictionCriteria.MatchId
                });

                return new ScorePredictionItem
                {
                    AwayScore = currentMatchStatus.AwayScore,
                    HomeScore = currentMatchStatus.HomeScore
                };
            }

            return currentUserPrediction;
        }

        public ICommandResult InsertOrUpdateDummyPrediction(ChartAdminInsertUpdateDummyPredictionCriteria insertCriteria)
        {
            var result = _scorePredictionRepository.ModifyData(insertCriteria);

            return result;
        }
    }
}