/// <reference path="../../Views/Charts/NewChartContent.cshtml" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="ScorePrediction.js" />
'use strict';

var scorePrediction = new ScorePrediction(),
    scorePredictionView = {
    url: {
        insertOrUpdateScorePrediction: LIVECHARTS.resolveUrl('ScorePrediction/InsertOrUpdateScorePrediction'),
        getCurrentUserPrediction: LIVECHARTS.resolveUrl('ScorePrediction/GetCharts'),
        getScorePredictionResult: LIVECHARTS.resolveUrl('ScorePrediction/GetScorePrictionResult')
    },
    init: function () {
        scorePredictionView.txtHomeScore = $('#homePredictionValue');
        scorePredictionView.txtAwayScore = $('#awayPredictionValue');
        scorePredictionView.btnSubmitPrediction = $('#submitPreditionValue');
        scorePredictionView.smallContainerResult = $('#smallResultContainer');
        scorePredictionView.registerEvents();
    },
    registerEvents: function () {
        scorePredictionView.btnSubmitPrediction.unbind('click').click(function () {
            var homeScore = scorePredictionView.txtHomeScore.val();
            var awayScore = scorePredictionView.txtAwayScore.val();
            scorePredictionView.insertOrUpdateScorePrediction({
                memberId: 1,
                matchId: CRITERIA.matchId,
                homeScore: homeScore,
                awayScore: awayScore
            });
        });
    },
    getCorrectScoreResult: function (params) { 
        LIVECHARTS.callAjax({
            url: scorePredictionView.url.getScorePredictionResult,
            type: 'POST',
            data: params,
            done: function (result) {
                var resultArray = scorePrediction.getCorrectScoreResult(result);
                scorePredictionView.smallContainerResult.highChart(scorePrediction.getScoreOption(resultArray));
            }
        });
    },
    insertOrUpdateScorePrediction: function (params) {
        LIVECHARTS.callAjax({
            url: scorePredictionView.url.insertOrUpdateScorePrediction,
            type: 'POST',
            data: params,
            done: function (result) {
                if (result.IsSuccessful) {
                    scorePredictionView.getCorrectScoreResult(params);
                }
            }
        });
    }
};


$(function () {
    scorePredictionView.init();
});