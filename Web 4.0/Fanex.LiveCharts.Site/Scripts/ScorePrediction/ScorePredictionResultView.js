/// <reference path="../../Views/ScorePrediction/Index.cshtml" />
/// <reference path="../../Views/Shared/_Layout.cshtml" />

/// <reference path="ScorePrediction.js" />
/*global LANGUAGE, LIVECHARTS, scorePredictionViewModel, ScorePrediction, isAdminSite*/
'use strict';

function ScorePredictionResultView(scorePredictionViewModel) {
    var me = this,
        reloadTimeOutMinutes = 1;
    
    /*Properties*/
    me.isLoading = false;
    me.isStopPrediction = scorePredictionViewModel.isStopScorePrediction || false;
    me.scorePrediction = new ScorePrediction(
        scorePredictionViewModel.homeName,
        scorePredictionViewModel.awayName,
        LANGUAGE.draw);

    if (typeof (isAdminSite) !== 'undefined') {
        me.scorePrediction.isAdminSite = isAdminSite;
    }

    me.url = {
        getScorePredictionResult: LIVECHARTS.resolveUrl('ScorePrediction/GetScorePrictionResult'),
        checkStopScorePrediction: LIVECHARTS.resolveUrl('ScorePrediction/CheckStopScorePrediction')
    };

    /*Functions*/
    me.init = function () {
        me.$oneXTwoResultContainer = $('#oneXTwoResultContainer');
        me.$oddEvenResultContainer = $('#oddEvenResultContainer');
        me.$correctScoreResultContainer = $('#correctScoreResultContainer');
        me.$handicapResultContainer = $('#handicapResultContainer');
        me.$refreshCountDownButton = $('.refresh-button-main');



        me.loadResult();
        if (!me.isStopPrediction) {
            me.startCountDown();
        }
    };

    me.startCountDown = function () {
        me.$refreshCountDownButton.parent().show();
        me.$refreshCountDownButton.refreshCountDown('stopCountDown').refreshCountDown(me.refreshCountDownOption);
    };

    me.refreshCountDownOption = {
        countDownMinutes: reloadTimeOutMinutes,
        countDownSeconds: 0,
        refreshFunction: function () {
            if (!me.isStopPrediction) {
                me.$refreshCountDownButton.find('.loading-icon').removeClass('loading-icon').addClass('wait-loading-icon');
                me.$refreshCountDownButton.find('.countdown-time').html('').hide();
                if (!me.isLoading) {
                    me.isLoading = true;
                    me.loadResult();
                }
                me.checkMatchStatus();

                setTimeout(function () {
                    me.$refreshCountDownButton.refreshCountDown('stopCountDown').refreshCountDown(me.refreshCountDownOption);
                }, 1000);
            }
        }
    };

    me.checkMatchStatus = function () {
        LIVECHARTS.callAjax({
            url: me.url.checkStopScorePrediction,
            type: 'POST',
            data: {
                matchId: scorePredictionViewModel.matchId
            },
            done: function (result) {
                me.isStopPrediction = result.IsStopScorePrediction;
                if (me.isStopPrediction) {
                    me.$refreshCountDownButton.parent().hide();
                    me.$refreshCountDownButton.refreshCountDown('stopCountDown');
                }
            }
        });
    };

    me.loadResult = function () {
        LIVECHARTS.callAjax({
            url: me.url.getScorePredictionResult,
            type: 'POST',
            data: {
                matchId: scorePredictionViewModel.matchId
            },
            done: function (result) {
                if (result) {
                    me.render1X2Result(result);
                    me.renderOddEvenResult(result);
                    me.renderCorrectScoreResult(result);
                    me.renderHandicapResult(result);
                    me.isLoading = false;
                }
            }
        });
    };

    /* Render result*/
    me.renderCorrectScoreResult = function (result) {
        var resultArray = me.scorePrediction.getCorrectScoreResult(result),
            getChartOption = me.scorePrediction.getScoreOption(resultArray, LANGUAGE.correctScore);
        
        me.$correctScoreResultContainer.highcharts(getChartOption);
    };

    me.renderHandicapResult = function (result) {
        var resultArray = me.scorePrediction.getHandicapResult(result),
            getChartOption = me.scorePrediction.getScoreOption(resultArray, LANGUAGE.handicap);

        me.$handicapResultContainer.highcharts(getChartOption);

    };

    me.render1X2Result = function (result) {
        var resultArray = me.scorePrediction.get1X2Result(result),
            getChartOption = me.scorePrediction.getBarOption(resultArray, '1x2');
        me.$oneXTwoResultContainer.highcharts(getChartOption);
    };

    me.renderOddEvenResult = function (result) {
        var resultArray = me.scorePrediction.getOddEvenResult(result),
            getChartOption = me.scorePrediction.getBarOption(resultArray, LANGUAGE.oddEven);
        me.$oddEvenResultContainer.highcharts(getChartOption);
    };
}

$(function () {
    var scorePredictionResultView = new ScorePredictionResultView(scorePredictionViewModel);
    scorePredictionResultView.init();
});