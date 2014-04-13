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
        me.$chatUserList = $('#chatUserList');
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
        //// Chat User status search
        me.$chatUserList.find('.chat-user-status').unbind('click').click(me.searchByChatUserEvent);
    };


    me.showMenuEdit = function () {
        $.contextMenu({
            selector: '.context-menu-one',
            build: function ($trigger, e) {
                // this callback is executed every time the menu is to be shown
                // its results are destroyed every time the menu is hidden
                // e is the original contextmenu event, containing e.pageX and e.pageY (amongst other data)
                return {
                    callback: function (key, options) {
                        var m = "clicked: " + key;
                        window.console && console.log(m) || alert(m);
                    },
                    items: {
                        "edit": { name: "Edit", icon: "edit" },
                        "cut": { name: "Cut", icon: "cut" },
                        "copy": { name: "Copy", icon: "copy" },
                        "paste": { name: "Paste", icon: "paste" },
                        "delete": { name: "Delete", icon: "delete" },
                        "sep1": "---------",
                        "quit": { name: "Quit", icon: "quit" }
                    }
                };
            }
        });

        //// Edit Chat User Menu
        $.contextMenu({
            selector: '#messageGrid .open-menu-remove-user',
            trigger: 'left',
            autoHide: true,
            build: function ($trigger, e) {
                var items = {},
                    currentStatus = $trigger.attr('data-user-status');
                switch(currentStatus)
                {
                    case 'active':
                        items.kickout = { name: "Kick out this user", icon: "delete" };
                        items.ban = { name: "Ban this user", icon: "delete" }
                        break;
                    case 'kickedOut':
                        items.recover = { name: "Re add this user", icon: "edit" };
                        items.ban = { name: "Ban this user", icon: "delete" }
                        break;
                    case 'banned':
                        items.unban = { name: "Un ban this user", icon: "cut" };
                        break;
                }
                //'kickout': { name: "Kick out this user", icon: "delete" },
                //    'ban': { name: "Ban this user", icon: "delete" }
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
                if (!isBan)
                {
                    result = banViewModel;
                }
                if (result.IsSuccessful) {
                    if (isBan) {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.banChatUserSuccess);
                    }
                    else
                    {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.recoverChatUser)
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
        var $this = $(this),
            currentChatMessageId = $this.attr('data-chat-message-id');

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
                       HeaderText: LANGUAGE.username, Width: 40, Name: 'ChatUserStatus', CellAlign: 'center', CellCssClass: 'user-status', CustomCellRender: function (cell) {
                           var currentUser = me.usersStatus[cell.Value],
                               userStatusClass = 'user-online';
                           if (currentUser.ChatUser.IsBanned)
                           {
                               userStatusClass = 'user-disabled';
                           }
                           else if (currentUser.KickedOut)
                           {
                               userStatusClass = 'user-kickedout';
                           }
                           else if (currentUser.IsLeft)
                           {
                               userStatusClass = 'user-offline';
                           }
                           return '<span class="' + userStatusClass + '"></span>';
                       }, MergeHeaderColNumber: 3 
                   },
                   { HeaderText: '', Width: 150, Name: 'UserName', 
                     CellAlign: 'center', CellCssClass: 'user-name-content'},
                   {
                       HeaderText: '', Width: 40, Name: 'ChatNameAction', CellAlign: 'center', CustomCellRender: function (cell) {
                           var currentUser = me.usersStatus[cell.Value],
                               currentStatus = 'active';
                           if (currentUser.ChatUser.IsBanned)
                           {
                               currentStatus = 'banned';
                           }
                           else if (currentUser.KickedOut)
                           {
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
                   { HeaderText: LANGUAGE.chatName, Width: 150, Name: 'ChatName', CellAlign: 'center' },
                   { HeaderText: LANGUAGE.chatMessage, Width: 450, Name: 'ChatMessage', MergeHeaderColNumber: 2, CellCssClass: 'message-content' },
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
        gridExpandHeight: 180,
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

               { Name: "ChatUserStatus", Value: currentMessage.ChatUser.MemberId },
               {
                   Name: "UserName",
                   Value: currentMessage.ChatUser.MemberId
               },
               { Name: "ChatNameAction", Value: currentMessage.ChatUser.MemberId },
               { Name: "ChatName", Value: currentMessage.ChatUser.MemberId },
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
                                        + '>' + currentMember.ChatUser.MemberId + '</option>';
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
                else if (userA.ChatUser.Banned && (!userB.IsLeft || !userB.KickedOut || !userB.ChatUser.Banned)) {
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
        me.initMessageGrid();
    };
}


$(function () {
    'use strict';
    var chatMonitor = new ChatMonitor();
    chatMonitor.init();
});