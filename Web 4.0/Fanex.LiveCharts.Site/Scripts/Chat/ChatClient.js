/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />

/*global DEFAULT_OBJECT*/
/*exported ChatClient*/
function ChatClient(chatUser, chatRoom) {
    'use strict';
    var me = this;

    me.chatUser = chatUser;
    me.chatRoom = chatRoom;

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
        me.$mainChatRoom.append('<li>' + chatMessage.Content + '</li>');
    };
    me.showTypingSignal = function () { };
    me.updateUserList = function (userList) {
        if (me.$memberOnline) {
            me.$memberOnline.html(userList.length);
        }
    }
    me.showAnnounceMessage = function () { };
    me.kickOutUserChat = function (chatUser) {
        if(chatUser && chatUser.chatUserId === me.chatUser.chatUserId)
        {
            alert('User is kicked out!');
        }
    };
    me.removeMessage = function () { };
    me.notifyUserLeftRoom = function (chatUserInRoom) {
        //if (chatUser != null && chatUser.ChatName) {
        //    me.$mainChatRoom.append('<li>' + chatUser.ChatName + ' has left.</li>');
        //}
        me.$mainChatRoom.append('<li>' + chatUserInRoom.ChatUserId + ' has left.</li>');
    };

    me.notifyUserJoinedRoom = function (chatUser) {
        if (chatUser && chatUser.ChatName) {
            me.$mainChatRoom.append('<li>' + chatUser.ChatName + ' joined room.</li>');
        }
    };
}