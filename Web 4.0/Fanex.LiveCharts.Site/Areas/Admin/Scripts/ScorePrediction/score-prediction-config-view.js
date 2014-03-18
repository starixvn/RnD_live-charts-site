/* File Created: March 13, 2014 */
/// <reference path="../../Views/ScorePrediction/ScorePredictionConfig.cshtml" />
/// <reference path="../live-charts-admin-site.js" />
/*global LiveChartsAdmin,scorePredictionConfig*/
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
        me.messageInfo = $('#messageNotification');
        me.registerEvents();
    };

    me.registerEvents = function () {
        me.$btnUpdateDummy.unbind('click').click(me.updateDummyPrediction);
        me.$txtHomeScore.stepper(me.stepperOption);
        me.$txtAwayScore.stepper(me.stepperOption);
        $('#predictionDummyGrid .edit-dummy-value').off('click').on('click', me.selectScorePredictionOption);
    };

    me.selectScorePredictionOption = function () {
        var $this = $(this);
        me.$txtHomeScore.val($this.attr('data-home-score'));
        me.$txtAwayScore.val($this.attr('data-away-score'));
        me.$txtPredictionValue.val($this.attr('data-dummy-value'));
    };

    me.updateDummyPrediction = function () {
        LiveChartsAdmin.callAjax({
            url: scorePredictionConfig.updateDummyUrl,
            type: 'POST',
            data: {
                HomeScore: me.$txtHomeScore.val(),
                AwayScore: me.$txtAwayScore.val(),
                MatchId: scorePredictionConfig.matchId,
                DummyCount: me.$txtPredictionValue.val()
            },
            done: function () {
                me.$frameResultChart.attr('src', me.$frameResultChart.attr('src'));
                me.updateScorePredictionLogTable();
                me.messageInfo.empty()
                            .removeClass('error-message').addClass('valid-message')
                            .html(me.messageInfo.attr('data-success-message')).show()
                            .delay(3000).fadeOut();
            }
        });
    };

    me.updateScorePredictionLogTable = function () {
        LiveChartsAdmin.callAjax({
            url: scorePredictionConfig.updateScorePredictionLogTableUrl,
            type: 'GET',
            dataType: 'html',
            data: {
                MatchId: scorePredictionConfig.matchId
            },
            done: function (result) {
                me.$tblScorePredictionLog.empty().html(result);
                $('#predictionDummyGrid .edit-dummy-value').off('click').on('click', me.selectScorePredictionOption);
            }
        });
    };
}


$(function () {
    'use strict';

    var scorePredictionConfigView = new ScorePredictionConfigView();
    scorePredictionConfigView.init();
});