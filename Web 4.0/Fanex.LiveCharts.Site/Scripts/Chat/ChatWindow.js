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
    me.maxRowsInput = 12;
    me.url = {
        getGetChatMessageByTimeUrl: LIVECHARTS.resolveUrl('Chat/GetGetChatMessageByTime'),
        getChatMessageByUserUrl: LIVECHARTS.resolveUrl('Chat/GetChatMessageByUser'),
        insertChatUserUrl: LIVECHARTS.resolveUrl('Chat/InsertChatUser')
    };

    //// Innitial all information
    me.init = function () {
        me.chatUser.chatName = LIVECHARTS.htmlDecode(me.chatUser.chatName);
        me.cacheDOMElements();
        me.chatClient.init();
        me.chatServer.init();
        me.chatAdapter.init(me, me.signalRConnectDone);
    };

    me.signalRConnectDone = function () {
        me.checkValidUser();
    };

    me.initChatRoom = function () {
        me.startTimer();
        me.handleBrowserStatus();
        me.joinRoom();
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
        me.$btnCreateChatName = $('#btnCreateChatName');
        me.$popupCreateChatName = $('#popupCreateChatName');
        me.$txtChatName = $('#txtChatName');
        me.$btnMultiline = $('#btnMultiline');
        me.$btnAgreeTerms = $('#btnAgreeTerms');
        me.$btnJoinRoom = $('#btnJoinRoom');
        me.$popupTermsCondition = $('#popupTermsCondition');
        me.$popupChatUserList = $('#popupChatUserList');
        me.$userListContainer = $('#userListContainer');
        me.$btnLeftRoom = $('#btnLeftRoom');
        me.$popupWaitingJoinRoom = $('#popupWaitingJoinRoom');
    };

    me.registerEvents = function () {
        me.$btnSubmitMessage.unbind('click').click(me.chatServer.sendMessage);
        me.$memberOnline.unbind('click').click(me.showDetailUserList);
        me.$btnMultiline.unbind('click').click(me.multilineHandle);
        me.$btnMultiline.powerTip({ placement: 'ne', smartPlacement: true });
        me.$popupChatUserList.modal({ show: false });
        me.$txtMessageInput.unbind('keypress').keypress(me.onKeyPressForSingleLine);
        me.$btnLeftRoom.unbind('click').click(me.chatClient.leftRoom).show();
        me.registerEmoticon();
    };

    me.checkValidUser = function () {
        if (me.chatUser.chatName === '') {
            me.$popupCreateChatName.modal({
                backdrop: 'static'
            });
            me.$btnCreateChatName.unbind('click').click(me.insertChatUser);
            me.$txtChatName.unbind('keypress').keypress(me.onKeyPressForInsertingChatUser);
        }
        else {
            me.askJoinRoom();
        }
    };

    me.showDetailUserList = function () {
        me.$userListContainer.html(me.chatClient.getCurrentUserList());
        me.$popupChatUserList.modal('show');
    };

    me.askJoinRoom = function () {
        me.$popupWaitingJoinRoom.find('#waitingMessage').html(me.chatUser.chatName);
        me.$popupWaitingJoinRoom.modal({
            backdrop: 'static'
        });
        me.$btnJoinRoom.unbind('click').click(function () {
            me.$popupWaitingJoinRoom.modal('hide');
            if (me.chatUser.agreedTerms || me.chatRoom.termConditionDisabled) {
                me.initChatRoom();
            }
            else {
                if (me.$popupTermsCondition.length > 0) {
                    me.$popupTermsCondition.modal({
                        backdrop: 'static'
                    });
                    me.$btnAgreeTerms.unbind('click').click(me.updateAgreeTerms);
                }
            }
        });
    };

    me.updateAgreeTerms = function () {
        LIVECHARTS.callAjax({
            url: DEFAULT_OBJECT.updateAgreeTerms,
            type: 'POST',
            data: {
                chatUserId: me.chatUser.chatUserId
            },
            done: function (result) {
                if (result && result.IsSuccessful) {
                    me.joinRoom();
                    me.$popupTermsCondition.modal('hide');
                }
                else {
                    LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, result.Message);
                }
            }
        });
    };

    me.onKeyPressForSingleLine = function (event) {
        if ((event.ctrlKey && (event.which === 13)) || (event.which === 10)) {
            event.preventDefault();
            $(this).val($(this).val() + "\n");
            return;
        }

        if (event.which === 13) {
            event.preventDefault();
            me.$btnSubmitMessage.trigger('click');
        }
    };

    me.onKeyPressForMultipleLine = function (event) {
        if (event.which === 13) {
            var $this = $(this);
            var currentContent = $this.val();
            var lines = currentContent.split('\n');
            if (lines.length >= me.maxRowsInput)
            {
                event.preventDefault();
            }
        }
    };

    me.onKeyPressForInsertingChatUser = function (event) {
        if (event.which === 13) {
            event.preventDefault();
            me.insertChatUser();
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
            mouseOnToPopup: true,
            manual: true
        });
        me.$btnEmoticon.unbind('click').click(function () {
            $.powerTip.show(me.$btnEmoticon);
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
            me.$txtMessageInput.unbind('keypress').keypress(me.onKeyPressForMultipleLine);
        }
        else {
            me.isMultiline = false;
            $this.removeClass('enable-multiline')
                .attr('data-multiline-status', 'off');
            currentTitle = $this.attr('data-multiline-off');
            me.$txtMessageInput.unbind('keypress').keypress(me.onKeyPressForSingleLine);
        }
        $this.data('powertip', currentTitle);
    };

    me.startTimer = function () {
        setInterval(function () {
            me.$gmt8Timer.html(moment().zone(me.formatTimer.timeZone).format(me.formatTimer.format));
        }, 1000);
    };

    //// Connect and Ajax

    me.insertChatUser = function () {
        var chatName = me.$txtChatName.val();
        if (me.validateChatName(chatName)) {
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
                        me.$popupCreateChatName.modal('hide');
                        me.chatUser.chatUserId = result.InsertedId;
                        me.chatUser.chatName = LIVECHARTS.htmlEncode(chatName);
                        me.askJoinRoom(true);
                    }
                    else {
                        LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, result.CommandResult.Message);
                    }
                }
            });
        }
    };

    me.validateChatName = function (chatName) {
        if (!chatName || /^\s*$/.test(chatName)) {
            LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, LANGUAGE.chatNameCannotEmpty);
            me.$txtChatName.focus();
            return false;
        }
        else if (chatName.length > 50) {
            LIVECHARTS.alertMessage(LIVECHARTS.messageType.error, LANGUAGE.maxLengthChatName);
            return;
        }

        return true;
    };

    me.joinRoom = function () {
        me.chatClient.joinRoom();
        me.registerEvents();
        me.chatServer.joinRoom();
    };

    me.handleBrowserStatus = function () {
        function onchange(evt) {
            var v = 'visible',
                h = 'hidden',
                evtMap = {
                    focus: v,
                    focusin: v,
                    pageshow: v,
                    blur: h,
                    focusout: h,
                    pagehide: h
                },
                currentStatus;

            evt = evt || window.event;
            if (evt.type in evtMap) {
                currentStatus = evtMap[evt.type];
            }
            else {
                currentStatus = document[hidden] ? 'hidden' : 'visible';
            }

            me.chatClient.isBrowserHidden = currentStatus === 'hidden';
        }

        var hidden = "hidden";

        // Standards:
        if (hidden in document) {
            document.addEventListener("visibilitychange", onchange);
        }
        else if ((hidden = "mozHidden") in document) {
            document.addEventListener("mozvisibilitychange", onchange);
        }
        else if ((hidden = "webkitHidden") in document) {
            document.addEventListener("webkitvisibilitychange", onchange);
        }
        else if ((hidden = "msHidden") in document) {
            document.addEventListener("msvisibilitychange", onchange);
        }
        // IE 9 and lower:
        else if ('onfocusin' in document) {
            document.onfocusin = document.onfocusout = onchange;
        }
        // All others:
        else {
            window.onpageshow = window.onpagehide
            = window.onfocus = window.onblur = onchange;
        }
    };
}


$(function () {
    'use strict';
    var chatWindow = new ChatWindow();
    chatWindow.init();
});