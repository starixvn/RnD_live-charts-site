/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />
/*global moment, CURRENT_TIME_FORMAT, DEFAULT_OBJECT, swearWords, LIVECHARTS, LANGUAGE*/
/*exported ChatClient*/
function ChatClient(chatUser, chatRoom) {
    'use strict';
    var me = this;

    me.chatUser = chatUser;
    me.chatRoom = chatRoom;
    me.currentUserInRoomList = null;
    me.currentMessageHistory = null;
    me.formatTimer = CURRENT_TIME_FORMAT;
    me.lastMessageBelongToChatUserId = 0;
    me.firstMessageBelongToChatUserId = 0;
    me.isNextMessageFromSameUser = false;
    me.numberOfHistoryMessagesShowing = 12;
    me.numberOfHistoryMessagesPaging = 100;
    me.numberOfGetMessageHistoryFromDb = 1000;
    me.currentHistoryTime = null;
    me.stillHaveHistoryMessage = false;
    me.isBrowserHidden = false;
    me.historyMessages = [];
    me.userList = {};
    me.userColor = '#f00';
    

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

    me.joinRoom = function () {
        me.userList[me.chatUser.chatUserId] = me.userColor;
        me.getChatMessageHistory();
    };

    me.addMessage = function (chatMessage, stopScrollTop) {
        me.isNextMessageFromSameUser = me.lastMessageBelongToChatUserId === chatMessage.ChatUser.ChatUserId;
        chatMessage = me.preProcessMessage(chatMessage);
        if (typeof me.userList[chatMessage.ChatUser.ChatUserId] === 'undefined') {
            me.userList[chatMessage.ChatUser.ChatUserId] = me.randomColor();
        }

        var messageText = me.buildMessage(chatMessage, me.isNextMessageFromSameUser);

        me.lastMessageBelongToChatUserId = chatMessage.IsDeleted ? Math.floor((Math.random()*10000)+1) : chatMessage.ChatUser.ChatUserId;

        me.$mainChatRoom.append(messageText);
        if (!stopScrollTop) {
            me.scrollTop();
        }

        if (me.isBrowserHidden) {
            me.playSound(DEFAULT_OBJECT.notificationSound);
        }
    };

    me.prependHistoryMessage = function (chatMessage, chatPreviousUserId) {
            me.isPreviousMessageFromSameUser = chatPreviousUserId === chatMessage.ChatUser.ChatUserId;
            chatMessage = me.preProcessMessage(chatMessage);
            if (typeof me.userList[chatMessage.ChatUser.ChatUserId] === 'undefined') {
                me.userList[chatMessage.ChatUser.ChatUserId] = me.randomColor();
            }
            var messageText = me.buildMessage(chatMessage, me.isPreviousMessageFromSameUser);
            me.$mainChatRoom.prepend(messageText);
    };

    me.buildMessage = function (chatMessage, isNextMessageFromSameUser) {
        return '<li class="chat-message"'
                    + ' data-message-id="' + chatMessage.ChatMessageId + '"'
                    + ' data-chat-user-id="' + chatMessage.ChatUser.ChatUserId + '"'
                    + ( chatMessage.IsDeleted ? ' style="display:none;"' : '')
                    + '>'
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
        chatMessage.MessageTime =  me.formatTime(moment(chatMessage.CreateTime));
        chatMessage.Content = chatMessage.Content.replace(/\r\n/g, "<br />").replace(/\n/g, "<br />");
        var tempDiv = $('<div>' + chatMessage.Content + '</div>'),
            chatMessageContent = tempDiv.emoticons(DEFAULT_OBJECT.emoticonsFolder).profanityFilter({
                customSwears: swearWords
            });
        chatMessage.Content = chatMessageContent.html();
        chatMessageContent = null;

        return chatMessage;
    };

    me.getChatMessageHistory = function () {
        LIVECHARTS.callAjax({
            url: DEFAULT_OBJECT.getHistoryMessageUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatRoom.chatRoomId,
                latestTime: me.currentHistoryTime
            },
            useBlockUI: false,
            done: function (chatMessages) {
                if (chatMessages) {
                    me.updateChatMessageHistory(chatMessages);
                }
            }
        });
    };

    me.updateChatMessageHistory = function (chatMessages) {
        me.historyMessages = chatMessages;
        var messageLength = me.historyMessages.length,
            lastMessageArray = [];

        me.stillHaveHistoryMessage = messageLength === me.numberOfGetMessageHistoryFromDb;


        if (me.numberOfHistoryMessagesShowing > messageLength) {
            lastMessageArray = me.historyMessages;
        }
        else {
            lastMessageArray = me.historyMessages.slice(messageLength - me.numberOfHistoryMessagesShowing, messageLength);
            me.$btnShowHistoryMessage.unbind('click').click(me.showPreviousMessages).show();
        }

        var lastMessageLength = lastMessageArray.length;
        for (var i = 0; i < lastMessageLength; i++) {
            var currentMesage = lastMessageArray[i];
            if (i === 0) {
                me.firstMessageBelongToChatUserId = currentMesage.ChatUser.ChatUserId;
            }
            me.addMessage(currentMesage, true);
        }

        me.scrollTop();
        if (!DEFAULT_OBJECT.disabledWelcomeMessage) {
            me.lastMessageBelongToChatUserId = 0;
            me.appendUserNotification(DEFAULT_OBJECT.welcomeMessage, 'welcome-message');
        }
    };

    me.updateUserList = function (userInRoomList) {
        if (me.$memberOnline && userInRoomList) {
            me.$memberOnline.html(userInRoomList.length);
            me.currentUserInRoomList = userInRoomList;
            me.scrollTop();
        }

    };

    me.showPreviousMessages = function () {
        var currentShowingMessageIndex = me.numberOfHistoryMessagesShowing,
            currentMessageLength = me.historyMessages.length,
            fromIndex = 0,
            toIndex = currentMessageLength - currentShowingMessageIndex - 1;
        if (currentShowingMessageIndex + me.numberOfHistoryMessagesPaging < currentMessageLength) {
            fromIndex = toIndex - me.numberOfHistoryMessagesPaging + 1;
        }

        for (var i = toIndex; i >= fromIndex; i--) {
            var previousUserId = 0;
            if (i > 0 && i !== fromIndex) {
                previousUserId = me.historyMessages[i - 1].ChatUser.ChatUserId;
            }
            me.prependHistoryMessage(me.historyMessages[i], previousUserId);
            me.currentHistoryTime = me.historyMessages[i].CreateTime;
        }

        if (fromIndex === 0) {
            if (me.stillHaveHistoryMessage) {
                me.getChatMessageHistory();
            }
            else {
                me.$btnShowHistoryMessage.hide();
            }
        }

        me.numberOfHistoryMessagesShowing += me.numberOfHistoryMessagesPaging;
    };

    me.getCurrentUserList = function () {
        var userChatNameList = [];
        if (me.currentUserInRoomList && me.currentUserInRoomList.length > 0) {
            var userRoomLength = me.currentUserInRoomList.length;
            
            for (var i = 0; i < userRoomLength; i++) {
                var chatUserInRoom = me.currentUserInRoomList[i];
                userChatNameList.push('<span style="color:' + me.userList[chatUserInRoom.ChatUser.ChatUserId] + ';">'
                                            + chatUserInRoom.ChatUser.ChatName
                                       + '</span>');
            }
        }

        return userChatNameList.join(', ');
    };

    me.kickOutChatUser = function (chatUserId) {
        if (chatUserId === me.chatUser.chatUserId) {
            location.href = DEFAULT_OBJECT.kickedOutPage;
        }
    };

    me.leftRoom = function () {
        window.location.href = window.location.href;
    };

    me.updateChatMessage = function (chatMessageId, isDeleted) {
        var deleteMessageElement = me.$mainChatRoom.find('.chat-message[data-message-id=' + chatMessageId + ']');
        if (deleteMessageElement.length > 0) {
            if (isDeleted) {
                deleteMessageElement.hide();
            }
            else {
                deleteMessageElement.show();
            }
        }
    };

    me.updateAllChatMessageByChatUser = function (chatUserId, isDeleted) {
        var deleteMessageElement = me.$mainChatRoom.find('.chat-message[data-chat-user-id=' + chatUserId + ']');
        if (deleteMessageElement.length > 0) {
            if (isDeleted) {
                deleteMessageElement.hide();
            }
            else {
                deleteMessageElement.show();
            }
        }
    };

    me.notifyUserLeftRoom = function (chatUserInRoom, currentChatUserList) {
        if (chatUserInRoom.ChatUser.ChatUserId !== me.chatUser.chatUserId) {
            me.renderUserNotification(chatUserInRoom.ChatUser, LANGUAGE.hasLeft, false);
        }
        me.updateUserList(currentChatUserList);
    };

    me.notifyUserJoinedRoom = function (chatUser, currentChatUserList) {
        if (chatUser && chatUser.ChatName && chatUser.ChatUserId !== me.chatUser.chatUserId) {
            me.renderUserNotification(chatUser, LANGUAGE.joinedRoom, true);
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
                    + '<span class="notification-message"> ' + messageText + '</span>',
            classMessage = isJoin ? 'joined-message' : 'left-message';

        me.appendUserNotification(message, classMessage);
        me.resetContinueMessage();
    };

    me.appendUserNotification = function (htmlMessage, messageClass) {
        me.$mainChatRoom.append('<li class="chat-message">'
                    + '<div class="message-content notification-content">'
                        + '<div class="message-content-main ' + messageClass + '">'
                                + htmlMessage
                        + '</div>'
                        + '<div class="message-content-time">' + me.formatTime(moment()) + '</div>'
                    + '</div>'
               + '</li>');
    };

    me.resetContinueMessage = function () {
        me.lastMessageBelongToChatUserId = 0;
        me.isNextMessageFromSameUser = false;
    };

    me.scrollTop = function () {
        me.$parentMainChatRoom.animate({ scrollTop: me.$mainChatRoom.height() + 2000 });
    };

    me.randomColor = function () {
        var randomColor = '';
        for (var i = 0; i < 6; i++) {
            randomColor += '' + parseInt(Math.random() * 9);
        }

        return '#' + randomColor;
    };

    me.playSound = function (filename) {
        /// <summary>Plays a notification sound</summary>
        /// <param FullName="fileFullName" type="String">The file path without extension</param>
        var $soundContainer = $("#soundContainer");
        if (!$soundContainer.length) {
            $soundContainer = $('<div id="soundContainer" style="display:none;" />').appendTo($("body"));
        }

        $soundContainer.html('<audio autoplay="autoplay"><source src="' + filename + '.mp3" type="audio/mpeg" />'
                            + '<source src="' + filename + '.ogg" type="audio/ogg" />'
                            +'<embed hidden="true" autostart="true" loop="false" src="' + filename + '.mp3" /></audio>');
    };

    me.formatTime = function (momentTime) {
        return momentTime.zone(me.formatTimer.timeZone).format(me.formatTimer.format);
    };
}