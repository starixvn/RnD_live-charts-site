/* File Created: March 20, 2014 */
/// <reference path="../../Views/Chat/Index.cshtml" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="ChatSignalRAdapter.js" />
/// <reference path="Chat.js" />
/// <reference path="ChatServer.js" />

/*global LIVECHARTS, 
        CURRENT_CHAT_USER, 
        ChatClient, 
        CURRENT_CHAT_ROOM, 
        CURRENT_CHAT_USER_IN_ROOM, 
        ChatClient,
        ChatSignalRAdapter,
        ChatServer,
        CURRENT_TIME_FORMAT,
        moment,
        DEFAULT_OBJECT,
        LANGUAGE*/
function ChatWindow() {
    'use strict';
    var me = this;

    //// Properties
    me.chatUser = CURRENT_CHAT_USER;
    me.chatRoom = CURRENT_CHAT_ROOM;
    me.chatUserInRoom = CURRENT_CHAT_USER_IN_ROOM;
    me.chatAdapter = new ChatSignalRAdapter();
    me.chatClient = new ChatClient(me.chatUser, me.chatRoom);
    me.chatServer = new ChatServer(me.chatAdapter, me.chatClient, me.chatUser, me.chatRoom, me.chatUserInRoom);
    me.formatTimer = CURRENT_TIME_FORMAT;
    me.isMultiline = false;
    me.url = {
        getGetChatMessageByTimeUrl: LIVECHARTS.resolveUrl('Chat/GetGetChatMessageByTime'),
        getChatMessageByUserUrl: LIVECHARTS.resolveUrl('Chat/GetChatMessageByUser'),
        insertChatUserUrl: LIVECHARTS.resolveUrl('Chat/InsertChatUser')
    };

    //// Innitial all information
    me.init = function () {
        me.cacheDOMElements();
        me.chatClient.init();
        me.chatServer.init();
        me.checkValidUser();
        me.startTimer();
    };


    me.cacheDOMElements = function () {
        me.$soundContainer = $('#soundContainer');
        me.$mainChatRoom = $('#mainChatRoom');
        me.$txtMessageInput = $('#messageInput');
        me.$btnSubmitMessage = $('#submitMessage');
        me.$memberOnline = $('#memberOnline');
        me.$btnEmotiocons = $('#emobitonButton');
        me.$gmt8Timer = $('#gmt8Timer');
        me.$btnEmoticon = $('#btnEmoticon');
        me.$btnJoinRoom = $('#btnJoinRoom');
        me.$popupJoinRoom = $('#popupJoinRoom');
        me.$txtChatName = $('#txtChatName');
        me.$btnMultiline = $('#btnMultiline');
    };

    me.registerEvents = function () {
        me.$btnSubmitMessage.unbind('click').click(me.chatServer.sendMessage);
        me.$memberOnline.unbind('click').click(me.chatClient.showDetailUserList);
        me.$btnMultiline.unbind('click').click(me.multilineHandle);
        me.$btnMultiline.powerTip({
            placement: 'ne',
            smartPlacement: true
        });
        me.$txtMessageInput.unbind('keypress').keypress(me.onKeyPress);
        me.registerEmoticon();
        //// Send Spam Notify: implement later
        //// Auto Enter
    };

    me.checkValidUser = function () {
        if (me.chatUser.chatName === '') {
            me.$popupJoinRoom.modal({
                backdrop: 'static'
            });
            me.$btnJoinRoom.unbind('click').click(me.insertChatUser);
        }
        else {
            me.connectServer();
        }
    };

    me.onKeyPress = function (event) {
        if (event.which === 13) {
            event.preventDefault();
            me.$btnSubmitMessage.trigger('click');
        }
    };

    me.registerEmoticon = function () {
        var tooltip = $("<div class='emoticon-icons'>" +
                            ":) :-( ;) :D :O <br/>" +
                            "(6) (A) :'( :| :o) <br/>" +
                            "8-) :-* (M)<br/>" +
                        "</div>");
        tooltip.emoticons(DEFAULT_OBJECT.emoticonsFolder);
        tooltip.find('.emoticon-img').on('click', function () {
            var $this = $(this);
            me.$txtMessageInput.focus().val(me.$txtMessageInput.val() + ' ' + $this.attr('alt'));
        });
        me.$btnEmoticon.data('powertipjq', tooltip);
        me.$btnEmoticon.powerTip({
            placement: 'nw',
            mouseOnToPopup: true
        });
    };

    me.multilineHandle = function () {
        var $this = $(this),
            currentStatus = $this.attr('data-multiline-status'),
            currentTitle = '';

        if (currentStatus === 'off') {
            me.isMultiline = true;
            $this.addClass('enable-multiline')
                .attr('data-multiline-status', 'on');
            currentTitle = $this.attr('data-multiline-on');
            me.$txtMessageInput.unbind('keypress');
        }
        else {
            me.isMultiline = false;
            $this.removeClass('enable-multiline')
                .attr('data-multiline-status', 'off');
            currentTitle = $this.attr('data-multiline-off');
            me.$txtMessageInput.unbind('keypress').keypress(me.onKeyPress);
        }
        $this.data('powertip', currentTitle);
    };

    me.bindEventKeys = function () { };

    me.startTimer = function () {
        setInterval(function () {
            me.$gmt8Timer.html(moment().zone(me.formatTimer.timeZone).format(me.formatTimer.format));
        }, 1000);
    };

    //// Connect and Ajax

    me.insertChatUser = function () {
        var chatName = me.$txtChatName.val();
        if (chatName != null && chatName !== '') {
            LIVECHARTS.callAjax({
                url: me.url.insertChatUserUrl,
                type: 'POST',
                data: {
                    memberId: me.chatUser.memberId,
                    chatName: chatName
                },
                useBlockUI: false,
                done: function (result) {
                    if (result && result.CommandResult.IsSuccessful) {
                        me.chatUser.chatUserId = result.InsertedId;
                        me.chatUser.chatName = chatName;
                        me.connectServer();
                        me.$popupJoinRoom.modal('hide');
                    }
                    else {
                        LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, result.CommandResult.Message);
                    }
                }
            });
        }
        else {
            LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, LANGUAGE.chatNameCannotEmpty);
            me.$txtChatName.focus();
        }
    };

    me.connectServer = function () {
        me.chatAdapter.init(me, me.connectToServerDone);
    };

    me.connectToServerDone = function () {
        try { $.unblockUI(); } catch (e) { }
        me.registerEvents();
        me.chatServer.joinRoom();

    };
        
}


$(function () {
    'use strict';
    var chatWindow = new ChatWindow();
    chatWindow.init();
});