/// <reference path="../../Views/Charts/NewChartContent.cshtml" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="ScorePrediction.js" />
/*global LIVECHARTS, ScorePrediction, SCORE_PREDICTION, LANGUAGE*/
'use strict';

function ScorePredictionView(defaultValue) {
    var me = this;

    me.scorePrediction = new ScorePrediction();
    me.matchId = defaultValue.matchId;
    me.memberId = defaultValue.memberId;
    me.refreshMinutes = 1;
    me.isShowedResult = false;
    me.isStopPrediction = defaultValue.isStopScorePrediction || false;
    me.reUpdateSuccessCode = 1000;
    me.isFreezeSubmit = false;
    me.resultChart = null;

    me.url = {
        insertOrUpdateScorePrediction: LIVECHARTS.resolveUrl('ScorePrediction/InsertOrUpdateScorePrediction'),
        getCurrentUserPrediction: LIVECHARTS.resolveUrl('ScorePrediction/GetUserPrediction'),
        getScorePredictionResult: LIVECHARTS.resolveUrl('ScorePrediction/GetScorePrictionResult'),
        resultPage: LIVECHARTS.resolveUrl('ScorePrediction/ResultPage'),
        checkStopScorePrediction: LIVECHARTS.resolveUrl('ScorePrediction/CheckStopScorePrediction')
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
        me.$predictionContainer = $('#predictionContainer');
        me.$messageNoVotingInfo = $('#messageNoVotingInfo');

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
                if (result.MemberId || me.isStopPrediction) {
                    me.getCorrectScoreResult({
                        matchId: me.matchId
                    });
                }
                else {
                    me.$messageNoVotingInfo.show();
                }

                me.$txtHomeScore.val(result.HomeScore);
                me.$txtAwayScore.val(result.AwayScore);
            }
        });
    };

    me.registerEvents = function () {
        me.$btnSubmitPrediction.unbind('click').click(function () {
            if (me.isStopPrediction) {
                LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, LANGUAGE.scorePredictionStopMessage);
            }
            else {
                if (!me.isFreezeSubmit) {
                    me.isFreezeSubmit = true;
                    me.$btnSubmitPrediction.addClass('disabled');
                    var freezeSubmitTimeOutId = setTimeout(function () {
                        me.isFreezeSubmit = false;
                        me.$btnSubmitPrediction.removeClass('disabled');
                        clearTimeout(freezeSubmitTimeOutId);
                    }, 5000);

                    //// Get Data and submit prediction.
                    var homeScore = me.$txtHomeScore.val();
                    var awayScore = me.$txtAwayScore.val();
                    me.insertOrUpdateScorePrediction({
                        memberId: me.memberId,
                        matchId: defaultValue.matchId,
                        homeScore: homeScore,
                        awayScore: awayScore
                    });
                }
            }
        });

        me.$fullResultLink.unbind('click').click(function () {
            window.open(me.url.resultPage
                + '?matchId=' + defaultValue.matchId
                + '&homeName=' + defaultValue.homeNameEncoded
                + '&awayName=' + defaultValue.awayNameEncoded,
                'ScorePredictionResult' + defaultValue.matchId,
                'width=860,height=650,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0,scrollbars=no,resizable=no');
        });
    };

    me.loadRefreshCountDown = function () {
        me.$resultRefreshCountDown.parent().show();
        me.$resultRefreshCountDown.refreshCountDown('stopCountDown')
                    .refreshCountDown({
                        countDownMinutes: me.refreshMinutes,
                        countDownSeconds: 0,
                        refreshFunction: function (countDownObject, countDownStart) {
                            if (!me.isStopPrediction) {
                                me.getCorrectScoreResult({
                                    matchId: me.matchId
                                });
                                me.checkMatchStatus();

                                setTimeout(function () {
                                    countDownStart(countDownObject);
                                }, 1000);
                            }
                            else {
                                me.$resultRefreshCountDown.refreshCountDown('stopCountDown');
                            }
                        }
                    });
    };

    me.getCorrectScoreResult = function (params) {
        if (!me.isShowedResult) {
            me.$resultContainer.show();
            me.$messageNoVotingInfo.hide();
            if (!me.isStopPrediction) {
                me.loadRefreshCountDown();
            }
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
                me.resultChart = me.$smallContainerResult.highcharts(scoreOption);
            }
        });
    };

    me.stopCountDownAndRemoveReload = function () {
        if (me.isStopPrediction) {
            me.$resultRefreshCountDown.refreshCountDown('stopCountDown');
            me.$resultRefreshCountDown.parent().hide();
        }
    };

    me.checkMatchStatus = function () {
        if (!me.isStopPrediction) {
            LIVECHARTS.callAjax({
                url: me.url.checkStopScorePrediction,
                type: 'POST',
                data: {
                    matchId: defaultValue.matchId
                },
                done: function (result) {
                    if (result) {
                        me.isStopPrediction = result.IsStopScorePrediction;
                        me.stopCountDownAndRemoveReload();
                    }
                }
            });
        }
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
                    if (result.Code === 1) {
                        me.isStopPrediction = true;
                        me.stopCountDownAndRemoveReload();
                    }
                    LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, result.Message);
                }
            }
        });
    };
}


$(function () {
        var scorePredictionView = new ScorePredictionView(SCORE_PREDICTION);
        scorePredictionView.init();
});