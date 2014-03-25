// --------------------------------------------------------------------------
// <copyright file="IScorePredictionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Interface
{
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Models.ScorePrediction;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IScorePredictionService
    {
        ScorePredictionTotal GetTotalScorePrediction(GetAllScorePredictionCriteria getAllScorePredictionCriteria);

        ICommandResult InsertOrUpdateScorePredictionItem(InsertOrUpdateScorePredictionItemCriteria insertOrUpdateScorePredictionItemCriteria);

        ScorePredictionItem GetUserPrediction(GetUserPredictionCriteria getUserPredictionCriteria);

        ICommandResult InsertOrUpdateDummyPrediction(ChartAdminInsertUpdateDummyPredictionCriteria insertCriteria);

        bool IsStopScorePrediction(int matchId);
    }
}