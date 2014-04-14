/* File Created: April 14, 2014 */
/*global SCORE_PREDICTION_SETTING*/
function ScorePredictionSetting() {
    'use strict';
    var me = this;

    me.cachedElements = function () {
        me.$messageContainer = $('#messageContainer');
        me.$availableForm = $('#availableForm');
        me.$setingValue = $('#SettingValue');
        me.$messageList = $('#messageList');
    };

    me.init = function () {
        me.cachedElements();

        me.$availableForm.submit(function () {
            if (me.$setingValue.val().length > 4) {
                me.$messageList.find('li').html(SCORE_PREDICTION_SETTING.maxInputErrorMessage);
                me.$messageList.removeClass('validation-summary-valid').addClass('validation-summary-errors');
                me.$messageContainer.show();
                return false;
            }

            return true;
        });

        if (SCORE_PREDICTION_SETTING.defaultSucces) {
            setTimeout(function () {
                me.$messageContainer.hide();
            }, 3000);
        }


    };
}

$(function () {
    'use strict';
    var scorePredictionSetting = new ScorePredictionSetting();
    scorePredictionSetting.init();
});