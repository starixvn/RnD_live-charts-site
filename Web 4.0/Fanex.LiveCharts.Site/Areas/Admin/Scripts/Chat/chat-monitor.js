/* File Created: April 1, 2014 */
/*global CHAT_MONITOR_DEFAULT, LiveChartsAdmin*/
function ChatMonitor() {
    'use strict';
    var me = this;
    me.chatMonitorDefault = CHAT_MONITOR_DEFAULT;
    me.usersStatus = {};


    me.cacheDomElements = function () {
        me.$messageGrid = $('#messageGrid');


    };

    me.registerGridEvents = function () {
        me.$btnReload = $('#btnReload');
        me.$allRemoveChatUser = $('.remove-chat-user');
        me.$allRemoveChatMessage = $('.remove-chat-message');
        me.$allRecoverChatUser = $('.recover-chat-user');
        me.$allRecoverChatMessage = $('.recover-chat-message');
        me.$btnReload.unbind('click').click(me.initMessageGrid);
        me.$allRemoveChatUser.unbind('click').click(me.kickOutChatUser);
        me.$allRemoveChatMessage.unbind('click').click(me.removeChatMessage);
        me.$allRecoverChatUser.unbind('click').click(me.recoverChatUser);
        me.$allRecoverChatMessage.unbind('click').click(me.recoverChatMessage);
    };

    me.kickOutChatUser = function () {
        var $this = $(this);
        var currentChatUserId = $this.attr('data-chat-user-id');
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
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, 'Kicked out successfully!');
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    me.removeChatMessage = function () {
        var $this = $(this);
        var currentChatMessageId = $this.attr('data-chat-message-id');
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
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, 'Delete message successfully!');
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
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, 'Recover chat user successfully!');
                    me.initMessageGrid();
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
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, 'Recover chat message successfully!');
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

    me.updateUsersStatus = function (usersInRoom) {
        me.usersStatus = {};
        var userLength = usersInRoom.length;
        for (var i = 0; i < userLength; i++) {
            var currentUser = usersInRoom[i];
            me.usersStatus[currentUser.ChatUser.MemberId] = currentUser;
        }
    }

    me.exportExcel = function () {
        LiveChartsAdmin.downloadFile(me.chatMonitorDefault.exportExcelUrl, {
            matchId: me.chatMonitorDefault.matchId
        });
    };

    me.gridOptions = {
        columns: [
                   { HeaderText: 'Time (GMT+8)', Width: 150, Name: 'Time', CellAlign: 'center', FormatDateTime: 'DD MMM, YYYY hh:mm A' },
                   { HeaderText: 'Username', Width: 150, Name: 'UserName', CellAlign: 'center', CustomCellRender: function (cell) {
                       var userStatusClass = me.usersStatus[cell.Value.MemberId].KickedOut ? 'recover-action recover-chat-user' : 'remove-action remove-chat-user';
                       return '<div class="user-name-content">' +
                                    '<span class="' + userStatusClass + '" '
                                            + 'data-member-id="' + cell.Value.MemberId + '"'
                                            + ' data-chat-user-id="' + cell.Value.ChatUserId + '"'
                                        + '></span>'
                                    + cell.Value.MemberId
                            + '</div>';
                   }
                   },
                   { HeaderText: 'Chat Name', Width: 150, Name: 'ChatName', CellAlign: 'center' },
                   { HeaderText: 'Chat Message', Width: 490, Name: 'ChatMessage', CustomCellRender: function (cell) {
                       var messageClass = cell.Value.isDeleted ? 'recover-action recover-chat-message' : 'remove-action remove-chat-message';
                       return '<div class="message-container">'
                                    + '<span class="' + messageClass + '" data-chat-message-id="'
                                        + cell.Value.chatMessageId
                                    + '"></span>'
                                    + '<div class="message-content">'
                                        + cell.Value.content.replace(/\r\n/g, "<br />").replace(/\n/g, "<br />")
                                    + '</div>'
                            + '</div>';
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
                       Value: currentMessage.ChatUser
                   },
                   { Name: "ChatName", Value: currentMessage.ChatUser.ChatName },
                   {
                       Name: "ChatMessage",
                       Value:
                       {
                           content: currentMessage.Content,
                           chatMessageId: currentMessage.ChatMessageId,
                           isDeleted: currentMessage.IsDeleted
                       }
                   }
                ],
                CustomAttr: 'data-chat-message-id="' + currentMessage.ChatMessageId + '"'
            });
        }

        return bodyRows;
    };

    me.init = function () {
        me.cacheDomElements();
        me.initMessageGrid();
        //me.registerEvents();
    };
}


$(function () {
    'use strict';
    var chatMonitor = new ChatMonitor();
    chatMonitor.init();
});