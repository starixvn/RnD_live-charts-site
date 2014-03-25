/*exported ChatServer*/

function ChatServer(chatAdapter, chatClient, chatUser, chatRoom, chatUserInRoom )
{
    'use strict';
    var me = this;

    me.chatAdapter = chatAdapter;
    me.chatRoom = chatRoom;
    me.chatUser = chatUser;
    me.chatClient = chatClient;
    me.chatUserInRoom = chatUserInRoom;

    me.cachedDomElements = function () {
        me.$mainChatRoom = $('#mainChatRoom');
        me.$txtMessageInput = $('#messageInput');
        me.$btnSubmitMessage = $('#submitMessage');
        me.$memberOnline = $('#memberOnline');
    };

    me.init = function () {
        me.cachedDomElements();
    };

    me.sendMessage = function () {
        me.chatAdapter.server.sendMessage({
            Content: me.$txtMessageInput.val(),
            ChatUser: me.chatUser,
            ChatRoom: me.chatRoom
        });

        me.$txtMessageInput.val('');
    };

    me.joinRoom = function () {
        me.chatAdapter.server.joinRoom(me.chatUser, me.chatRoom, me.chatUserInRoom);
    };

    me.getUserList = function () {
        me.chatAdapter.server.getUserList(me.chatRoom, me.updateListUser);
    };

}