/// <reference path="../../Views/ScorePrediction/Index.cshtml" />
/// <reference path="../../Views/Shared/_Layout.cshtml" />

/// <reference path="ScorePrediction.js" />
/*global LANGUAGE, LIVECHARTS, scorePredictionViewModel, ScorePrediction*/
'use strict';

function ScorePredictionResultView() {
    var me = this;

    me.scorePrediction = new ScorePrediction(
        scorePredictionViewModel.homeName,
        scorePredictionViewModel.awayName,
        LANGUAGE.draw);

    me.url = {
        getScorePredictionResult: LIVECHARTS.resolveUrl('ScorePrediction/GetScorePrictionResult')
    };

    me.init = function () {
        me.oneXTwoResultContainer = $('#oneXTwoResultContainer');
        me.oddEvenResultContainer = $('#oddEvenResultContainer');
        me.correctScoreResultContainer = $('#correctScoreResultContainer');
        me.handicapResultContainer = $('#handicapResultContainer');

        me.loadResult();
    };

    me.loadResult = function () {
        LIVECHARTS.callAjax({
            url: me.url.getScorePredictionResult,
            type: 'POST',
            data: {
                matchId: scorePredictionViewModel.matchId
            },
            done: function (result) {
                me.render1X2Result(result);
                me.renderOddEvenResult(result);
                me.renderCorrectScoreResult(result);
                me.renderHandicapResult(result);
            }
        });
    };

    me.renderCorrectScoreResult = function (result) {
        var resultArray = me.scorePrediction.getCorrectScoreResult(result);
        var getChartOption = me.scorePrediction.getScoreOption(resultArray, 'Correct Score');
        me.correctScoreResultContainer.highcharts(getChartOption);
    };

    me.renderHandicapResult = function (result) {
        var resultArray = me.scorePrediction.getHandicapResult(result);
        var getChartOption = me.scorePrediction.getScoreOption(resultArray, 'Handicap');

        me.handicapResultContainer.highcharts(getChartOption);

    };

    me.render1X2Result = function (result) {
        var resultArray = me.scorePrediction.get1X2Result(result);
        var getChartOption = me.scorePrediction.getBarOption(resultArray, '1x2');
        me.oneXTwoResultContainer.highcharts(getChartOption);
    };

    me.renderOddEvenResult = function (result) {
        var resultArray = me.scorePrediction.getOddEvenResult(result);
        var getChartOption = me.scorePrediction.getBarOption(resultArray, 'Odd/Even');
        me.oddEvenResultContainer.highcharts(getChartOption);
    };
}

$(function () {
    var scorePredictionResultView = new ScorePredictionResultView();
    scorePredictionResultView.init();
});