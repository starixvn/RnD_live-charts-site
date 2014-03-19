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
        me.registerEvents();
    };

    me.registerEvents = function () {
        me.$btnUpdateDummy.unbind('click').click(me.updateDummyPrediction);
        me.$btnResetValue.unbind('click').click(me.resetValue);
        me.$txtHomeScore.stepper(me.stepperOption);
        me.$txtAwayScore.stepper(me.stepperOption);
        me.$txtPredictionValue.parent().find()
        $('#predictionDummyGrid .edit-dummy-value').off('click').on('click', me.selectScorePredictionOption);
    };

    me.selectScorePredictionOption = function () {
        var $this = $(this);
        me.$txtHomeScore.val($this.attr('data-home-score'));
        me.$txtAwayScore.val($this.attr('data-away-score'));
        me.$txtPredictionValue.val($this.attr('data-dummy-value'));
    };

    me.resetValue = function () {
        me.$txtHomeScore.val(0);
        me.$txtAwayScore.val(0);
        me.$txtPredictionValue.val(0);
    };

    me.updateDummyPrediction = function () {
        if (me.isValidInput()) {
            LiveChartsAdmin.callAjax({
                url: SCORE_PREDICTION_CONFIG.updateDummyUrl,
                type: 'POST',
                data: {
                    HomeScore: me.$txtHomeScore.val(),
                    AwayScore: me.$txtAwayScore.val(),
                    MatchId: SCORE_PREDICTION_CONFIG.matchId,
                    DummyCount: me.$txtPredictionValue.val()
                },
                done: function (commandResult) {
                    if (commandResult.IsSuccessful) {
                        me.$frameResultChart.attr('src', me.$frameResultChart.attr('src'));
                        me.updateScorePredictionLogTable();
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, commandResult.Message);
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, commandResult.Message);
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
                $('#predictionDummyGrid .edit-dummy-value').off('click').on('click', me.selectScorePredictionOption);
            }
        });
    };

    me.isValidInput = function () {
        me.$messageSummaryContainer.hide();
        me.$messageSummaryDetails.empty();
        me.$txtPredictionValue.removeClass('input-validation-error');

        if (isNaN(me.$txtPredictionValue.val())) {

            me.$messageSummaryContainer.show();
            me.$txtPredictionValue.addClass('input-validation-error');
            me.$messageSummaryDetails.append('<li>' + SCORE_PREDICTION_CONFIG.errorMessage + '</li>');
            return false;
        }

        return true;
    };
}


$(function () {
    'use strict';

    var scorePredictionConfigView = new ScorePredictionConfigView();
    scorePredictionConfigView.init();
});