/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />
/*global moment, CURRENT_TIME_FORMAT, DEFAULT_OBJECT, swearWords*/
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
    me.isNextMessageFromSameUser = false;
    me.userList = {};

    me.cachedDomElement = function () {

        me.$mainChatRoom = $('#mainChatRoom');
        me.$parentMainChatRoom = me.$mainChatRoom.parent();
        me.$txtMessageInput = $('#messageInput');
        me.$btnSubmitMessage = $('#submitMessage');
        me.$memberOnline = $('#memberOnline');
    };
    me.init = function () {
        me.cachedDomElement();
    };

    me.addMessage = function (chatMessage, stopScrollTop) {
        if (typeof chatMessage === 'object') {
            me.isNextMessageFromSameUser = me.lastMessageBelongToChatUserId === chatMessage.ChatUser.ChatUserId;
            chatMessage = me.preProcessMessageContent(chatMessage);
            if (typeof me.userList[chatMessage.ChatUser.ChatUserId] === 'undefined') {
                me.userList[chatMessage.ChatUser.ChatUserId] = me.randomColor();
            }

            me.$mainChatRoom.append('<li class="chat-message"'
                                    + ' data-message-id="' + chatMessage.ChatMessageId + '"'
                                    + ' data-chat-user-id="' + chatMessage.ChatUser.ChatUserId + '">'
                                    + (me.isNextMessageFromSameUser ? '' : '<div class="message-chat-name" style="color:' + me.userList[chatMessage.ChatUser.ChatUserId] + ';">' + chatMessage.ChatUser.ChatName + ':</div>')
                                    + '<div class="message-content">'
                                        + '<div class="message-content-main">' + chatMessage.Content + '</div>'
                                        + '<div class="message-content-time">' + chatMessage.MessageTime + '</div>'
                                    + '</div>'
                                + '</li>');
            me.lastMessageBelongToChatUserId = chatMessage.ChatUser.ChatUserId;

            if (!stopScrollTop) {
                me.scrollTop();
            }
        }

    };

    me.preProcessMessageContent = function (chatMessage) {
        chatMessage.MessageTime = moment(chatMessage.CreateTime).zone(me.formatTimer.timeZone).format(me.formatTimer.format);
        chatMessage.Content = chatMessage.Content.replace(/\r\n/g, "<br />").replace(/\n/g, "<br />");
        //result.replace(/\r\n\r\n/g, "</p><p>").replace(/\n\n/g, "</p><p>");
        //// Emoticonize text and check bad word
        var tempDiv = $('<div>' + chatMessage.Content + '</div>');
        var chatMessageContent = tempDiv.emoticons(DEFAULT_OBJECT.emoticonsFolder).profanityFilter({
            customSwears: swearWords
        });
        chatMessage.Content = chatMessageContent.html();
        chatMessageContent = null;

        return chatMessage;
    };

    me.updateChatMessageHistory = function (chatMessages) {
        for (var index in chatMessages) {
            var currentMesage = chatMessages[index];
            me.addMessage(currentMesage, true);
        }

        me.scrollTop();
    };

    me.showTypingSignal = function () { };
    me.updateUserList = function (userList) {
        if (me.$memberOnline && userList) {
            me.$memberOnline.html(userList.length);
            me.currentUserList = userList;
            me.scrollTop();
        }

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
    me.showAnnounceMessage = function () { };
    me.kickOutUserChat = function (chatUser) {
        if(chatUser && chatUser.chatUserId === me.chatUser.chatUserId)
        {
            alert('User is kicked out!');
        }
    };
    me.removeMessage = function () { };
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
        me.$parentMainChatRoom.animate({ scrollTop: me.$mainChatRoom.height() + 2000 });
    };

    me.randomColor = function () {
        var randomColor = '#' + Math.random().toString(16).substr(2, 6);

        return randomColor;
    };
}