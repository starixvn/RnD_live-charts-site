/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />

/*exported ChatClient*/
function ChatClient(chatUser, chatRoom) {
    'use strict';
    var me = this;

    me.chatUser = chatUser;
    me.chatRoom = chatRoom;
    me.currentUserList = null;
    me.currentMessageHistory = null;
    me.formatTimer = CURRENT_TIME_FORMAT;

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
            var messageTime = moment(chatMessage.CreateTime).zone(me.formatTimer.timeZone).format(me.formatTimer.format);
            me.$mainChatRoom.append('<li class="chat-message">'
                                    + '<div class="message-chat-name">' + chatMessage.ChatUser.ChatName + '</div>'
                                    + '<div class="message-content">'
                                        + '<div class="message-content-main">' + chatMessage.Content + '</div>'
                                        + '<div class="message-content-time">' + messageTime + '</div>'
                                    + '</div>'
                                + '</li>');
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
                    + '<div class="message-content">'
                        + '<div class="message-content-main ' + classMessage + '">'
                                + message
                        + '</div>'
                        + '<div class="message-content-time">' + moment().zone(me.formatTimer.timeZone).format(me.formatTimer.format) + '</div>'
                    + '</div>'
               + '</li>');
    }
}