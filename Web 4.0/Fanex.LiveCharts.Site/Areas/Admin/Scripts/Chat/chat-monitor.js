/* File Created: April 1, 2014 */
/*global CHAT_MONITOR_DEFAULT, LiveChartsAdmin, LANGUAGE*/
function ChatMonitor() {
    'use strict';
    var me = this;
    me.chatMonitorDefault = CHAT_MONITOR_DEFAULT;
    me.usersStatus = {};


    me.cacheDomElements = function () {
        me.$messageGrid = $('#messageGrid');
        me.$menuEditChatUser = $('#menuEditChatUser');
        me.$menuEditChatMessage = $('#menuEditChatMessage');

    };

    me.registerGridEvents = function () {
        me.$btnReload = $('#btnReload');
        me.$allOpenMenuRemoveUser = $('#messageGrid .open-menu-remove-user');
        me.$allKickOutChatUser = $('#messageGrid .kickout-chat-user');
        me.$allBanChatUser = $('#messageGrid .ban-chat-user');
        me.$allRemoveOneMessage = $('#messageGrid .remove-one-message');
        me.$allRemoveAllMessages = $('#messageGrid .remove-all-messages');
        me.$allOpenMenuRemoveMessage = $('#messageGrid .open-menu-remove-message');
        me.$allRecoverChatUser = $('#messageGrid .recover-chat-user');
        me.$allRecoverChatMessage = $('#messageGrid .recover-chat-message');
        me.$allMenuActions = $('#messageGrid .menu-action');
        me.$btnReload.unbind('click').click(me.initMessageGrid);
        //// Open Menu
        //me.$allOpenMenuRemoveUser.unbind('click').click(me.showMenuEdit);
        //me.$allOpenMenuRemoveMessage.unbind('click').click(me.showMenuEdit);
        me.showMenuEdit();
        //// User actionss
        me.$allRecoverChatUser.unbind('click').click(me.recoverChatUser);
//        me.$allKickOutChatUser.unbind('click').click(me.kickOutChatUser);
//        me.$allBanChatUser.unbind('click').click(me.banChatUser);

        //// Chat Message actions
        me.$allRecoverChatMessage.unbind('click').click(me.recoverChatMessage);
//        me.$allRemoveOneMessage.unbind('click').click(me.removeChatMessage);
//        me.$allRemoveAllMessages.unbind('click').click(me.removeAllChatMessageByChatUser);

    };

    me.showMenuEdit = function () {
        $.contextMenu({
            selector: '#messageGrid .open-menu-remove-user',
            trigger: 'left',
            autoHide: true,
            callback: function (key, options) {
                var $spanData = $(this);
                switch (key) {
                    case 'kickout': { me.kickOutChatUser($spanData.attr('data-chat-user-id')); };
                    case 'ban': { me.banChatUser($spanData.attr('data-chat-user-id'), true); }
                }
            },
            items: {
                'kickout': { name: "Kick out this user", icon: "delete" },
                'ban': { name: "Ban this user", icon: "delete" }
            }
            //            ,events: {
            //                show: function (opt) {
            //                    var $this = this;
            //                    $.contextMenu.setInputValues(opt, { name: "11111" });
            //                }
            //            }
        });

        $.contextMenu({
            selector: '#messageGrid .open-menu-remove-message',
            trigger: 'left',
            autoHide: true,
            callback: function (key, options) {
                var $spanData = $(this);
                switch (key) {
                    case 'removeThisMessage': { me.removeChatMessage($spanData.attr('data-chat-message-id')); };
                    case 'removeAllMessageOfChatUser': { me.removeAllChatMessageByChatUser($spanData.attr('data-chat-user-id')); }
                }
            },
            items: {
                "removeThisMessage": { name: "Remove This Message", icon: "delete" },
                "removeAllMessageOfChatUser": { name: "Remove All Messages From This User ", icon: "delete" }
            }
        });
        //        var $this = $(this);
        //        var menuAction = $this.next();
        //        menuAction.show();
        //        menuAction.css(
        //            {
        //                marginLeft: '-' + menuAction.find('.actions').width() + 'px'
        //            });
    };

    ///// Chat User Action
    me.banChatUser = function (currentChatUserId, isBanned) {
        //banChatUserUrl
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.banChatUser,
            type: 'POST',
            data: {
                chatUserId: currentChatUserId
            },
            done: function (banViewModel) {
                if (banViewModel.CommandResult.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.banChatUserSuccess);
//                    if (kickOutViewModel.NeedToSendRequestKickOut) {
//                        me.kickOutChatUserOnMemberSite(kickOutViewModel);

//                    }
                    me.initMessageGrid();
                }
            }
        });
    };
    me.kickOutChatUser = function (currentChatUserId) {
//        var $spanData = $(this).parent().parent().prev();
//        var currentChatUserId = $spanData.attr('data-chat-user-id');

        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.kickOutChatUserUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId,
                chatUserId: currentChatUserId
            },
            done: function (kickOutViewModel) {
                if (kickOutViewModel.CommandResult.IsSuccessful) {
                    if (kickOutViewModel.NeedToSendRequestKickOut) {
                        me.kickOutChatUserOnMemberSite(kickOutViewModel);

                    }
                    me.initMessageGrid();
                }
            }
        });
    };
    me.kickOutChatUserOnMemberSite = function (kickOutViewModel) {

        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.memberChartSiteUrl + '/Chat/KickOutChatUser',
            type: 'POST',
            data: {
                connectionId: kickOutViewModel.ConnectionId,
                chatUserId: kickOutViewModel.ChatUserId,
                tokenName: kickOutViewModel.TokenName,
                tokenKey: kickOutViewModel.TokenKey
            },
            done: function (result) {
                if (result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.kickedOutChatUserSuccess);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };
    me.recoverChatUser = function () {
        var $this = $(this);
        var currentChatUserId = $this.attr('data-chat-user-id');
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.recoverChatUserUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId,
                chatUserId: currentChatUserId
            },
            done: function (result) {
                if (result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.recoverChatUser);
                    me.initMessageGrid();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    ///// Chat Message Action
    me.removeChatMessage = function (currentChatMessageId) {
//        var $spanData = $(this).parent().parent().prev();
//        var currentChatMessageId = $spanData.attr('data-chat-message-id');
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.deleteChatMessageUrl,
            type: 'POST',
            data: {
                chatRoomName: me.chatMonitorDefault.chatRoomName,
                chatMessageId: currentChatMessageId
            },
            done: function (deleteMessageViewModel) {
                if (deleteMessageViewModel.CommandResult.IsSuccessful) {
                    me.removeChatMessageOnMemberSite(deleteMessageViewModel);
                    me.initMessageGrid();
                }
            }
        });
    };
    me.removeAllChatMessageByChatUser = function (currentChatUserId) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.deleteAllChatMessageByChatUserUrl,
            type: 'POST',
            data: {
                chatRoomName: me.chatMonitorDefault.chatRoomName,
                chatUserId: currentChatUserId
            },
            done: function (deleteMessageViewModel) {
                if (deleteMessageViewModel.CommandResult.IsSuccessful) {
                    //me.removeChatMessageOnMemberSite(deleteMessageViewModel);
                    me.initMessageGrid();
                }
            }
        });
    };
    me.removeChatMessageOnMemberSite = function (deleteMessageViewModel) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.memberChartSiteUrl + '/Chat/DeleteChatMessage',
            type: 'POST',
            data: {
                chatMessageId: deleteMessageViewModel.ChatMessageId,
                chatRoomName: deleteMessageViewModel.ChatRoomName,
                tokenName: deleteMessageViewModel.TokenName,
                tokenKey: deleteMessageViewModel.TokenKey
            },
            done: function (result) {
                if (result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.deleteMessageSuccess);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };
    me.recoverChatMessage = function () {
        var $this = $(this);
        var currentChatMessageId = $this.attr('data-chat-message-id');
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.recoverChatMessageUrl,
            type: 'POST',
            data: {
                //chatRoomName: me.chatMonitorDefault.chatRoomName,
                chatMessageId: currentChatMessageId
            },
            done: function (result) {
                if (result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.recoverChatSuccess);
                    me.initMessageGrid();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    me.initMessageGrid = function () {
        me.bodyRows = [];
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.getChatMessagesUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId
            },
            done: function (result) {
                me.updateUsersStatus(result.ChatUsers);
                me.gridOptions.bodyRows = me.buildGridBody(result.ChatMessages);
                me.$messageGrid.fanexGrid(me.gridOptions);
                me.registerGridEvents();
            }
        });
    };
    me.gridOptions = {
        columns: [
                   { HeaderText: LANGUAGE.timeGTM8, Width: 150, Name: 'Time', CellAlign: 'center', FormatDateTime: 'DD MMM, YYYY hh:mm A' },
                   { HeaderText: LANGUAGE.username, Width: 150, Name: 'UserName', CellAlign: 'center', CellCssClass: 'user-name-content', MergeHeaderColNumber: 2 },
                   {
                       HeaderText: '', Width: 40, Name: 'ChatNameAction', CellAlign: 'center', CustomCellRender: function (cell) {
                           var kickedOut = me.usersStatus[cell.Value.MemberId].KickedOut;
                           var userStatusClass = kickedOut ? 'recover-action recover-chat-user' : 'edit-action open-menu-remove-user';
                           var iconHtml = '<span class="' + userStatusClass + '"'
                                            + ' data-member-id="' + cell.Value.MemberId + '"'
                                            + ' data-chat-user-id="' + cell.Value.ChatUserId + '"'
                                            + '></span>';
//                           if (!kickedOut) {
//                               iconHtml += '<div style="display: none;" class="menu-action"> <ul class="actions" '
//                                                        + '> '
//                                                        + '<li class="kickout-chat-user">Kick Out <span class="action-user">' + cell.Value.MemberId + '</span></li> '
//                                                        + '<li class="ban-chat-user">Ban <span class="action-user">' + cell.Value.MemberId + '</span></li> '
//                                                        + '</ul> '
//                                                + '</div>';
//                           }
                           return iconHtml;
                       }
                   },
                   { HeaderText: LANGUAGE.chatName, Width: 150, Name: 'ChatName', CellAlign: 'center' },
                   { HeaderText: LANGUAGE.chatMessage, Width: 490, Name: 'ChatMessage', MergeHeaderColNumber: 2, CellCssClass: 'message-content' },
                   {
                       HeaderText: '', Width: 40, Name: 'ChatMessageAction', CellAlign: 'center', CustomCellRender: function (cell) {

                           var messageClass = cell.Value.isDeleted ? 'recover-action recover-chat-message' : 'edit-action open-menu-remove-message';
                           var iconHtml = '<span class="' + messageClass + '"'
                                                        + ' data-chat-message-id="' + cell.Value.chatMessageId + '"'
                                                        + ' data-chat-user-id="' + cell.Value.chatUserId + '"'
                                            + '></span>';
//                           if (!cell.Value.isDeleted) {
//                               iconHtml += '<div style="display: none;" class="menu-action"> \
//                                            <ul class="actions"> '
//                                                        + '<li class="remove-one-message">Delete this message</li> '
//                                                        + '<li class="remove-all-messages">Delete all message from <span class="action-user">' + cell.Value.memberName + '</span></li> '
//                                                        + '</ul> '
//                                                + '</div>';
//                           }
                           return iconHtml;
                       }
                   }
        ],
        gridExpandHeight: 125,
        showPager: true,
        pagerOption: {
            itemsPerPage: 50,
            currentPage: 0,
            itemsPerPageArray: [50, 100, 500],
            showDetail: true,
            afterPagedFunction: me.registerGridEvents
        },
        showTitle: true,
        titleInfo:
            {
                text: me.chatMonitorDefault.gridTitle,
                showExcelButton: true,
                exportExcelFunction: me.exportExcel,
                customButtons: '<input type="button" id="btnReload" class="reload-button">',
                showButtonsWhenNoData: false
            },
        timeZone: '+08:00'
    };
    me.buildGridBody = function (chatMessages) {
        var messagesLength = chatMessages.length;
        var bodyRows = [];
        for (var i = 0; i < messagesLength; i++) {
            var currentMessage = chatMessages[i];
            bodyRows.push({
                Columns: [
                   { Name: "Time", Value: currentMessage.CreateTime },
                   {
                       Name: "UserName",
                       Value: currentMessage.ChatUser.MemberId
                   },
                   { Name: "ChatNameAction", Value: currentMessage.ChatUser },
                   { Name: "ChatName", Value: currentMessage.ChatUser.ChatName },
                   {
                       Name: "ChatMessage",
                       Value: currentMessage.Content.replace(/\r\n/g, "<br />").replace(/\n/g, "<br />")
                   },
                   {
                       Name: 'ChatMessageAction', Value: {
                           chatMessageId: currentMessage.ChatMessageId,
                           isDeleted: currentMessage.IsDeleted,
                           chatUserId: currentMessage.ChatUser.ChatUserId,
                           memberName: currentMessage.ChatUser.ChatName
                       }
                   }
                ],
                CustomAttr: 'data-chat-message-id="' + currentMessage.ChatMessageId + '"'
            });
        }

        return bodyRows;
    };
    me.updateUsersStatus = function (usersInRoom) {
        me.usersStatus = {};
        var userLength = usersInRoom.length;
        for (var i = 0; i < userLength; i++) {
            var currentUser = usersInRoom[i];
            me.usersStatus[currentUser.ChatUser.MemberId] = currentUser;
        }
    };

    me.exportExcel = function () {
        LiveChartsAdmin.downloadFile(me.chatMonitorDefault.exportExcelUrl, {
            matchId: me.chatMonitorDefault.matchId
        });
    };
    me.init = function () {
        me.cacheDomElements();
        me.initMessageGrid();
        $(document).delegate('html', 'click', function (e, isInsideCall) {
            var $target = $(e.target);
            if (!$target.hasClass('edit-action')) {
                me.$allMenuActions.hide();
            }
        });
        //me.registerEvents();
    };
}


$(function () {
    'use strict';
    var chatMonitor = new ChatMonitor();
    chatMonitor.init();
});