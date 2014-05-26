/* File Created: March 20, 2014 */
/*exported ChatSignalRAdapter*/
function ChatSignalRAdapter() {
    'use strict';
    var me = this;

    me.init = function (chatWindow, callBackFunctionAfterConnected) {
        // These are the methods that ARE CALLED BY THE SERVER.
        // Client functions should not call these functions.
        me.hub = $.connection.chatHub;
        me.hub.client.addMessage = chatWindow.chatClient.addMessage;
        me.hub.client.showTypingSignal = chatWindow.chatClient.showTypingSignal;
        me.hub.client.updateUserList = chatWindow.chatClient.updateUserList;
        me.hub.client.showAnnounceMessage = chatWindow.chatClient.showAnnounceMessage;
        me.hub.client.kickOutUserChat = chatWindow.chatClient.kickOutUserChat;
        me.hub.client.removeMessage = chatWindow.chatClient.removeMessage;
        me.hub.client.notifyUserLeftRoom = chatWindow.chatClient.notifyUserLeftRoom;
        me.hub.client.notifyUserJoinedRoom = chatWindow.chatClient.notifyUserJoinedRoom;
        me.hub.client.kickoutBecauseChatSameRoom = chatWindow.chatClient.kickoutBecauseChatSameRoom;
        me.hub.client.kickOutChatUser = chatWindow.chatClient.kickOutChatUser;
        me.hub.client.updateChatMessage = chatWindow.chatClient.updateChatMessage;
        me.hub.client.banChatUser = chatWindow.chatClient.kickOutChatUser;
        me.hub.client.updateAllChatMessageByChatUser = chatWindow.chatClient.updateAllChatMessageByChatUser;
        me.hub.client.updateMuteChatUser = chatWindow.chatClient.updateMuteChatUser;
        me.hub.client.updateMuteChatRoom = chatWindow.chatClient.updateMuteChatRoom;
        me.hub.client.disableChatRoom = chatWindow.chatClient.disableChatRoom;

        if (!window.hubReady) {
            window.hubReady = $.connection.hub.start();
        }

        window.hubReady.done(function () {
            // function passed by ChatJS to the adapter to be called when the adapter initialization is completed
            if (callBackFunctionAfterConnected) {
                callBackFunctionAfterConnected();
            }
        });
    };

    me.server =
    {
        sendMessage: function (chatMessage, done) {
            me.hub.server.sendMessage(chatMessage).done(function (result) {
                me.server.callBackDone(done, result);
            });
        },
        sendTypingSignal: function (otherUserId, done) {
            me.hub.server.sendTypingSignal(otherUserId).done(function (result) {
                me.server.callBackDone(done, result);
            });
        },
        getChatMessageHistory: function (chatRoomId, lastedTime, done) {
            me.hub.server.getChatMessageHistory(chatRoomId, lastedTime).done(function (result) {
                me.server.callBackDone(done, result);
            });
        },
        getUserInfo: function (otherUserId, done) {
            me.hub.server.getUserInfo(otherUserId).done(function (result) {
                me.server.callBackDone(done, result);
            });
        },
        getUserList: function (chatRoom, done) {
            me.hub.server.getUserList(chatRoom).done(function (result) {
                me.server.callBackDone(done, result);
            });
        },
        joinRoom: function (chatUser, chatRoom, done) {
            me.hub.server.joinRoom(chatUser, chatRoom).done(function (result) {
                me.server.callBackDone(done, result);
            });
        },
        callBackDone: function (doneFunction, result) {
            if (doneFunction) {
                doneFunction(result);
            }
        }
    };
}