/* File Created: March 20, 2014 */
/// <reference path="../../Views/Chat/Index.cshtml" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="ChatSignalRAdapter.js" />
/// <reference path="Chat.js" />
/// <reference path="ChatServer.js" />

/*global LIVECHARTS, 
        CURRENT_CHAT_USER, 
        ChatClient, 
        CURRENT_CHAT_ROOM, 
        CURRENT_CHAT_USER_IN_ROOM, 
        ChatClient,
        ChatSignalRAdapter,
        ChatServer*/
function ChatWindow() {
    'use strict';
    var me = this;

    //// Properties
    me.chatUser = CURRENT_CHAT_USER;
    me.chatRoom = CURRENT_CHAT_ROOM;
    me.chatUserInRoom = CURRENT_CHAT_USER_IN_ROOM;
    me.chatAdapter = new ChatSignalRAdapter();
    me.chatClient = new ChatClient(me.chatUser, me.chatRoom);
    me.chatServer = new ChatServer(me.chatAdapter, me.chatClient, me.chatUser, me.chatRoom, me.chatUserInRoom);
    me.formatTimer = CURRENT_TIME_FORMAT;
    me.url = {
        getGetChatMessageByTimeUrl: LIVECHARTS.resolveUrl('Chat/GetGetChatMessageByTime'),
        getChatMessageByUserUrl: LIVECHARTS.resolveUrl('Chat/GetChatMessageByUser'),
        insertChatUserUrl: LIVECHARTS.resolveUrl('Chat/InsertChatUser')
    };

    //// Innitial all information
    me.init = function () {
        try { $.blockUI(); } catch (e) { }
        me.cacheDOMElements();
        me.chatClient.init();
        me.chatServer.init();
        me.checkValidUser();
        me.startTimer();
    };

    //// cacheDomElements
    me.cacheDOMElements = function () {
        me.$soundContainer = $('#soundContainer');
        me.$mainChatRoom = $('#mainChatRoom');
        me.$txtMessageInput = $('#messageInput');
        me.$btnSubmitMessage = $('#submitMessage');
        me.$memberOnline = $('#memberOnline');
        me.$btnEmotiocons = $('#emobitonButton');
        me.$gmt8Timer = $('#gmt8Timer');
    };
    //// Events
    me.registerEvents = function () {
        me.$btnSubmitMessage.unbind('click').click(me.chatServer.sendMessage);
        me.$memberOnline.unbind('click').click(me.chatClient.showDetailUserList);
        
        
        //// Send Spam Notify: implement later
        //// Auto Enter
        //// 
    };

    me.checkValidUser = function () {
        if (me.chatUser.chatName === '') {
            var chatName = prompt('Enter your name:', '');
            me.insertChatUser(chatName);
        }
        else {
            me.connectServer();
        }
    };

    me.insertChatUser = function (chatName) {
        if (chatName != null && chatName !== '') {
            LIVECHARTS.callAjax({
                url: me.url.insertChatUserUrl,
                type: 'POST',
                data: {
                    memberId: me.chatUser.memberId,
                    chatName: chatName
                },
                useBlockUI: false,
                done: function (result) {
                    if (result && result.CommandResult.IsSuccessful) {
                        me.chatUser.chatUserId = result.InsertedId;
                        me.chatUser.chatName = chatName;
                        me.connectServer();
                    }
                    else {
                        alert(result.Message);
                    }
                }
            });
        }
        else
        {
            try { $.unblockUI(); } catch (e) { }
            alert('Username cannot be null!');
        }
    };

    me.connectServer = function () {
        me.chatAdapter.init(me, me.connectToServerDone);
    };
    me.connectToServerDone = function () {
        try { $.unblockUI(); } catch (e) { }
        me.registerEvents();
        me.chatServer.joinRoom();
        
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
//        if (!me.$soundContainer.length) {
//            me.$soundContainer = $("<div>").attr("id", "soundContainer").appendTo($("body"));
//        }
//        $soundContainer.html('<audio autoplay="autoplay"><source src="'
//            + filename + '.mp3" type="audio/mpeg" /><source src="'
//            + filename + '.ogg" type="audio/ogg" /><embed hidden="true" autostart="true" loop="false" src="'
//            + filename + '.mp3" /></audio>');
    };
    
    
    me.checkBadWords = function (messageText) {

    };

    me.startTimer = function () {
        setInterval(function () {
            me.$gmt8Timer.html(moment().zone(me.formatTimer.timeZone).format(me.formatTimer.format));
        }, 1000);
    };
}


$(function () {
    'use strict';
    var chatWindow = new ChatWindow();
    chatWindow.init();
});