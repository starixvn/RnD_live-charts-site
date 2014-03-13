/// <reference path="../../Views/Charts/NewChartContent.cshtml" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="ScorePrediction.js" />
/*global CRITERIA, LIVECHARTS, ScorePrediction, IS_ADMIN_SITE*/
'use strict';

function ScorePredictionView() {
    var me = this;

    me.scorePrediction = new ScorePrediction();
    me.matchId = CRITERIA.matchId;
    me.memberId = Math.floor((Math.random() * -10000) + 1);
    me.refreshMinutes = 1;
    me.isShowedResult = false;
    me.reUpdateSuccessCode = 1000;

    me.url = {
        insertOrUpdateScorePrediction: LIVECHARTS.resolveUrl('ScorePrediction/InsertOrUpdateScorePrediction'),
        getCurrentUserPrediction: LIVECHARTS.resolveUrl('ScorePrediction/GetUserPrediction'),
        getScorePredictionResult: LIVECHARTS.resolveUrl('ScorePrediction/GetScorePrictionResult'),
        resultPage: LIVECHARTS.resolveUrl('ScorePrediction/Index')
    };

    me.init = function () {
        me.$txtHomeScore = $('#homePredictionValue');
        me.$txtAwayScore = $('#awayPredictionValue');
        me.$inputScore = $('#homePredictionValue, #awayPredictionValue');
        me.$btnSubmitPrediction = $('#submitPreditionValue');
        me.$smallContainerResult = $('#smallResultContainer');
        me.$fullResultLink = $('#fullResultLink');
        me.$resultRefreshCountDown = $('#resultRefreshCountDown');
        me.$resultContainer = $('#resultContainer');

        //// Spinner

        me.$inputScore.stepper({ min: 0,
            max: 99,
            step: 1
        });


        me.loadDefaultScorePrediction();
        me.registerEvents();
    };

    me.loadDefaultScorePrediction = function () {
        LIVECHARTS.callAjax({
            url: me.url.getCurrentUserPrediction,
            type: 'POST',
            data: {
                memberId: me.memberId,
                matchId: me.matchId
            },
            useBlockUI: false,
            done: function (result) {
                if (result.MemberId) {
                    me.getCorrectScoreResult({
                        matchId: me.matchId
                    });
                }

                me.$txtHomeScore.val(result.HomeScore);
                me.$txtAwayScore.val(result.AwayScore);
            }
        });
    };

    me.registerEvents = function () {
        me.$btnSubmitPrediction.unbind('click').click(function () {
            var homeScore = me.$txtHomeScore.val();
            var awayScore = me.$txtAwayScore.val();
            me.insertOrUpdateScorePrediction({
                memberId: me.memberId,
                matchId: CRITERIA.matchId,
                homeScore: homeScore,
                awayScore: awayScore
            });
        });

        me.$fullResultLink.unbind('click').click(function () {
            window.open(me.url.resultPage
                + '?matchId=' + CRITERIA.matchId
                + '&homeName=' + CRITERIA.homeNameEncoded
                + '&awayName=' + CRITERIA.awayNameEncoded,
                'ScorePredictionResult',
                'width=1000,height=700,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0,scrollbars=no,resizable=no');
        });
    };

    me.loadRefreshCountDown = function () {
        me.$resultRefreshCountDown.parent().show();
        me.$resultRefreshCountDown.refreshCountDown('stopCountDown')
                    .refreshCountDown({
                        countDownMinutes: me.refreshMinutes,
                        countDownSeconds: 0,
                        refreshFunction: function (countDownObject, countDownStart) {
                            me.getCorrectScoreResult({
                                matchId: me.matchId
                            });
                            setTimeout(function () {
                                countDownStart(countDownObject);
                            }, 1000);
                        }
                    });
    };

    me.getCorrectScoreResult = function (params) {
        if (!me.isShowedResult) {
            me.$resultContainer.show();
            me.loadRefreshCountDown();
            me.isShowedResult = true;
        }

        LIVECHARTS.callAjax({
            url: me.url.getScorePredictionResult,
            type: 'POST',
            data: params,
            useBlockUI: false,
            done: function (result) {
                var resultArray = me.scorePrediction.getCorrectScoreResult(result);
                var scoreOption = me.scorePrediction.getScoreOption(resultArray, 'Correct Score');
                me.$smallContainerResult.highcharts(scoreOption);
            }
        });
    };

    me.insertOrUpdateScorePrediction = function (params) {
        LIVECHARTS.callAjax({
            url: me.url.insertOrUpdateScorePrediction,
            type: 'POST',
            data: params,
            done: function (result) {
                if (result.IsSuccessful || result.Code === me.reUpdateSuccessCode) {
                    LIVECHARTS.alertMessage(LIVECHARTS.messageType.success, result.Message);
                    me.getCorrectScoreResult(params);
                }
                else {
                    LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, result.Message);
                }
            }
        });
    };
}


$(function () {
    if (!IS_ADMIN_SITE) {
        var scorePredictionView = new ScorePredictionView();
        scorePredictionView.init();
    }
});