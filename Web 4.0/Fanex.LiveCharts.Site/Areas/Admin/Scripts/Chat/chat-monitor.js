/* File Created: April 1, 2014 */
/*global CHAT_MONITOR_DEFAULT, LiveChartsAdmin, LANGUAGE*/
function ChatMonitor() {
    'use strict';
    var me = this;
    me.chatMonitorDefault = CHAT_MONITOR_DEFAULT;
    me.usersStatus = {};
    me.createDllSearch = false;
    me.usersAndMessageCached = {};
    me.refreshMinutes = 1;
    me.stopCountDown = false;


    me.cacheDomElements = function () {
        me.$messageGrid = $('#messageGrid');
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
        me.startCountDown();

    };

    me.startCountDown = function () {
        me.$resultRefreshCountDown.parent().show();
        me.$resultRefreshCountDown.refreshCountDown({
            countDownMinutes: me.refreshMinutes,
            countDownSeconds: 0,
            refreshFunction: function (countDownObject, countDownStart) {
                //                            if (!me.isStopPrediction) {
                //                                me.getCorrectScoreResult({
                //                                    matchId: me.matchId
                //                                });
                //                                me.checkMatchStatus();
                if (!me.stopCountDown) {
                    me.initMessageGrid();
                }
                setTimeout(function () {
                    countDownStart(countDownObject);
                }, 1000);
                //                            }
                //                            else {
                //                                me.$resultRefreshCountDown.refreshCountDown('stopCountDown');
                //                            }
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

        //me.$btnReload.unbind('click').click(me.initMessageGrid);
        //// Open Menu
        me.showMenuEdit();
        //// User actionss
        me.$allRecoverChatUser.unbind('click').click(me.recoverChatUser);
        //// Chat Message actions
        me.$allRecoverChatMessage.unbind('click').click(me.recoverChatMessage);
        //// Chat User status search
        me.$chatUserList.find('.chat-user-status').unbind('click').click(me.searchByChatUserEvent);

        
        
    };


    me.showMenuEdit = function () {

        //// Edit Chat User Menu
        $.contextMenu({
            selector: '#messageGrid .open-menu-remove-user',
            trigger: 'left',
            autoHide: true,
            build: function ($trigger/*, e*/) {
                var items = {},
                    currentStatus = $trigger.attr('data-user-status');
                switch (currentStatus) {
                    case 'active':
                        items.kickout = { name: LANGUAGE.kickOutThisUser, icon: "kick-out" };
                        items.ban = { name: LANGUAGE.banThisUser, icon: "ban" };
                        break;
                    case 'kickedOut':
                        items.recover = { name: LANGUAGE.reAddThisUser, icon: "add" };
                        items.ban = { name: LANGUAGE.banThisUser, icon: "ban" };
                        break;
                    case 'banned':
                        items.unban = { name: LANGUAGE.unBanThisUser, icon: "un-ban" };
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
                                me.updateBanChatUser(currentChatUserId, true);
                                break;
                            case 'recover':
                                me.recoverChatUser(currentChatUserId);
                                break;
                            case 'unban':
                                me.updateBanChatUser(currentChatUserId, false);
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
            build: function ($trigger/*, e*/) {
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
        me.gridOptions.pagerOption.currentPage = 0;
        me.gridOptions.bodyRows = me.buildGridBody(me.usersAndMessageCached.ChatMessages);
        me.$messageGrid.fanexGrid(me.gridOptions);
        me.registerGridEvents();
    };
    me.exportExcel = function () {
        LiveChartsAdmin.downloadFile(me.chatMonitorDefault.exportExcelUrl, {
            matchId: me.chatMonitorDefault.matchId,
            chatUserId: parseInt(me.$ddlChatName.val()),
            searchText: me.$txtMessageSearch.val()
        });
    };
    me.gridOptions = {
        columns: [
                   { HeaderText: LANGUAGE.timeGTM8, Width: 150, Name: 'Time', CellAlign: 'center', FormatDateTime: 'DD MMM, YYYY hh:mm A' },
                   {
                       HeaderText: LANGUAGE.username, Width: 40, Name: 'ChatUserStatus', CellAlign: 'center',
                       CellCssClass: 'user-status', CustomCellRender: function (cell) {
                           var currentUser = me.usersStatus[cell.Value],
                               userStatusClass = 'user-online',
                               statusTooltip = 'Online';
                           if (currentUser.ChatUser.IsBanned) {
                               userStatusClass = 'user-disabled';
                               statusTooltip = 'Banned';
                           }
                           else if (currentUser.KickedOut) {
                               userStatusClass = 'user-kickedout';
                               statusTooltip = 'KickedOut';
                           }
                           else if (currentUser.IsLeft) {
                               userStatusClass = 'user-offline';
                               statusTooltip = 'OffLine';
                           }
                           return '<span title="' + statusTooltip + '" class="' + userStatusClass + '"></span>';
                       }, MergeHeaderColNumber: 3
                   },
                   { HeaderText: '', Width: 150, Name: 'UserName',
                       CellAlign: 'center', CellCssClass: 'user-name-content', AllowTruncate: true
                   },
                   {
                       HeaderText: '', Width: 40, Name: 'ChatNameAction', CellAlign: 'center', CustomCellRender: function (cell) {
                           var currentUser = me.usersStatus[cell.Value],
                               currentStatus = 'active';
                           if (currentUser.ChatUser.IsBanned) {
                               currentStatus = 'banned';
                           }
                           else if (currentUser.KickedOut) {
                               currentStatus = 'kickedOut';
                           }

                           var iconHtml = '<span class="edit-action open-menu-remove-user"'
                                            + ' data-user-status="' + currentStatus + '"'
                                            + ' data-member-id="' + currentUser.ChatUser.MemberId + '"'
                                            + ' data-chat-user-id="' + currentUser.ChatUser.ChatUserId + '"'
                                            + '></span>';
                           return iconHtml;
                       }
                   },
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
                   { HeaderText: '', Width: 405, Name: 'ChatMessage', CellCssClass: 'message-content' },
                   {
                       HeaderText: '', Width: 40, Name: 'ChatMessageAction', CellAlign: 'center', CustomCellRender: function (cell) {
                           var iconHtml = '<span class="edit-action open-menu-remove-message"'
                                                + ' data-message-status="' + (cell.Value.isDeleted ? 'deleted' : 'avail') + '"'
                                                + ' data-chat-message-id="' + cell.Value.chatMessageId + '"'
                                                + ' data-chat-user-id="' + cell.Value.chatUserId + '"'
                                            + '></span>';
                           return iconHtml;
                       }
                   }
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
    me.buildGridBody = function (chatMessages) {
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
                   Value:  me.usersStatus[currentMessage.ChatUser.MemberId].ChatUser.MemberName
               },
               { Name: "ChatNameAction", Value: currentMessage.ChatUser.MemberId },
               { Name: "ChatName", Value: currentMessage.ChatUser.ChatName },
               { Name: "ChatMessageStatus", Value: currentMessage.IsDeleted },
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
            chatUserId = parseInt(me.$ddlChatName.val()),
            sortedUsers = me.getSortedUsers(usersInRoom),
            userLength = usersInRoom.length;
        me.usersStatus = {};
        me.$ddlMemberName.empty().append('<option value="0" data-chat-user-id="0">All</option>');
        me.$ddlChatName.empty().append('<option value="0" data-member-id="0">All</option>');
        me.$chatUserList.empty();
        

        for (var i = 0; i < userLength; i++) {
            var currentUser = usersInRoom[i],
                currentMember = sortedUsers.memberArray[i],
                currentChatUser = sortedUsers.chatUserArray[i],
                currentChatUserStatus = sortedUsers.chatUserStatusArray[i];

            me.usersStatus[currentUser.ChatUser.MemberId] = currentUser;
            me.$ddlMemberName.append(me.buildMemberOption(currentMember, memberId));
            me.$ddlChatName.append(me.buildChatNameOption(currentChatUser, chatUserId));
            me.$chatUserList.append(me.buildChatUserStatusOption(currentChatUserStatus));
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
    me.buildChatUserStatusOption = function (currentChatUserStatus) {
        var currentUserStatusClass = 'user-online';
        if (currentChatUserStatus.ChatUser.IsBanned)
        {
            currentUserStatusClass = 'user-banned';
        }
        else if (currentChatUserStatus.KickedOut)
        {
            currentUserStatusClass = 'user-kicked-out';
        }
        else if (currentChatUserStatus.IsLeft)
        {
            currentUserStatusClass = 'user-left';
        }
        return '<li class="chat-user-status" '
                    + 'data-chat-user-id="' + currentChatUserStatus.ChatUser.ChatUserId + '" '
                    + 'data-member-id="' + currentChatUserStatus.ChatUser.MemberId + '" '
                + '>'
                    + '<span class="' + currentUserStatusClass + '"></span>'
                    + '<span class="chat-name-text">' + currentChatUserStatus.ChatUser.ChatName + '</span>'
                + '</li>';
    };
    me.getSortedUsers = function (usersInRoom) {
        var usersLength = usersInRoom.length,
            memberArray = usersInRoom.slice(0, usersLength).sort(function (userA, userB) {
                return userA.ChatUser.MemberId > userB.ChatUser.MemberId;
            }),
            chatUserArray = usersInRoom.slice(0, usersLength).sort(function (userA, userB) {
                return userA.ChatUser.ChatName > userB.ChatUser.ChatName;
            }),
            chatUserStatusArray = usersInRoom.slice(0, usersLength).sort(function (userA, userB) {
                if (userA.IsLeft && !userB.IsLeft) {
                    return true;
                } else if (userA.KickedOut && (!userB.IsLeft || !userB.KickedOut)) {
                    return true;
                }
                else if (userA.ChatUser.IsBanned && (!userB.IsLeft || !userB.KickedOut || !userB.ChatUser.IsBanned)) {
                    return true;
                }

                return false;
            });

        return {
            memberArray: memberArray,
            chatUserArray: chatUserArray,
            chatUserStatusArray: chatUserStatusArray
        };
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
            chatUserStatusWidth = 160,
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