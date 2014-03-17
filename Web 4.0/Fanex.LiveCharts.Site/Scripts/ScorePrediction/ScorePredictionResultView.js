/// <reference path="../../Views/ScorePrediction/Index.cshtml" />
/// <reference path="../../Views/Shared/_Layout.cshtml" />

/// <reference path="ScorePrediction.js" />
/*global LANGUAGE, LIVECHARTS, scorePredictionViewModel, ScorePrediction*/
'use strict';

function ScorePredictionResultView() {
    var me = this,
        reloadTimeOutMinutes = 1;
    
    /*Properties*/
    me.isLoading = false;
    me.scorePrediction = new ScorePrediction(
        scorePredictionViewModel.homeName,
        scorePredictionViewModel.awayName,
        LANGUAGE.draw);

    if (typeof (isAdminSite) !== 'undefined') {
        me.scorePrediction.isAdminSite = isAdminSite;
    }

    me.url = {
        getScorePredictionResult: LIVECHARTS.resolveUrl('ScorePrediction/GetScorePrictionResult')
    };

    /*Functions*/
    me.init = function () {
        me.$oneXTwoResultContainer = $('#oneXTwoResultContainer');
        me.$oddEvenResultContainer = $('#oddEvenResultContainer');
        me.$correctScoreResultContainer = $('#correctScoreResultContainer');
        me.$handicapResultContainer = $('#handicapResultContainer');
        me.$refreshCountDownButton = $('.refresh-button-main');

        

        me.loadResult();
        me.$refreshCountDownButton.parent().show();
        me.$refreshCountDownButton.refreshCountDown('stopCountDown').refreshCountDown(me.refreshCountDownOption);
    };

    me.refreshCountDownOption = {
        countDownMinutes: reloadTimeOutMinutes,
        countDownSeconds: 0,
        refreshFunction: function () {
            me.$refreshCountDownButton.find('.loading-icon').removeClass('loading-icon').addClass('wait-loading-icon');
            me.$refreshCountDownButton.find('.countdown-time').html('').hide();
            if (!me.isLoading) {
                me.isLoading = true;
                me.loadResult();
            }

            setTimeout(function () {
                me.$refreshCountDownButton.refreshCountDown('stopCountDown').refreshCountDown(me.refreshCountDownOption);
            }, 1000);
        }
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
            getChartOption = me.scorePrediction.getScoreOption(resultArray, 'Correct Score');
        
        me.$correctScoreResultContainer.highcharts(getChartOption);
    };

    me.renderHandicapResult = function (result) {
        var resultArray = me.scorePrediction.getHandicapResult(result),
            getChartOption = me.scorePrediction.getScoreOption(resultArray, 'Handicap');

        me.$handicapResultContainer.highcharts(getChartOption);

    };

    me.render1X2Result = function (result) {
        var resultArray = me.scorePrediction.get1X2Result(result),
            getChartOption = me.scorePrediction.getBarOption(resultArray, '1x2');
        me.$oneXTwoResultContainer.highcharts(getChartOption);
    };

    me.renderOddEvenResult = function (result) {
        var resultArray = me.scorePrediction.getOddEvenResult(result),
            getChartOption = me.scorePrediction.getBarOption(resultArray, 'Odd/Even');
        me.$oddEvenResultContainer.highcharts(getChartOption);
    };
}

$(function () {
    var scorePredictionResultView = new ScorePredictionResultView();
    scorePredictionResultView.init();
});