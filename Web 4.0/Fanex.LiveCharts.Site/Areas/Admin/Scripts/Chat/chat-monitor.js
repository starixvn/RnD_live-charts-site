/* File Created: April 1, 2014 */
/*global CHAT_MONITOR_DEFAULT, LiveChartsAdmin, LANGUAGE*/
function ChatMonitor() {
    'use strict';
    var me = this;
    me.chatMonitorDefault = CHAT_MONITOR_DEFAULT;
    me.usersStatus = {};
    me.createDllSearch = false;
    me.usersAndMessageCached = {};


    me.cacheDomElements = function () {
        me.$messageGrid = $('#messageGrid');
        me.$menuEditChatUser = $('#menuEditChatUser');
        me.$menuEditChatMessage = $('#menuEditChatMessage');
        me.$btnSearchChatMessage = $('#btnSearchChatMessage');
        me.$ddlMemberName = $('#ddlMemberName');
        me.$ddlChatName = $('#ddlChatName');
        me.$txtMessageSearch = $('#txtMessageSearch');
    };
    me.registerEvents = function () {
        me.$btnSearchChatMessage.unbind('click').click(me.initMessageGrid);
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
        me.showMenuEdit();
        //// User actionss
        me.$allRecoverChatUser.unbind('click').click(me.recoverChatUser);
        //// Chat Message actions
        me.$allRecoverChatMessage.unbind('click').click(me.recoverChatMessage);
    };


    me.showMenuEdit = function () {
        //// Edit Chat User Menu
        $.contextMenu({
            selector: '#messageGrid .open-menu-remove-user',
            trigger: 'left',
            autoHide: true,
            callback: function (key/*, options*/) {
                var $spanData = $(this);
                switch (key) {
                    case 'kickout':
                        {
                            me.kickOutChatUser($spanData.attr('data-chat-user-id'));
                            break;
                        }
                    case 'ban':
                        {
                            me.banChatUser($spanData.attr('data-chat-user-id'));
                            break;
                        }
                }
            },
            items: {
                'kickout': { name: "Kick out this user", icon: "delete" },
                'ban': { name: "Ban this user", icon: "delete" }
            }
        });
        //// Edit Message Menu
        $.contextMenu({
            selector: '#messageGrid .open-menu-remove-message',
            trigger: 'left',
            autoHide: true,
            callback: function (key/*, options*/) {
                var $spanData = $(this);
                switch (key) {
                    case 'removeThisMessage':
                        {
                            me.removeChatMessage($spanData.attr('data-chat-message-id'));
                            break;
                        }
                    case 'removeAllMessageOfChatUser':
                        {
                            me.removeAllChatMessageByChatUser($spanData.attr('data-chat-user-id'));
                            break;
                        }
                }
            },
            items: {
                "removeThisMessage": { name: "Remove This Message", icon: "delete" },
                "removeAllMessageOfChatUser": { name: "Remove All Messages From This User ", icon: "delete" }
            }
        });
    };

    ///// Chat User Action
    me.banChatUser = function (currentChatUserId) {
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
                    me.initMessageGrid();
                }
            }
        });
    };
    me.kickOutChatUser = function (currentChatUserId) {
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
        var memberId = parseInt(me.$ddlMemberName.val()),
            chatUserId = parseInt(me.$ddlChatName.val()),
            searchText = me.$txtMessageSearch.val();
        me.bodyRows = [];
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.getChatMessagesUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId,
                chatUserId: chatUserId,
                searchText: searchText
            },
            done: function (result) {
                me.usersAndMessageCached = result;
                me.updateUsersStatus(me.usersAndMessageCached.ChatUsers);
                me.renderGrid();
            }
        });
    };

    me.renderGrid = function () {
        me.gridOptions.bodyRows = me.buildGridBody(me.usersAndMessageCached.ChatMessages);
        me.$messageGrid.fanexGrid(me.gridOptions);
        me.registerGridEvents();
    };

    me.gridOptions = {
        columns: [
                   { HeaderText: LANGUAGE.timeGTM8, Width: 150, Name: 'Time', CellAlign: 'center', FormatDateTime: 'DD MMM, YYYY hh:mm A' },
                   { HeaderText: LANGUAGE.username, Width: 150, Name: 'UserName', 
                     CellAlign: 'center', CellCssClass: 'user-name-content', MergeHeaderColNumber: 2 },
                   {
                       HeaderText: '', Width: 40, Name: 'ChatNameAction', CellAlign: 'center', CustomCellRender: function (cell) {
                           var kickedOut = me.usersStatus[cell.Value.MemberId].KickedOut;
                           var userStatusClass = kickedOut ? 'recover-action recover-chat-user' : 'edit-action open-menu-remove-user';
                           var iconHtml = '<span class="' + userStatusClass + '"'
                                            + ' data-member-id="' + cell.Value.MemberId + '"'
                                            + ' data-chat-user-id="' + cell.Value.ChatUserId + '"'
                                            + '></span>';
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
                           return iconHtml;
                       }
                   }
        ],
        gridExpandHeight: 170,
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
        var messagesLength = chatMessages.length,
            bodyRows = [];

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
        var memberId = parseInt(me.$ddlMemberName.val()),
            chatUserId = parseInt(me.$ddlChatName.val());
        me.usersStatus = {};
        me.$ddlMemberName.empty().append('<option value="0">All</option>');
        me.$ddlChatName.empty().append('<option value="0">All</option>');
        var userLength = usersInRoom.length;
        for (var i = 0; i < userLength; i++) {
            var currentUser = usersInRoom[i];
            me.usersStatus[currentUser.ChatUser.MemberId] = currentUser;
            me.$ddlMemberName.append('<option value="' + currentUser.ChatUser.MemberId + '" '
                                        + (memberId === currentUser.ChatUser.MemberId ? 'selected ' : '')
                                        + 'data-chat-user-id="' + currentUser.ChatUser.ChatUserId + '"'
                                        +'>' + currentUser.ChatUser.MemberId + '</option>');
            me.$ddlChatName.append('<option value="' + currentUser.ChatUser.ChatUserId + '" '
                                        + (chatUserId === currentUser.ChatUser.ChatUserId ? 'selected' : '')
                                        + 'data-member-id="' + currentUser.ChatUser.MemberId + '"'
                                        + '>' + currentUser.ChatUser.ChatName + '</option>');
        }
        if (me.createDllSearch) {
            me.$ddlMemberName.trigger("chosen:updated");
            me.$ddlChatName.trigger("chosen:updated");
        }
        else {
            me.$ddlMemberName.chosen({}).change(me.memberChange);
            me.$ddlChatName.chosen({}).change(me.chatNameChange);
            me.createDllSearch = true;
        }
    };
    me.chatNameChange = function () {
        var currentChatUserSelected = me.$ddlChatName.find('option:selected');
        me.$ddlMemberName.val(currentChatUserSelected.attr('data-member-id')).trigger("chosen:updated");
    };

    me.memberChange = function () {
        var currentChatMemberSelected = me.$ddlMemberName.find('option:selected');
        me.$ddlChatName.val(currentChatUserSelected.attr('data-chat-user-id')).trigger("chosen:updated");
    };
    me.exportExcel = function () {
        LiveChartsAdmin.downloadFile(me.chatMonitorDefault.exportExcelUrl, {
            matchId: me.chatMonitorDefault.matchId,
            chatUserId: parseInt(me.$ddlChatName.val()),
            searchText: me.$txtMessageSearch.val()
        });
    };
    me.init = function () {
        me.cacheDomElements();
        me.registerEvents();
        me.initMessageGrid();
    };
}


$(function () {
    'use strict';
    var chatMonitor = new ChatMonitor();
    chatMonitor.init();
});