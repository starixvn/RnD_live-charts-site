/* File Created: March 13, 2014 */
/// <reference path="../../Views/ScorePrediction/ScorePredictionConfig.cshtml" />
/// <reference path="../live-charts-admin-site.js" />
/*global LiveChartsAdmin,SCORE_PREDICTION_CONFIG*/
function ScorePredictionConfigView() {
    'use strict';
    var me = this;

    me.stepperOption = { 
            min: 0,
            max: 99,
            step: 1
    };

    me.init = function () {
        me.$btnUpdateDummy = $('#updateDummy');
        me.$txtHomeScore = $('#homeScore');
        me.$txtAwayScore = $('#awayScore');
        me.$txtPredictionValue = $('#predictionValue');
        me.$frameResultChart = $('#frmChartContent');
        me.$tblScorePredictionLog = $('#predictionDummyGrid');
        me.$btnResetValue = $('#resetDummy');
        me.$messageInfo = $('#messageNotification');
        me.$messageSummaryContainer = $('#messageDetailsContainer');
        me.$messageSummaryDetails = $('#messageDetailsContainer .messageDetails');
        me.$predictionOptionId = $('#predictionOptionId');
        me.registerEvents();
    };

    me.registerEvents = function () {
        me.$btnUpdateDummy.unbind('click').click(me.updateDummyPrediction);
        me.$btnResetValue.unbind('click').click(me.resetValue);
        me.$txtHomeScore.stepper(me.stepperOption);
        me.$txtAwayScore.stepper(me.stepperOption);
        me.$txtPredictionValue.parent().find();
        $('#predictionDummyGrid .edit-dummy-value').unbind('click').click(me.selectScorePredictionOption);
    };

    me.selectScorePredictionOption = function () {
        var $this = $(this);
        LiveChartsAdmin.callAjax({
            url: SCORE_PREDICTION_CONFIG.getScorePredictionoptionUrl,
            type: 'POST',
            data: {
                scorePredictionOptionId: $this.attr('data-sp-id')
            },
            done: function (scorePredictionOption) {
                if (scorePredictionOption) {
                    me.$txtHomeScore.val(scorePredictionOption.HomeScore);
                    me.$txtAwayScore.val(scorePredictionOption.AwayScore);
                    me.$txtPredictionValue.val(scorePredictionOption.DummyCount);
                    me.$predictionOptionId.val(scorePredictionOption.ScorePredictionOptionId);
                }
            }
        });
        //
    };

    me.resetValue = function () {
        me.$txtHomeScore.val(0);
        me.$txtAwayScore.val(0);
        me.$txtPredictionValue.val(0);
        me.$predictionOptionId.val(0);
    };

    me.updateDummyPrediction = function () {
        if (me.isValidInput()) {
            var dummyValue = me.$txtPredictionValue.val();
            if (dummyValue === '' || dummyValue === '0') {
                var cf = confirm(SCORE_PREDICTION_CONFIG.deleteMessage);
                if (cf === false) {
                    return;
                }
            }

            LiveChartsAdmin.callAjax({
                url: SCORE_PREDICTION_CONFIG.updateDummyUrl,
                type: 'POST',
                data: {
                    HomeScore: me.$txtHomeScore.val(),
                    AwayScore: me.$txtAwayScore.val(),
                    MatchId: SCORE_PREDICTION_CONFIG.matchId,
                    DummyCount: dummyValue
                },
                done: function (commandResult) {
                    if (commandResult.IsSuccessful) {
                        me.$frameResultChart.attr('src', me.$frameResultChart.attr('src'));
                        me.updateScorePredictionLogTable();
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, commandResult.Message);
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                    }
                }
            });
        }
    };

    me.updateScorePredictionLogTable = function () {
        LiveChartsAdmin.callAjax({
            url: SCORE_PREDICTION_CONFIG.updateScorePredictionLogTableUrl,
            type: 'GET',
            dataType: 'html',
            data: {
                MatchId: SCORE_PREDICTION_CONFIG.matchId
            },
            done: function (result) {
                me.$tblScorePredictionLog.empty().html(result);
                $('#predictionDummyGrid .edit-dummy-value').unbind('click').click(me.selectScorePredictionOption);
            }
        });
    };

    me.isValidInput = function () {
        me.$messageSummaryContainer.hide();
        me.$messageSummaryDetails.empty();
        me.$txtPredictionValue.removeClass('input-validation-error');

        var currentPredictionValue = me.$txtPredictionValue.val(),
            isEdit = $('#predictionDummyGrid').find('.edit-dummy-value[data-home-score=' + me.$txtHomeScore.val() + ']'
                                                    + '[data-away-score=' + me.$txtAwayScore.val() + ']'),
                                                    isValid = true;
        if (isEdit.length === 0 && parseInt(currentPredictionValue) <= 0) {
            me.$messageSummaryDetails.append('<li>' + SCORE_PREDICTION_CONFIG.addNewInvalidMessage + '</li>');
            isValid = false;
        }
        else if (isNaN(currentPredictionValue) || parseInt(currentPredictionValue) < 0) {
            me.$messageSummaryDetails.append('<li>' + SCORE_PREDICTION_CONFIG.errorMessage + '</li>');
            isValid = false;
        }
        else if (currentPredictionValue.length >= 9) {
            me.$messageSummaryDetails.append('<li>' + SCORE_PREDICTION_CONFIG.maxNumberErrorMessage + '</li>');
            isValid = false;
        }

        if (!isValid) {
            me.$messageSummaryContainer.show();
            me.$txtPredictionValue.addClass('input-validation-error');
        }

        return isValid;
    };
}


$(function () {
    'use strict';

    var scorePredictionConfigView = new ScorePredictionConfigView();
    scorePredictionConfigView.init();
});