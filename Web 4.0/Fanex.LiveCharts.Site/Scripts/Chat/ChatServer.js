/*global LIVECHARTS, LANGUAGE*/
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
    me.pastMinutes = 500;

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
        var currentMessage = me.$txtMessageInput.val();
        if (currentMessage !== '') {
            me.chatAdapter.server.sendMessage({
                Content: currentMessage,
                ChatUser: me.chatUser,
                ChatRoom: me.chatRoom
            });

            me.$txtMessageInput.val('');
        }
        else {
            LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, LANGUAGE.messageCannotEmpty);
        }
    };

    me.joinRoom = function () {
        me.chatAdapter.server.joinRoom(me.chatUser, me.chatRoom, me.chatUserInRoom);
        me.getChatMessageHistory();
    };

    me.getUserList = function () {
        me.chatAdapter.server.getUserList(me.chatRoom, me.updateListUser);
    };

    me.getChatMessageHistory = function () {
        me.chatAdapter.server.getChatMessageHistory(me.chatRoom.chatRoomId, null, me.chatClient.updateChatMessageHistory);
    };
}