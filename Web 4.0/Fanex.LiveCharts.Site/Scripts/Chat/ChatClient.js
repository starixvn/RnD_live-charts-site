/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />

/*global DEFAULT_OBJECT*/
/*exported ChatClient*/
function ChatClient() {
    'use strict';
    var me = this;

    me.chatUser = {
        memberId: DEFAULT_OBJECT.memberId,
        chatName: DEFAULT_OBJECT.chatName,
        isBanned: DEFAULT_OBJECT.isBanned
    };

    me.init = function () {
        
    };

    me.doneConnectToServer = function(){
    };

    me.checkAlreadyHasChatName = function () {

    };

    me.getCurrenRoomMember = function () {

    };

    me.keyHandler = function () { 
        
    };
}