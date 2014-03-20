/* File Created: March 20, 2014 */
/*exported ChatSignalRAdapter*/
function ChatSignalRAdapter() {
    'use strict';
    var me = this;

    me.init = function (chat, done) {
        // These are the methods that ARE CALLED BY THE SERVER.
        // Client functions should not call these functions.
        me.hub = $.connection.chatHub;
        me.hub.client.sendMessage = function (message) {
            chat.client.sendMessage(message);
        };

        me.hub.client.sendTypingSignal = function (otherUserId) {
            chat.client.sendTypingSignal(otherUserId);
        };

        me.hub.client.usersListChanged = function (usersList) {
            chat.client.usersListChanged(usersList);
        };

        if (!window.hubReady) {
            window.hubReady = $.connection.hub.start();
        }

        window.hubReady.done(function () {
            // function passed by ChatJS to the adapter to be called when the adapter initialization is completed
            done();
        });
    };


    // These are the methods that ARE CALLED BY THE CLIENT
        // Client functions should call these functions
    me.server =
    {

        sendMessage: function (otherUserId, messageText, clientGuid, done) {
            /// <summary>Sends a message to server</summary>
            /// <param FullName="otherUserId" type="Number">The id of the user to which the message is being sent</param>
            /// <param FullName="messageText" type="String">Message text</param>
            /// <param FullName="clientGuid" type="String">Message client guid. Each message must have a client id 
            /// in order for it to be recognized when it comes back from the server</param>
            /// <param FullName="done" type="Function">Function to be called when this method completes</param>
            me.hub.server.sendMessage(otherUserId, messageText, clientGuid).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        sendTypingSignal: function (otherUserId, done) {
            /// <summary>Sends a typing signal to the server</summary>
            /// <param FullName="otherUserId" type="Number">The id of the user to which the typing signal is being sent</param>
            /// <param FullName="done" type="Function">Function to be called when this method completes</param>
            me.hub.server.sendTypingSignal(otherUserId).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        getMessageHistory: function (otherUserId, done) {
            /// <summary>Gets message history from the server</summary>
            /// <param FullName="otherUserId" type="Number">The id of the user from which you want the history</param>
            /// <param FullName="done" type="Number">Function to be called when this method completes</param>
            me.hub.server.getMessageHistory(otherUserId).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        getUserInfo: function (otherUserId, done) {
            /// <summary>Gets information about the user</summary>
            /// <param FullName="otherUserId" type="Number">The id of the user from which you want the information</param>
            /// <param FullName="done" type="Function">FUnction to be called when this method completes</param>
            me.hub.server.getUserInfo(otherUserId).done(function (result) {
                if (done) {
                    done(result);
                }
            });
        },

        getUsersList: function (done) {
            /// <summary>Gets the list of the users in the current room</summary>
            /// <param FullName="otherUserId" type="Number">The id of the user from which you want the information</param>
            /// <param FullName="done" type="Function">FUnction to be called when this method completes</param>
            me.hub.server.getUsersList().done(function (result) {
                if (done) {
                    done(result);
                }
            });
        }
    };
}