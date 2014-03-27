/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />
/*global moment, CURRENT_TIME_FORMAT*/
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

    me.cachedDomElement = function () {
        me.$mainChatRoom = $('#mainChatRoom');
        me.$txtMessageInput = $('#messageInput');
        me.$btnSubmitMessage = $('#submitMessage');
        me.$memberOnline = $('#memberOnline');
    };
    me.init = function () {
        me.cachedDomElement();
    };

    me.addMessage = function (chatMessage) {
        if (chatMessage) {
            me.isNextMessageFromSameUser = me.lastMessageBelongToChatUserId === chatMessage.ChatUser.ChatUserId;
            var messageTime = moment(chatMessage.CreateTime).zone(me.formatTimer.timeZone).format(me.formatTimer.format);
            //// Emoticonize text
            var tempDiv = $('<div>' + chatMessage.Content + '</div>');
            var chatMessageContent = tempDiv.emoticons(DEFAULT_OBJECT.emoticonsFolder);
            me.$mainChatRoom.append('<li class="chat-message">'
                                    + (me.isNextMessageFromSameUser ? '' : '<div class="message-chat-name">' + chatMessage.ChatUser.ChatName + ':</div>')
                                    + '<div class="message-content">'
                                        + '<div class="message-content-main">' + chatMessageContent.html() + '</div>'
                                        + '<div class="message-content-time">' + messageTime + '</div>'
                                    + '</div>'
                                + '</li>');
            me.lastMessageBelongToChatUserId = chatMessage.ChatUser.ChatUserId;
            chatMessageContent = null;
        }

    };

    me.updateChatMessageHistory = function (chatMessages) {
        for (var index in chatMessages) {
            var currentMesage = chatMessages[index];
            me.addMessage(currentMesage);
        }
    };

    me.showTypingSignal = function () { };
    me.updateUserList = function (userList) {
        if (me.$memberOnline && userList) {
            me.$memberOnline.html(userList.length);
            me.currentUserList = userList;
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
        //if (chatUser != null && chatUser.ChatName) {
        //    me.$mainChatRoom.append('<li>' + chatUser.ChatName + ' has left.</li>');
        //}

        //me.renderUserNotification(chatUser, ' joined room.');
        //me.$mainChatRoom.append('<li>' + chatUserInRoom.ChatUserId + ' has left.</li>');
        me.renderUserNotification(chatUserInRoom, ' has left.', false);
        me.updateUserList(currentChatUserList);
    };

    me.notifyUserJoinedRoom = function (chatUser, currentChatUserList) {
        if (chatUser && chatUser.ChatName) {
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
}