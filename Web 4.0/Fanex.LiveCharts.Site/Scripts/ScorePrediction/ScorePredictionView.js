/// <reference path="../../Views/Charts/NewChartContent.cshtml" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="ScorePrediction.js" />
/*global CRITERIA, LIVECHARTS, ScorePrediction*/
'use strict';

function ScorePredictionView() {
    var me = this;

    me.scorePrediction = new ScorePrediction();

    me.url = {
        insertOrUpdateScorePrediction: LIVECHARTS.resolveUrl('ScorePrediction/InsertOrUpdateScorePrediction'),
        getCurrentUserPrediction: LIVECHARTS.resolveUrl('ScorePrediction/GetCharts'),
        getScorePredictionResult: LIVECHARTS.resolveUrl('ScorePrediction/GetScorePrictionResult'),
        resultPage: LIVECHARTS.resolveUrl('ScorePrediction/Index')
    };

    me.init = function () {
        me.txtHomeScore = $('#homePredictionValue');
        me.txtAwayScore = $('#awayPredictionValue');
        me.btnSubmitPrediction = $('#submitPreditionValue');
        me.smallContainerResult = $('#smallResultContainer');
        me.fullResultLink = $('#fullResultLink');
        me.memberId = Math.floor((Math.random() * 10000) + 1);
        me.registerEvents();
    };

    me.registerEvents = function () {
        me.btnSubmitPrediction.unbind('click').click(function () {
            var homeScore = me.txtHomeScore.val();
            var awayScore = me.txtAwayScore.val();
            me.insertOrUpdateScorePrediction({
                memberId: me.memberId,
                matchId: CRITERIA.matchId,
                homeScore: homeScore,
                awayScore: awayScore
            });
        });

        me.fullResultLink.unbind('click').click(function () {
            window.open(me.url.resultPage
                + '?matchId=' + CRITERIA.matchId
                + '&homeName=' + CRITERIA.homeNameEncoded
                + '&awayName=' + CRITERIA.awayNameEncoded,
                '',
                'width=660,height=640,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0,scrollbars=no,resizable=no');
        });
    };

    me.getCorrectScoreResult = function (params) { 
        LIVECHARTS.callAjax({
            url: me.url.getScorePredictionResult,
            type: 'POST',
            data: params,
            done: function (result) {
                var resultArray = me.scorePrediction.getHandicapResult(result);
                var scoreOption = me.scorePrediction.getScoreOption(resultArray);
                me.smallContainerResult.highcharts(scoreOption);
            }
        });
    };

    me.insertOrUpdateScorePrediction = function (params) {
        LIVECHARTS.callAjax({
            url: me.url.insertOrUpdateScorePrediction,
            type: 'POST',
            data: params,
            done: function (result) {
                if (result.IsSuccessful || result.Code === 1000) {
                    me.getCorrectScoreResult(params);
                }
            }
        });
    };
}


$(function () {
    var scorePredictionView = new ScorePredictionView();
    scorePredictionView.init();
});