/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />
/*global moment, CURRENT_TIME_FORMAT, DEFAULT_OBJECT, swearWords, LIVECHARTS, LANGUAGE*/
/*exported ChatClient*/
function ChatClient(chatUser, chatRoom) {
    'use strict';
    var me = this;

    me.chatUser = chatUser;
    me.chatRoom = chatRoom;
    me.currentUserList = null;
    me.currentMessageHistory = null;
    me.formatTimer = CURRENT_TIME_FORMAT;
    me.lastMessageBelongToChatUserId = 0;
    me.firstMessageBelongToChatUserId = 0;
    me.isNextMessageFromSameUser = false;
    me.numberOfHistoryMessagesShowing = 12;
    me.numberOfHistoryMessagesPaging = 100;
    me.isBrowserHidden = false;
    me.historyMessages = [];
    me.userList = {};

    me.cachedDomElement = function () {

        me.$mainChatRoom = $('#mainChatRoom');
        me.$parentMainChatRoom = me.$mainChatRoom.parent();
        me.$txtMessageInput = $('#messageInput');
        me.$btnSubmitMessage = $('#submitMessage');
        me.$memberOnline = $('#memberOnline');
        me.$btnShowHistoryMessage = $('#showMessageHistory');
    };
    me.init = function () {
        me.cachedDomElement();
    };

    me.addMessage = function (chatMessage, stopScrollTop) {
        if (typeof chatMessage === 'object') {
            me.isNextMessageFromSameUser = me.lastMessageBelongToChatUserId === chatMessage.ChatUser.ChatUserId;
            chatMessage = me.preProcessMessage(chatMessage);
            if (typeof me.userList[chatMessage.ChatUser.ChatUserId] === 'undefined') {
                me.userList[chatMessage.ChatUser.ChatUserId] = me.randomColor();
            }

            var messageText = me.buildMessage(chatMessage, me.isNextMessageFromSameUser);
            me.lastMessageBelongToChatUserId = chatMessage.ChatUser.ChatUserId;

            me.$mainChatRoom.append(messageText);
            if (!stopScrollTop) {
                me.scrollTop();
            }

            if (me.isBrowserHidden) {
                me.playSound(DEFAULT_OBJECT.notificationSound);
            }
        }
    };

    me.prependHistoryMessage = function (chatMessage, chatPreviousUserId) {
        if (typeof chatMessage === 'object') {
            me.isPreviousMessageFromSameUser = chatPreviousUserId === chatMessage.ChatUser.ChatUserId;
            chatMessage = me.preProcessMessage(chatMessage);
            if (typeof me.userList[chatMessage.ChatUser.ChatUserId] === 'undefined') {
                me.userList[chatMessage.ChatUser.ChatUserId] = me.randomColor();
            }
            var messageText = me.buildMessage(chatMessage, me.isPreviousMessageFromSameUser);
            me.$mainChatRoom.prepend(messageText);
        }
    };

    me.buildMessage = function (chatMessage, isNextMessageFromSameUser) {
        return '<li class="chat-message"'
                    + ' data-message-id="' + chatMessage.ChatMessageId + '"'
                    + ' data-chat-user-id="' + chatMessage.ChatUser.ChatUserId + '">'
                    + (isNextMessageFromSameUser ?
                        '' :
                        '<div class="message-chat-name" style="color:' + me.userList[chatMessage.ChatUser.ChatUserId] + ';">'
                            + chatMessage.ChatUser.ChatName
                        + ':</div>')
                    + '<div class="message-content">'
                        + '<div class="message-content-main">' + chatMessage.Content + '</div>'
                        + '<div class="message-content-time">' + chatMessage.MessageTime + '</div>'
                    + '</div>'
                + '</li>';
    };

    me.preProcessMessage = function (chatMessage) {
        chatMessage.MessageTime = moment(chatMessage.CreateTime).zone(me.formatTimer.timeZone).format(me.formatTimer.format);
        chatMessage.Content = chatMessage.Content.replace(/\r\n/g, "<br />").replace(/\n/g, "<br />");
        var tempDiv = $('<div>' + chatMessage.Content + '</div>');
        var chatMessageContent = tempDiv.emoticons(DEFAULT_OBJECT.emoticonsFolder).profanityFilter({
            customSwears: swearWords
        });
        chatMessage.Content = chatMessageContent.html();
        chatMessageContent = null;

        return chatMessage;
    };

    me.updateChatMessageHistory = function (chatMessages) {
        me.historyMessages = chatMessages;
        var messageLength = me.historyMessages.length;
        var lastMessageArray = [];

        if (me.numberOfHistoryMessagesShowing > messageLength) {
            lastMessageArray = me.historyMessages;
        }
        else {
            lastMessageArray = me.historyMessages.slice(messageLength - me.numberOfHistoryMessagesShowing, messageLength);
            me.$btnShowHistoryMessage.unbind('click').click(me.showPreviousMessages).show();
        }

        for (var index in lastMessageArray) {
            var currentMesage = lastMessageArray[index];
            if (parseInt(index) === 0) {
                me.firstMessageBelongToChatUserId = currentMesage.ChatUser.ChatUserId;
            }
            me.addMessage(currentMesage, true);
        }

        me.scrollTop();
    };

    me.updateUserList = function (userList) {
        if (me.$memberOnline && userList) {
            me.$memberOnline.html(userList.length);
            me.currentUserList = userList;
            me.scrollTop();
        }

    };

    me.showPreviousMessages = function () {
        var currentShowingMessageIndex = me.numberOfHistoryMessagesShowing,
            currentMessageLength = me.historyMessages.length,
            fromIndex = 0,
            toIndex = currentMessageLength - currentShowingMessageIndex - 1;
        if (currentShowingMessageIndex + me.numberOfHistoryMessagesPaging < currentMessageLength) {
            fromIndex = me.numberOfHistoryMessagesPaging - me.numberOfHistoryMessagesPaging;
        }

        for (var i = toIndex; i >= fromIndex; i--) {
            var previousUserId = 0;
            if (i > 0) {
                previousUserId = me.historyMessages[i - 1].ChatUser.ChatUserId;
            }
            me.prependHistoryMessage(me.historyMessages[i], previousUserId);
        }

        if (fromIndex === 0) {
            me.$btnShowHistoryMessage.hide();
        }

        me.numberOfHistoryMessagesShowing += me.numberOfHistoryMessagesPaging;
    };

    me.showDetailUserList = function () {
        if(me.currentUserList && me.currentUserList.length > 0)
        {
            var userChatNameList = [];
            for (var index in me.currentUserList) {
                var chatUser = me.currentUserList[index];
                userChatNameList.push(chatUser.ChatName);
            }

            alert(userChatNameList.join(',\n'));
        }
    };

    me.kickOutUserChat = function (chatUser) {
        if(chatUser && chatUser.chatUserId === me.chatUser.chatUserId)
        {
            alert('User is kicked out!');
            // User kickout handler
        }
    };

    me.notifyUserLeftRoom = function (chatUserInRoom, currentChatUserList) {
        if (chatUserInRoom.ChatUserId !== me.chatUser.chatUserId) {
            me.renderUserNotification(chatUserInRoom, ' has left.', false);
        }
        me.updateUserList(currentChatUserList);
    };

    me.notifyUserJoinedRoom = function (chatUser, currentChatUserList) {
        if (chatUser && chatUser.ChatName && chatUser.ChatUserId !== me.chatUser.chatUserId) {
            me.renderUserNotification(chatUser, ' joined room.', true);
        }
        me.updateUserList(currentChatUserList);
    };

    me.kickoutBecauseChatSameRoom = function (chatUser) {
        /*jshint unused:false*/
        LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, LANGUAGE.youAreDisconnect);
        var disconnectTimeOutId = setTimeout(function () {
            location.href = DEFAULT_OBJECT.disconnectPage;
            clearTimeout(disconnectTimeOutId);
        }, 5000);
    };

    me.renderUserNotification = function (chatUser, messageText, isJoin) {
        var message = '<span class="notification-name">' + chatUser.ChatName + '</span>'
                    + '<span class="notification-message">' + messageText + '</span>';
        var classMessage = isJoin ? 'joined-message' : 'left-message';

        me.$mainChatRoom.append('<li class="chat-message">'
                    + '<div class="message-content notification-content">'
                        + '<div class="message-content-main ' + classMessage + '">'
                                + message
                        + '</div>'
                        + '<div class="message-content-time">' + moment().zone(me.formatTimer.timeZone).format(me.formatTimer.format) + '</div>'
                    + '</div>'
               + '</li>');
        me.resetContinueMessage();
    };

    me.resetContinueMessage = function () {
        me.lastMessageBelongToChatUserId = 0;
        me.isNextMessageFromSameUser = false;
    };

    me.scrollTop = function () {
        ///var scrollTopTimeOutId = setTimeout(function () {
            me.$parentMainChatRoom.animate({ scrollTop: me.$mainChatRoom.height() + 2000 });
        //    clearTimeout(scrollTopTimeOutId);
        //}, 10);
    };

    me.randomColor = function () {
        var randomColor = '#' + Math.floor(Math.random()*16777215).toString(16);

        return randomColor;
    };

    me.playSound = function (filename) {
        /// <summary>Plays a notification sound</summary>
        /// <param FullName="fileFullName" type="String">The file path without extension</param>
        var $soundContainer = $("#soundContainer");
        if (!$soundContainer.length) {
            $soundContainer = $("<div>").attr("id", "soundContainer").appendTo($("body"));
        }

        $soundContainer.html('<audio autoplay="autoplay"><source src="' + filename + '.mp3" type="audio/mpeg" /><source src="' + filename + '.ogg" type="audio/ogg" /><embed hidden="true" autostart="true" loop="false" src="' + filename + '.mp3" /></audio>');
    };
}