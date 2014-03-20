/* File Created: March 20, 2014 */
/// <reference path="ChatSignalRAdapter.js" />

/*global DEFAULT_VALUE, ChatSignalRAdapter */
/*exported Chat*/
function Chat() {
    'use strict';
    var me = this;

    me.chatUser = {
        memberId: DEFAULT_VALUE.MemberId,
        chatName: DEFAULT_VALUE.ChatName,
        isBanned: DEFAULT_VALUE.IsBanned
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