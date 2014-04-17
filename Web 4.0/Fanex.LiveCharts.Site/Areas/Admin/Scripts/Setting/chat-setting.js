/* File Created: April 8, 2014 */
/*global CHAT_SETTING, LiveChartsAdmin*/
function ChatSetting() {
    'use strict';
    var me = this;

    me.init = function () {
        me.cachedElements();
        me.$btnUpdateTermCondition.unbind('click').click(me.updateTermCondition);
        me.$btnUpdateWelcomeMessage.unbind('click').click(me.updateWelcomeMessage);
        me.$btnUpdateChatNameDenyList.unbind('click').click(me.updateChatNameDenyList);
    };

    me.cachedElements = function () {
        me.$btnUpdateTermCondition = $('#btnUpdateTermCondition');
        me.$txtTermCondition = $('#txtTermCondition');
        me.$txtWelcomeMessage = $('#txtWelcomeMessage');
        me.$btnUpdateWelcomeMessage = $('#btnUpdateWelcomeMessage');
        me.$cbWelcomeMessageDisabled = $('#cbWelcomeMessageDisabled');
        me.$cbTermConditionDisabled = $('#cbTermConditionDisabled');
        me.$btnUpdateChatNameDenyList = $('#btnUpdateChatNameDenyList');
        me.$txtChatNameDenyList = $('#txtChatNameDenyList');
    };



    me.updateTermCondition = function () {
        var termCondition = me.$txtTermCondition.val();
        if (me.isNotValid(termCondition)) {
            me.$txtTermCondition.focus();
            LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, CHAT_SETTING.invalidInput);
            return;
        }

        LiveChartsAdmin.callAjax({
            url: CHAT_SETTING.updateTermConditionUrl,
            type: 'POST',
            data: {
                termCondition: termCondition,
                disabled: me.$cbTermConditionDisabled.prop('checked')
            },
            done: function (commandResult) {
                if (commandResult.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, commandResult.Message);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                }
            }
        });

    };

    me.updateWelcomeMessage = function () {
        var welcomeMessage = me.$txtWelcomeMessage.val();
        if (me.isNotValid(welcomeMessage)) {
            me.$txtWelcomeMessage.focus();
            LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, CHAT_SETTING.invalidInput);
            return;
        }

        LiveChartsAdmin.callAjax({
            url: CHAT_SETTING.updateWelcomeMessageUrl,
            type: 'POST',
            data: {
                welcomeMessage: welcomeMessage,
                disabled: me.$cbWelcomeMessageDisabled.prop('checked')
            },
            done: function (commandResult) {
                if (commandResult.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, commandResult.Message);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                }
            }
        });

    };

    me.updateChatNameDenyList = function () {
        var chatNameDenyList = me.$txtChatNameDenyList.val();
        if (me.isNotValid(chatNameDenyList)) {
            me.$txtChatNameDenyList.focus();
            LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, CHAT_SETTING.invalidInput);
            return;
        }

        LiveChartsAdmin.callAjax({
            url: CHAT_SETTING.updateChatNameDenyListUrl,
            type: 'POST',
            data: {
                chatNameDenyList: chatNameDenyList
            },
            done: function (commandResult) {
                if (commandResult.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, commandResult.Message);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                }
            }
        });

    };

    me.isNotValid = function (value) {
        return !value || /^\s*$/.test(value);
    };
}


$(function () {
    'use strict';
    var chatSetting = new ChatSetting();
    chatSetting.init();
});


