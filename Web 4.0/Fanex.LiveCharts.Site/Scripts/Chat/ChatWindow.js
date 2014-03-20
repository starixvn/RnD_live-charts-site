
/* File Created: March 20, 2014 */
/// <reference path="../../Views/Chat/Index.cshtml" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="ChatSignalRAdapter.js" />
/// <reference path="Chat.js" />
/*global Chat, LIVECHARTS*/
function ChatWindow() {
    'use strict';
    var me = this;
    me.chatUser =
        {
            chatUserId: DEFAULT_OBJECT.chatUserId,
            chatName: DEFAULT_OBJECT.chatName,
            memberId: DEFAULT_OBJECT.memeberId
        };
    me.chatClient = new ChatClient();
    me.chatAdapter = new ChatSignalRAdapter();
    me.url = {
        getGetChatMessageByTimeUrl: LIVECHARTS.resolveUrl('Chat/getGetChatMessageByTime'),
        getChatMessageByUserUrl: LIVECHARTS.resolveUrl('Chat/GetChatMessageByUser'),
        insertChatUserUrl: LIVECHARTS.resolveUrl('Chat/InsertChatUserUrl')
    };

    //// Innitial all information
    me.init = function () {
        try { $.blockUI(); } catch (e) { }
        me.cacheDOMElements();
        me.chatClient.init();
        me.checkValidUser();
    };
    //// cacheDomElements
    me.cacheDOMElements = function () {
        me.$soundContainer = $('#soundContainer');
        me.$mainChatRoom = $('#mainChatRoom');
        me.$txtMessageInput = $('#messageInput');
        me.$btnSubmitMessage = $('#submitMessage');
        me.$memberOnline = $('#memberOnline')
    };
    //// Events
    me.registerEvents = function () {
        me.$btnSubmitMessage.unbind('click').click(me.client.sendMessage);
    };
    me.checkValidUser = function () {
        if (DEFAULT_OBJECT.chatName === '') {
            me.insertChatUser();
        }
        else {
            me.connectServer();
        }
    };
    me.insertChatUser = function () {
        LIVECHARTS.callAjax({
            url: me.url.getGetChatMessageByTimeUrl,
            type: 'POST',
            data: {
                memberId: me.chatUser.chatUserId
            },
            useBlockUI: false,
            done: function (result) {
                if (result.IsSuccessful) {
                    me.connectServer();
                }
                else {

                }
            }
        });
    }
    me.connectServer = function () {
        me.chatAdapter.init(me, me.connectToServerDone);
    }
    me.connectToServerDone = function () {
        try { $.unblockUI(); } catch (e) { };
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
                ChatUserId: me.chatUser.chatUserId,
                MemberId: me.chatUser.memberId
            });
        },
        userListChange: function (totalUser) {
            if (me.$memberOnline) {
                me.$memberOnline.html(totalUser);
            }
        },
        notifyUserLeftRoom: function (chatUser) {
            me.$mainChatRoom.append('<li>' + chatUser.ChatName + ' has left.</li>');
        },
        notifyUserJoinedRoom: function (chatUser) {
            me.$mainChatRoom.append('<li>' + chatUser.chatName + ' joined room.</li>');
        }
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
    me.checkBadWords = function (messageText) {
            
    };
}


$(function () {
    'use strict';
    var chatWindow = new ChatWindow();
    chatWindow.init();
});