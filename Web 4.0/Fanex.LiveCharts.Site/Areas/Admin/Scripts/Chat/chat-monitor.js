/* File Created: April 1, 2014 */
/*global CHAT_MONITOR_DEFAULT, LiveChartsAdmin, LANGUAGE*/
function ChatMonitor() {
    'use strict';
    var me = this;
    me.chatMonitorDefault = CHAT_MONITOR_DEFAULT;
    me.usersStatus = {};
    me.createDllSearch = false;
    me.roomData = {};
    me.refreshMinutes = 1;
    me.stopCountDown = false;


    me.cacheDomElements = function () {
        me.$messageGrid = $('#messageGrid');
        me.$chatUserGrid = $('#chatUserGrid');
        me.$menuEditChatUser = $('#menuEditChatUser');
        me.$menuEditChatMessage = $('#menuEditChatMessage');
        me.$btnSearchChatMessage = $('#btnSearchChatMessage');
        me.$ddlMemberName = $('#ddlMemberName');
        me.$ddlChatName = $('#ddlChatName');
        me.$txtMessageSearch = $('#txtMessageSearch');
        me.$chatUserList = $('#chatUserList');
        me.$resultRefreshCountDown = $('#resultRefreshCountDown');
        me.$btnExportmessageGrid = $('#btnExportmessageGrid');
        me.$stopAutoRefresh = $('#stopAutoRefresh');
        me.$content = $('#body');
    };

    me.registerEvents = function () {
        me.$btnSearchChatMessage.unbind('click').click(me.initMessageGrid);
        me.$btnExportmessageGrid.unbind('click').click(me.exportExcel);
        me.$stopAutoRefresh.unbind('click').click(me.handleRefreshCountDown);
        setInterval(me.updateUnMuteForExpireMute, 300000);
        me.startCountDown();

    };

    me.startCountDown = function () {
        me.$resultRefreshCountDown.parent().show();
        me.$resultRefreshCountDown.refreshCountDown({
            countDownMinutes: me.refreshMinutes,
            countDownSeconds: 0,
            refreshFunction: function (countDownObject, countDownStart) {
                if (!me.stopCountDown) {
                    me.initMessageGrid();
                }
                setTimeout(function () {
                    countDownStart(countDownObject);
                }, 1000);
            }
        });
    };

    me.handleRefreshCountDown = function () {
        var $this = $(this);
        var isCheck = $this.prop('checked');
        me.stopCountDown = isCheck;
        if (isCheck) {
            me.$resultRefreshCountDown.refreshCountDown('stopCountDown');
            me.$resultRefreshCountDown.parent().hide();
        }
        else {
            me.startCountDown();
        }
    };

    me.registerGridEvents = function () {
        //me.$btnReload = $('#btnReload');
        me.$allOpenMenuRemoveUser = $('#messageGrid .open-menu-remove-user');
        me.$allKickOutChatUser = $('#messageGrid .kickout-chat-user');
        me.$allBanChatUser = $('#messageGrid .ban-chat-user');
        me.$allRemoveOneMessage = $('#messageGrid .remove-one-message');
        me.$allRemoveAllMessages = $('#messageGrid .remove-all-messages');
        me.$allOpenMenuRemoveMessage = $('#messageGrid .open-menu-remove-message');
        me.$allRecoverChatUser = $('#messageGrid .recover-chat-user');
        me.$allRecoverChatMessage = $('#messageGrid .recover-chat-message');
        me.$allMenuActions = $('#messageGrid .menu-action');
        me.$btnMuteButton = $('#muteButton');

        //me.$btnReload.unbind('click').click(me.initMessageGrid);
        //// Open Menu
        me.showMenuEdit();
        //// User actionss
        me.$allRecoverChatUser.unbind('click').click(me.recoverChatUser);
        //// Chat Message actions
        me.$allRecoverChatMessage.unbind('click').click(me.recoverChatMessage);
        //// Chat User status search

        me.$chatUserGrid.find('.user-status-click-search').unbind('click').click(me.searchByChatUserEvent);
        //me.$chatUserList.find('.chat-user-status').unbind('click').click(me.searchByChatUserEvent);

        if (me.$btnMuteButton.hasClass('btn-mute')) {
            $.contextMenu({
                selector: '#muteButton',
                trigger: 'left',
                autoHide: true,
                build: function () {
                    return {
                        callback: function (key) {
                            switch (key) {
                                case 'muteall':
                                    me.updateMuteChatRoomOnAdminSite(true, 101);
                                    break;
                                case 'mute5':
                                    me.updateMuteChatRoomOnAdminSite(true, 1);
                                    break;
                                case 'mute10':
                                    me.updateMuteChatRoomOnAdminSite(true, 2);
                                    break;
                                case 'mute15':
                                    me.updateMuteChatRoomOnAdminSite(true, 15);
                                    break;
                            }
                        },
                        items: {
                            "mute5": { name: LANGUAGE.muteThisRoom5Minutes, icon: "mute" },
                            "mute10": { name: LANGUAGE.muteThisRoom10Minutes, icon: "mute" },
                            "mute15": { name: LANGUAGE.muteThisRoom15Minutes, icon: "mute" },
                            "muteall": { name: LANGUAGE.muteThisRoomInWholeMatchTime, icon: "mute" }
                        }
                    };
                }
            });
        }
        else {
            me.$btnMuteButton.unbind('click').click(function () { me.updateMuteChatRoomOnAdminSite(false, 0); });
        }

    };


    me.showMenuEdit = function () {
        //// Edit Chat User Menu
        $.contextMenu({
            selector: '#chatUserGrid .open-menu-remove-user',
            trigger: 'left',
            autoHide: true,
            build: function ($trigger) {
                var items = {},
                    currentStatus = $trigger.attr('data-user-status');
                switch (currentStatus) {
                    case 'active':
                        items.mute5 = { name: LANGUAGE.muteThisUser5Minutes, icon: 'mute' };
                        items.mute10 = { name: LANGUAGE.muteThisUser10Minutes, icon: 'mute' };
                        items.mute15 = { name: LANGUAGE.muteThisUser15Minutes, icon: 'mute' };
                        items.muteall = { name: LANGUAGE.muteThisUser, icon: 'mute' };
                        items.kickout = { name: LANGUAGE.kickOutThisUser, icon: 'kick-out' };
                        items.ban = { name: LANGUAGE.banThisUser, icon: 'ban' };
                        break;
                    case 'muted':
                        items.unmute = { name: LANGUAGE.unmuteThisUser, icon: 'un-mute' };
                        items.kickout = { name: LANGUAGE.kickOutThisUser, icon: 'kick-out' };
                        items.ban = { name: LANGUAGE.banThisUser, icon: 'ban' };
                        break;
                    case 'kickedOut':
                        items.recover = { name: LANGUAGE.reAddThisUser, icon: 'add' };
                        items.ban = { name: LANGUAGE.banThisUser, icon: 'ban' };
                        break;
                    case 'banned':
                        items.unban = { name: LANGUAGE.unBanThisUser, icon: 'un-ban' };
                        break;
                }

                return {
                    callback: function (key/*, options*/) {
                        var $spanData = $(this),
                            currentChatUserId = $spanData.attr('data-chat-user-id');
                        switch (key) {
                            case 'kickout':
                                me.kickOutChatUser(currentChatUserId);
                                break;
                            case 'ban':
                            case 'unban':
                                me.updateBanChatUser(currentChatUserId, key === 'ban');
                                break;
                            case 'recover':
                                me.recoverChatUser(currentChatUserId);
                                break;
                            case 'muteall':
                                me.updateMuteChatUserOnAdminSite(currentChatUserId, true, 101);
                                break;
                            case 'mute5':
                                me.updateMuteChatUserOnAdminSite(currentChatUserId, true, 1);
                                break;
                            case 'mute10':
                                me.updateMuteChatUserOnAdminSite(currentChatUserId, true, 2);
                                break;
                            case 'mute15':
                                me.updateMuteChatUserOnAdminSite(currentChatUserId, true, 15);
                                break;
                            case 'unmute':
                                me.updateMuteChatUserOnAdminSite(currentChatUserId, false);
                                break;
                        }
                    },
                    items: items
                };
            }
        });
        //// Edit Message Menu
        $.contextMenu({
            selector: '#messageGrid .open-menu-remove-message',
            trigger: 'left',
            autoHide: true,
            build: function ($trigger) {
                var items = {},
                    currentStatus = $trigger.attr('data-message-status');
                switch (currentStatus) {
                    case 'deleted':
                        items.recoverThisMessage = { name: LANGUAGE.recoverThisMessage, icon: "recover" };
                        items.recoverAllMessageOfChatUser = { name: LANGUAGE.recoverMessageFromUser, icon: "recover-all" };
                        items.removeAllMessageOfChatUser = { name: LANGUAGE.removeAllMessagesFromUser, icon: "remove-all" };
                        break;
                    case 'avail':
                        items.removeThisMessage = { name: LANGUAGE.removeThisMessage, icon: "delete" };
                        items.recoverAllMessageOfChatUser = { name: LANGUAGE.recoverMessageFromUser, icon: "recover-all" };
                        items.removeAllMessageOfChatUser = { name: LANGUAGE.removeAllMessagesFromUser, icon: "remove-all" };
                        break;
                }
                return {
                    callback: function (key/*, options*/) {
                        var $spanData = $(this);
                        switch (key) {
                            case 'removeThisMessage':
                                me.updateChatMessage($spanData.attr('data-chat-message-id'), true);
                                break;
                            case 'removeAllMessageOfChatUser':
                                me.updateAllChatMessageByChatUser($spanData.attr('data-chat-user-id'), true);
                                break;
                            case 'recoverThisMessage':
                                me.updateChatMessage($spanData.attr('data-chat-message-id'), false);
                                break;
                            case 'recoverAllMessageOfChatUser':
                                me.updateAllChatMessageByChatUser($spanData.attr('data-chat-user-id'), false);
                                break;
                        }
                    },
                    items: items
                };
            }
        });
    };

    ///// Chat User Action
    me.updateBanChatUser = function (currentChatUserId, isBan) {
        if (isBan && !LiveChartsAdmin.confirmUpdateAction()) {
            return;
        }
        //banChatUserUrl
        LiveChartsAdmin.callAjax({
            url: isBan ? me.chatMonitorDefault.banChatUser : me.chatMonitorDefault.recoverBannedChatUserUrl,
            type: 'POST',
            data: {
                chatUserId: currentChatUserId
            },
            done: function (banViewModel) {
                var result = banViewModel.CommandResult;
                if (!isBan) {
                    result = banViewModel;
                }
                if (result.IsSuccessful) {
                    if (isBan) {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.banChatUserSuccess);
                        me.banChatUserOnMemberSite(banViewModel);
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.recoverChatUser);
                    }

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
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.kickedOutChatUserSuccess);
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
            data: kickOutViewModel,
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
    me.banChatUserOnMemberSite = function (banChatUserViewModel) {

        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.memberChartSiteUrl + '/Chat/BanChatUser',
            type: 'POST',
            data: banChatUserViewModel,
            done: function (result) {
                if (result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.banChatUserSuccess);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };
    me.recoverChatUser = function (currentChatUserId) {
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
    me.updateMuteChatUserOnAdminSite = function (currentChatUserId, isMute, mutePeriod) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.updateMuteChatUserUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId,
                chatRoomName: me.chatMonitorDefault.chatRoomName,
                chatUserId: currentChatUserId,
                mutePeriod: mutePeriod,
                isMute: isMute
            },
            done: function (result) {
                if (result.CommandResult.IsSuccessful) {
                    me.updateMuteOnMemberSite(result, false);
                    me.initMessageGrid();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };
    me.updateMuteOnMemberSite = function (muteViewModel, isRoom) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.memberChartSiteUrl + (isRoom ? '/Chat/UpdateMuteChatRoom' : '/Chat/UpdateMuteChatUser'),
            type: 'POST',
            data: muteViewModel,
            done: function (result) {
                if (result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };
    me.updateMuteChatRoomOnAdminSite = function (isMute, mutePeriod) {
        isMute = isMute || false;
        if (isMute && !LiveChartsAdmin.confirmUpdateAction()) {
            return;
        }

        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.updateMuteChatRoomUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId,
                chatRoomName: me.chatMonitorDefault.chatRoomName,
                mutePeriod: mutePeriod,
                isMute: isMute
            },
            done: function (result) {
                if (result.CommandResult.IsSuccessful) {
                    me.updateMuteOnMemberSite(result, true);
                    me.chatMonitorDefault.isRoomMuted = isMute;
                    me.initMessageGrid();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    ///// Chat Message Action
    me.updateChatMessage = function (currentChatMessageId, isDelete) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.updateChatMessageUrl,
            type: 'POST',
            data: {
                chatRoomName: me.chatMonitorDefault.chatRoomName,
                chatMessageId: currentChatMessageId,
                isDelete: isDelete
            },
            done: function (updateMessageViewModel) {
                if (updateMessageViewModel.CommandResult.IsSuccessful) {
                    me.updateChatMessageOnMemberSite(updateMessageViewModel);
                    me.initMessageGrid();
                }
            }
        });
    };
    me.updateAllChatMessageByChatUser = function (currentChatUserId, isDelete) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.updateAllChatMessageByChatUserUrl,
            type: 'POST',
            data: {
                chatRoomName: me.chatMonitorDefault.chatRoomName,
                chatUserId: currentChatUserId,
                isDelete: isDelete
            },
            done: function (updateMessageByChatUserViewModel) {
                if (updateMessageByChatUserViewModel.CommandResult.IsSuccessful) {
                    me.updateAllChatMessagebyChatUserOnMemberSite(updateMessageByChatUserViewModel);
                    me.initMessageGrid();
                }
            }
        });
    };
    me.updateChatMessageOnMemberSite = function (deleteChatMessageViewModel) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.memberChartSiteUrl + '/Chat/UpdateChatMessage',
            type: 'POST',
            data: deleteChatMessageViewModel,
            done: function (result) {
                if (result.IsSuccessful) {
                    if (deleteChatMessageViewModel.IsDeleted) {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.deleteMessageSuccess);
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.recoverChatSuccess);
                    }
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };
    me.updateAllChatMessagebyChatUserOnMemberSite = function (updateMessageByChatUserViewModel) {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.memberChartSiteUrl + '/Chat/UpdateAllChatMessagesByChatUser',
            type: 'POST',
            data: updateMessageByChatUserViewModel,
            done: function (result) {
                if (result.IsSuccessful) {
                    if (updateMessageByChatUserViewModel.IsDeleted) {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.deleteMessageSuccess);
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.recoverChatSuccess);
                    }
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    //// Chat Room Action
    me.updateUnMuteForExpireMute = function () {
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.updateUnMuteForExpiredMute,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId,
                updateType: 0 //// Check both chat room muted and chat user muted
            },
            done: function (result) {
                if (!result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    //// Grid
    me.initMessageGrid = function () {
        var chatUserId = parseInt(me.$ddlChatName.val()),
            searchText = me.$txtMessageSearch.val();
        me.bodyRows = [];
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.getChatMessagesUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId,
                chatUserId: chatUserId,
                searchText: searchText,
                chatRoomName: me.chatMonitorDefault.chatRoomName,
                matchId: me.chatMonitorDefault.matchId
            },
            done: function (result) {
                me.roomData = result;
                me.updateUsersStatus(me.roomData.ChatUsers);
                me.renderGrid();
            }
        });
    };
    me.renderGrid = function () {
        me.messageGridOptions.pagerOption.currentPage = 0;
        me.messageGridOptions.bodyRows = me.buildMessageGridBody(me.roomData.ChatMessages);
        me.$messageGrid.fanexGrid(me.messageGridOptions);
        me.registerGridEvents();
    };
    me.exportExcel = function () {
        LiveChartsAdmin.downloadFile(me.chatMonitorDefault.exportExcelUrl, {
            matchId: me.chatMonitorDefault.matchId,
            chatUserId: parseInt(me.$ddlChatName.val()),
            searchText: me.$txtMessageSearch.val()
        });
    };
    me.renderColumnChatUserStatus = function (cell) {
        var currentUser = me.usersStatus[cell.Value],
            userStatusClass = 'user-online',
            statusTooltip = 'Online';
        if (currentUser) {
            if (currentUser.ChatUser.IsBanned) {
                userStatusClass = 'user-disabled';
                statusTooltip = 'Banned';
            }
            else if (currentUser.KickedOut) {
                userStatusClass = 'user-kickedout';
                statusTooltip = 'KickedOut';
            }
            else if (currentUser.Muted && currentUser.ContinueMuteInMinutes !== 0) {
                userStatusClass = 'user-muted';
                statusTooltip = 'Muted';
            }
            else if (currentUser.IsLeft) {
                userStatusClass = 'user-offline';
                statusTooltip = 'OffLine';
            }
            return '<span title="' + statusTooltip + '" class="' + userStatusClass + '"></span>';
        }
        else {
            return '<span></span>';
        }
    };
    me.renderChatNameAction = function (cell) {
        var currentUser = me.usersStatus[cell.Value],
                               currentStatus = 'active';
        if (currentUser) {
            if (currentUser.ChatUser.IsBanned) {
                currentStatus = 'banned';
            }
            else if (currentUser.KickedOut) {
                currentStatus = 'kickedOut';
            }
            else if (currentUser.Muted && currentUser.ContinueMuteInMinutes !== 0) {
                currentStatus = 'muted';
            }

            var iconHtml = '<span class="edit-action open-menu-remove-user"'
												+ ' data-user-status="' + currentStatus + '"'
												+ ' data-member-id="' + currentUser.ChatUser.MemberId + '"'
												+ ' data-chat-user-id="' + currentUser.ChatUser.ChatUserId + '"'
												+ '></span>';
            return iconHtml;
        }
        return '';
    };
    me.messageGridOptions = {
        columns: [
                   { HeaderText: LANGUAGE.timeGTM8, Width: 150, Name: 'Time', CellAlign: 'center', FormatDateTime: 'DD MMM, YYYY hh:mm A' },
                   {
                       HeaderText: LANGUAGE.username, Width: 40, Name: 'ChatUserStatus', CellAlign: 'center',
                       CellCssClass: 'user-status', CustomCellRender: me.renderColumnChatUserStatus, MergeHeaderColNumber: 2
                   },
                   { HeaderText: '', Width: 150, Name: 'UserName', CellAlign: 'center', CellCssClass: 'user-name-content', AllowTruncate: true },
                   { HeaderText: LANGUAGE.chatName, Width: 150, Name: 'ChatName', CellAlign: 'center', AllowTruncate: true },
                   {
                       HeaderText: LANGUAGE.chatMessage, Width: 40, Name: 'ChatMessageStatus', CellAlign: 'center',
                       CellCssClass: 'chat-message-status', CustomCellRender: function (cell) {
                           return '<span '
                                    + 'title="' + (cell.Value ? 'Deleted' : 'Available') + '"'
                                    + ' class="'
                                    + (cell.Value ? 'deleted-message' : 'avai-message')
                                    + '"></span>';
                       }, MergeHeaderColNumber: 3
                   },
                   {
                       HeaderText: '', Width: 40, Name: 'ChatMessageAction', CellAlign: 'center', CustomCellRender: function (cell) {
                           var iconHtml = '<span class="edit-action open-menu-remove-message"'
                                                + ' data-message-status="' + (cell.Value.isDeleted ? 'deleted' : 'avail') + '"'
                                                + ' data-chat-message-id="' + cell.Value.chatMessageId + '"'
                                                + ' data-chat-user-id="' + cell.Value.chatUserId + '"'
                                            + '></span>';
                           return iconHtml;
                       }
                   },
                   { HeaderText: '', Width: 340, Name: 'ChatMessage', CellCssClass: 'message-content' }
        ],
        gridExpandHeight: 195,
        showPager: true,
        pagerOption: {
            itemsPerPage: 50,
            currentPage: 0,
            itemsPerPageArray: [50, 100, 500],
            showDetail: true,
            afterPagedFunction: me.registerGridEvents
        },
        showTitle: false,
        timeZone: '+08:00'
    };
    me.chatUserGridOptions = {
        columns: [
            {
                HeaderText: LANGUAGE.chatName, Width: 30, Name: 'ChatUserStatus', CellAlign: 'center',
                CellCssClass: 'user-status', CustomCellRender: me.renderColumnChatUserStatus, MergeHeaderColNumber: 3
            },
            { HeaderText: '', Width: 150, Name: 'ChatName', CellAlign: 'center', AllowTruncate: true, CellCssClass: 'user-status-click-search' },
            { HeaderText: '', Width: 40, Name: 'ChatNameAction', CellAlign: 'center', CustomCellRender: me.renderChatNameAction }
        ],
        gridExpandHeight: 195,
        showTitle: true,
        titleInfo:
        {
            text: ""
        }
    };
    me.buildMessageGridBody = function (chatMessages) {
        var messagesLength = chatMessages.length,
            bodyRows = [];

        for (var i = 0; i < messagesLength; i++) {
            var currentMessage = chatMessages[i];

            bodyRows.push({
                Columns: [
               { Name: "Time", Value: currentMessage.CreateTime },

               { Name: "ChatUserStatus", Value: currentMessage.ChatUser.MemberId },
               {
                   Name: "UserName",
                   Value: me.usersStatus[currentMessage.ChatUser.MemberId] != null ? me.usersStatus[currentMessage.ChatUser.MemberId].ChatUser.MemberName : ''
               },
               { Name: "ChatName", Value: currentMessage.ChatUser.ChatName },
               { Name: "ChatMessageStatus", Value: currentMessage.IsDeleted },
               {
                   Name: 'ChatMessageAction', Value: {
                       chatMessageId: currentMessage.ChatMessageId,
                       isDeleted: currentMessage.IsDeleted,
                       chatUserId: currentMessage.ChatUser.ChatUserId,
                       memberName: currentMessage.ChatUser.ChatName
                   }
               },
               {
                   Name: "ChatMessage",
                   Value: currentMessage.Content.replace(/\r\n/g, "<br />").replace(/\n/g, "<br />")
               }
                ],
                CustomAttr: 'data-chat-message-id="' + currentMessage.ChatMessageId + '"'
            });

        }

        return bodyRows;
    };
    me.updateUsersStatus = function (usersInRoom) {
        var memberId = parseInt(me.$ddlMemberName.val()),
            chatUserId = parseInt(me.$ddlChatName.val()),
            sortedUsers = me.getSortedUsers(usersInRoom),
            userLength = usersInRoom.length;
        me.usersStatus = {};
        me.$ddlMemberName.empty().append('<option value="0" data-chat-user-id="0">All</option>');
        me.$ddlChatName.empty().append('<option value="0" data-member-id="0">All</option>');
        me.chatUserGridBodyRows = [];

        for (var i = 0; i < userLength; i++) {
            var currentUser = usersInRoom[i],
                currentMember = sortedUsers.memberArray[i],
                currentChatUser = sortedUsers.chatUserArray[i],
                currentChatUserStatus = sortedUsers.chatUserStatusArray[i];

            me.usersStatus[currentUser.ChatUser.MemberId] = currentUser;
            me.$ddlMemberName.append(me.buildMemberOption(currentMember, memberId));
            me.$ddlChatName.append(me.buildChatNameOption(currentChatUser, chatUserId));
            me.buildChatUserStatusRow(currentChatUserStatus);
        }

        //// Create new choosen or just update.
        if (me.createDllSearch) {
            me.$ddlMemberName.trigger("chosen:updated");
            me.$ddlChatName.trigger("chosen:updated");
        }
        else {
            me.$ddlMemberName.chosen({}).change(me.memberChange);
            me.$ddlChatName.chosen({}).change(me.chatNameChange);
            me.createDllSearch = true;
        }

        //// Render Chat Name Grid.
        me.chatUserGridOptions.bodyRows = me.chatUserGridBodyRows;
        me.chatUserGridOptions.titleInfo.text = me.buildTotalChatUserTitle();
        me.chatUserGridOptions.titleInfo.customButtons = me.renderMuteButton();
        me.$chatUserGrid.fanexGrid(me.chatUserGridOptions);
    };
    me.renderMuteButton = function () {
        //'<input type="button" id="muteButton" class="btn-mute" />'
        // me.chatMonitorDefault.
        var buttonHtml = '<input type="button" id="muteButton" ';
        if (me.roomData.ChatRoom.Muted) {
            buttonHtml += 'class="btn-un-mute" title="' + LANGUAGE.clickToUnMute + '"';
        }
        else {
            buttonHtml += 'class="btn-mute" title="' + LANGUAGE.clickToMute + '"';
        }

        return buttonHtml + ' />';
    };
    me.buildMemberOption = function (currentMember, currentSelectedMemberId) {
        return '<option value="' + currentMember.ChatUser.MemberId + '" '
                                        + (currentSelectedMemberId === currentMember.ChatUser.MemberId ? 'selected ' : '')
                                        + 'data-chat-user-id="' + currentMember.ChatUser.ChatUserId + '"'
                                        + '>' + currentMember.ChatUser.MemberName + '</option>';
    };
    me.buildChatNameOption = function (currentChatUser, currentSelectedChatUserId) {
        return '<option value="' + currentChatUser.ChatUser.ChatUserId + '" '
                                        + (currentSelectedChatUserId === currentChatUser.ChatUser.ChatUserId ? 'selected ' : '')
                                        + 'data-member-id="' + currentChatUser.ChatUser.MemberId + '"'
                                        + '>' + currentChatUser.ChatUser.ChatName + '</option>';
    };
    me.buildChatUserStatusRow = function (currentChatUserStatus) {
        me.chatUserGridBodyRows.push({
            Columns: [
               { Name: "ChatUserStatus", Value: currentChatUserStatus.ChatUser.MemberId },
               { Name: "ChatName", Value: currentChatUserStatus.ChatUser.ChatName,
                   CustomAttr: 'data-chat-user-id="' + currentChatUserStatus.ChatUser.ChatUserId + '"' +
                                'data-member-id="' + currentChatUserStatus.ChatUser.MemberId + '"'

               },
               { Name: "ChatNameAction", Value: currentChatUserStatus.ChatUser.MemberId }
               ]
        });
       };
       me.buildTotalChatUserTitle = function () {
           return '<div class="title-status"><span>Total: ' + me.chatUserGridBodyRows.length + '&nbsp;(&nbsp;</span>'
                + '<span class="user-online"></span><span>:' + me.totalUserInRoom.onlineUsers + ',&nbsp;</span>'
                + '<span class="user-muted"></span><span>:' + me.totalUserInRoom.mutedUsers + ',&nbsp;</span>'
                + '<span class="user-kickedout"></span><span>:' + me.totalUserInRoom.kickedOutUsers + ',&nbsp;</span>'
                + '<span class="user-disabled"></span><span>:' + me.totalUserInRoom.bannedUsers + '&nbsp;)</span>'
           + '</div>';
       };
    me.getSortedUsers = function (usersInRoom) {
        var memberArray = usersInRoom.slice(0).sort(function (userA, userB) {
            return userA.ChatUser.MemberName > userB.ChatUser.MemberName;
        }),
            chatUserArray = usersInRoom.slice(0).sort(function (userA, userB) {
                return userA.ChatUser.ChatName > userB.ChatUser.ChatName;
            }),
            chatUserStatusArray = me.sortChatUserStatus(usersInRoom.slice(0));

        return {
            memberArray: memberArray,
            chatUserArray: chatUserArray,
            chatUserStatusArray: chatUserStatusArray
        };
    };
    me.sortChatUserByChatName = function (userA, userB) {
        return userA.ChatUser.ChatName > userB.ChatUser.ChatName;
    };
    me.sortChatUserStatus = function (chatUsersInRoom) {
        var onlineUsers = [],
            offLineUser = [],
            kickedOutUsers = [],
            bannedUsers = [],
            mutedUsers = [],
            chatUserLength = chatUsersInRoom.length;

        for (var i = 0; i < chatUserLength; i++) {
            var currentUser = chatUsersInRoom[i];
            if (currentUser.ChatUser.IsBanned) {
                bannedUsers.push(currentUser);
            }
            else if (currentUser.KickedOut) {
                kickedOutUsers.push(currentUser);
            }
            else if (currentUser.Muted && currentUser.ContinueMuteInMinutes !== 0) {
                mutedUsers.push(currentUser);
            }
            else if (!currentUser.IsLeft) {
                onlineUsers.push(currentUser);
            }
            else {
                offLineUser.push(currentUser);
            }
        }

        me.totalUserInRoom =
        {
            onlineUsers: onlineUsers.length,
            offLineUser: offLineUser.length,
            kickedOutUsers: kickedOutUsers.length,
            bannedUsers: bannedUsers.length,
            mutedUsers: mutedUsers.length
        };

        return mutedUsers.concat(onlineUsers).concat(offLineUser).concat(kickedOutUsers).concat(bannedUsers);
    };
    me.searchByChatUserEvent = function () {
        var $this = $(this),
            currentChatUserId = $this.attr('data-chat-user-id'),
            currentMemberId = $this.attr('data-member-id');
        me.$ddlChatName.val(currentChatUserId).trigger("chosen:updated");
        me.$ddlMemberName.val(currentMemberId).trigger("chosen:updated");
        me.$btnSearchChatMessage.trigger('click');
    };
    me.chatNameChange = function () {
        var currentChatUserSelected = me.$ddlChatName.find('option:selected');
        me.$ddlMemberName.val(currentChatUserSelected.attr('data-member-id')).trigger("chosen:updated");
    };
    me.memberChange = function () {
        var currentChatMemberSelected = me.$ddlMemberName.find('option:selected');
        me.$ddlChatName.val(currentChatMemberSelected.attr('data-chat-user-id')).trigger("chosen:updated");
    };
    me.init = function () {
        me.cacheDomElements();
        me.registerEvents();
        me.autoAdjustWidth();
        me.initMessageGrid();
    };
    me.autoAdjustWidth = function () {
        var currentContentWidth = me.$content.width(),
            chatUserStatusWidth = 250,
            minContentWidth = chatUserStatusWidth + 600;
        if (currentContentWidth < minContentWidth) {
            currentContentWidth = minContentWidth;
        }
        var gridWidth = currentContentWidth - chatUserStatusWidth;
        me.$content.find('.chart-monitor').width(gridWidth - 30);
        me.$content.find('.grid-title').width(gridWidth - 35);
    };
}


$(function () {
    'use strict';
    var chatMonitor = new ChatMonitor();
    chatMonitor.init();
});