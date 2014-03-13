/* File Created: March 13, 2014 */
/// <reference path="../live-charts-admin-site.js" />

function ScorePredictionConfigView() {
    var me = this;

    me.init = function () {
        me.$UpdateDummy = $('#updateDummy');
        me.registerEvents();
    };

    me.registerEvents = function () {
        me.$UpdateDummy.unbind('click').click(me.updateDummyPrediction);
    };

    me.updateDummyPrediction = function () {
        LiveChartsAdmin.callAjax({
            url: $('#updateDummyUrl').val(),
            type: 'POST',
            data: {
                ScorePredictionOptionId: 21,
                DummyCount: 3
            },
            success: function () { 
                location.reload();
            }
        });
    }
};


$(function () {
    var scorePredictionConfigView = new ScorePredictionConfigView();
    scorePredictionConfigView.init();
});