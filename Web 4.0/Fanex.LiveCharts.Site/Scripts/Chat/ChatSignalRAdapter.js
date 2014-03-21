/* File Created: March 20, 2014 */
/*exported ChatSignalRAdapter*/
function ChatSignalRAdapter() {
    'use strict';
    var me = this;

    me.init = function (chat, done) {
        // These are the methods that ARE CALLED BY THE SERVER.
        // Client functions should not call these functions.
        me.hub = $.connection.chatHub;
        me.hub.client.addMessage = function (message) {
            chat.client.addMessage(message);
        };

        me.hub.client.sendTypingSignal = function (otherUserId) {
            chat.client.sendTypingSignal(otherUserId);
        };

        me.hub.client.usersListChanged = function (usersList) {
            chat.client.usersListChanged(usersList);
        };

        //me.hub.client.

        if (!window.hubReady) {
            window.hubReady = $.connection.hub.start();
        }

        window.hubReady.done(function () {
            // function passed by ChatJS to the adapter to be called when the adapter initialization is completed
            done();
        });
    };

    me.server =
    {

        sendMessage: function (chatMessage, chatRoom, done) {
            me.hub.server.sendMessage(chatMessage, chatRoom).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        sendTypingSignal: function (otherUserId, done) {
            me.hub.server.sendTypingSignal(otherUserId).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        getMessageHistory: function (otherUserId, done) {
            me.hub.server.getMessageHistory(otherUserId).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        getUserInfo: function (otherUserId, done) {
            me.hub.server.getUserInfo(otherUserId).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        getUserList: function (chatRoom, done) {
            me.hub.server.getUserList(chatRoom).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        joinRoom: function (chatUser, chatRoom, done) {
            me.hub.server.joinRoom(chatUser, chatRoom).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        }
    };
}