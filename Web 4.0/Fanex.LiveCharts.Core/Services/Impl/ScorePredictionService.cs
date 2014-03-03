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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionService : ServiceBase, IScorePredictionService
    {
        private IScorePredictionRepository _scorePredictionRepository;

        public ScorePredictionService()
            : this(null)
        {
        }

        public ScorePredictionService(IScorePredictionRepository scorePredictionRepository)
        {
            _scorePredictionRepository = scorePredictionRepository ?? new ScorePredictionRepository();
        }

        public ScorePredictionTotal GetTotalScorePrediction(
            GetAllScorePredictionCriteria getAllScorePredictionCriteria)
        {
            var scorePredictionTotal = new ScorePredictionTotal
            {
                ScorePredictionOptions = _scorePredictionRepository.Fetch<ScorePredictionOption>(getAllScorePredictionCriteria).ToList()
            };

            scorePredictionTotal.TotalPrediction = scorePredictionTotal.ScorePredictionOptions.Sum(optionTotal => optionTotal.ScorePredictionCount);

            return scorePredictionTotal;
        }

        public ICommandResult InsertOrUpdateScorePredictionItem(
            InsertOrUpdateScorePredictionItemCriteria insertOrUpdateScorePredictionItemCriteria)
        {
            return _scorePredictionRepository.ModifyData(insertOrUpdateScorePredictionItemCriteria);
        }

        public ScorePredictionItem GetUserPrediction(
            GetUserPredictionCriteria getUserPredictionCriteria)
        {
            return _scorePredictionRepository.Get<ScorePredictionItem>(getUserPredictionCriteria);
        }
    }
}