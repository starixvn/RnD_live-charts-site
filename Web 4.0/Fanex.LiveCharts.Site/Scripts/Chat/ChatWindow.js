/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />
/// <reference path="Chat.js" />
/*global Chat, LIVECHARTS*/
function ChatWindow() {
    'use strict';
    var me = this;

    //// Cache Element


    me.chat = new Chat();
    me.chatAdapter = new ChatSignalRAdapter();

    //// Innitial all information
    me.init = function () {
        me.url = {
            getGetChatMessageByTimeUrl: LIVECHARTS.resolveUrl('Chat/getGetChatMessageByTime'),
            GetChatMessageByUserUrl: LIVECHARTS.resolveUrl('Chat/GetChatMessageByUser')
        };
        me.cacheDOMElements();
        me.chat.init();
        me.chatAdapter.init(me, me.doneConnectToServer);
    };

    //// cacheDomElements
    me.cacheDOMElements = function () {
        me.$soundContainer = $("#soundContainer");
        me.$mainChatRoom = $("#mainChatRoom");
        me.$txtMessageInput = $("#messageInput");
        me.$btnSubmitMessage = $("#submitMessage");
    };

    me.playSound = function () {
        if (!me.$soundContainer.length) {
            me.$soundContainer = $("<div>").attr("id", "soundContainer").appendTo($("body"));
        }
        $soundContainer.html('<audio autoplay="autoplay"><source src="'
            + filename + '.mp3" type="audio/mpeg" /><source src="'
            + filename + '.ogg" type="audio/ogg" /><embed hidden="true" autostart="true" loop="false" src="' 
            + filename + '.mp3" /></audio>');
    };

    me.getHistoryMessage = function () {
        LIVECHARTS.callAjax({
            url: me.url.getGetChatMessageByTimeUrl,
            type: 'POST',
            data: me.chat.getChartMessageCriteria,
            useBlockUI: false,
            done: function (result) {
                //// Handle Later
            }
        });
    };


}


$(function () {
    'use strict';
    var chatWindow = new ChatWindow();
    chatWindow.init();
});