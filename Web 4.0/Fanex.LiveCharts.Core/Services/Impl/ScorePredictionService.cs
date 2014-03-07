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
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Models.ScorePrediction;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.NAP.Framework.Data;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionService : ServiceBase, IScorePredictionService
    {
        private const string ScorePredictionTotalCacheKey = "Fanex.LiveCharts.Services.Impl.ScorePredictionService";
        private const int ReUpdateCode = 1000;
        private IScorePredictionRepository _scorePredictionRepository;

        public ScorePredictionService()
            : this(null)
        {
        }

        public ScorePredictionService(IScorePredictionRepository scorePredictionRepository)
        {
            _scorePredictionRepository = scorePredictionRepository ?? new ScorePredictionRepository();
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

            scorePredictionTotal.TotalPrediction = scorePredictionTotal.ScorePredictionOptions.Sum(optionTotal => optionTotal.ScorePredictionCount);

            if (isUsingCache)
            {
                CacheManager.Instance.Set<ScorePredictionTotal>(cacheKey, scorePredictionTotal);
            }

            return scorePredictionTotal;
        }

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

        public ScorePredictionItem GetUserPrediction(
            GetUserPredictionCriteria getUserPredictionCriteria)
        {
            var currentUserPrediction = _scorePredictionRepository.Get<ScorePredictionItem>(getUserPredictionCriteria);

            if (currentUserPrediction == null)
            {
                return new ScorePredictionItem();
            }

            return currentUserPrediction;
        }
    }
}