/* File Created: March 20, 2014 */
/// <reference path="../../Views/Chat/Index.cshtml" />

/// <reference path="ChatSignalRAdapter.js" />
/// <reference path="Chat.js" />
/*global Chat, LIVECHARTS*/
function ChatWindow() {
    'use strict';
    var me = this;
    me.chatClient = new ChatClient();
    me.chatAdapter = new ChatSignalRAdapter();
    me.url = {
        getGetChatMessageByTimeUrl: LIVECHARTS.resolveUrl('Chat/getGetChatMessageByTime'),
        GetChatMessageByUserUrl: LIVECHARTS.resolveUrl('Chat/GetChatMessageByUser')
    };

    //// Innitial all information
    me.init = function () {
        try { $.blockUI(); } catch (e) { }
        me.cacheDOMElements();
        me.chatClient.init();
        me.chatAdapter.init(me, me.connectToServerDone);
        me.registerEvents();
    };

    //// Chat Client
    me.client = 
    {
        addMessage: function (chatMessage) {
            me.$mainChatRoom.append('<li>' + chatMessage.Content + '</li>');
        },
        sendMessage: function () {
            me.chatAdapter.server.sendMessage({
                ChatRoomId: DEFAULT_OBJECT.matchId,
                Content: me.$txtMessageInput.val(),
                ChatUser: {
                    MemberId: DEFAULT_OBJECT.memberId
                }
            }, DEFAULT_OBJECT.matchId);

            me.$txtMessageInput.empty();
        },
        joinRoom: function () {
            me.chatAdapter.server.joinRoom({
                ChatUserId: 1,
            });
        }
    };


    me.connectToServerDone = function () {
        try { $.unblockUI(); } catch (e) { }
    };


    //// cacheDomElements
    me.cacheDOMElements = function () {
        me.$soundContainer = $("#soundContainer");
        me.$mainChatRoom = $("#mainChatRoom");
        me.$txtMessageInput = $("#messageInput");
        me.$btnSubmitMessage = $("#submitMessage");
       
    };

    //// Events
    me.registerEvents = function () {
        me.$btnSubmitMessage.unbind('click').click(me.client.sendMessage);
    };

   
    //// Query Data
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

    //// Utilities
    me.playSound = function () {
        if (!me.$soundContainer.length) {
            me.$soundContainer = $("<div>").attr("id", "soundContainer").appendTo($("body"));
        }
        $soundContainer.html('<audio autoplay="autoplay"><source src="'
            + filename + '.mp3" type="audio/mpeg" /><source src="'
            + filename + '.ogg" type="audio/ogg" /><embed hidden="true" autostart="true" loop="false" src="'
            + filename + '.mp3" /></audio>');
    };
}


$(function () {
    'use strict';
    var chatWindow = new ChatWindow();
    chatWindow.init();
});