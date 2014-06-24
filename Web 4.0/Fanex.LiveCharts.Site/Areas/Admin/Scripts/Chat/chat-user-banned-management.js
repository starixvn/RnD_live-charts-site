/* File Created: May 17, 2014 */
/*global CHAT_USER_BANNED_MANAGEMENT_DEFAULT, LANGUAGE, LiveChartsAdmin*/
function ChatUserBannedManagement() {
    'use strict';

    var me = this;

    me.defaultValue = CHAT_USER_BANNED_MANAGEMENT_DEFAULT;

    me.chatUserStatus = {
        all: 0,
        banned: 1,
        online: 2
    };

    me.init = function () {
        me.cachedElements();
        me.registerEvents();
    };

    me.cachedElements = function () {
        me.$txtMemberSearch = $('#txtMemberSearch');
        me.$txtChatNameSearch = $('#txtChatNameSearch');
        me.$ddlChatUserStatus = $('#ddlChatUserStatus');
        me.$btnSearchChatUser = $('#btnSearchChatUser');
        me.$chatUserGrid = $('#chatUserGrid');
    };

    me.cacheGridElements = function () {
        me.$cbBanAll = $('#cbBanAll');
        me.$cbBan = $('#chatUserGrid .banned-action');
        me.$cbReset = $('#chatUserGrid .reset-chat-name-action');
    };

    me.registerEvents = function () {
        me.$btnSearchChatUser.unbind('click').click(me.searchChatUser);
    };

    me.registerGridEvents = function () {
        //me.$cbBanAll.unbind('click').click(me.updateBanAllChatUsers);
        me.registerMenuAction();
        me.$cbBan.unbind('click').click(me.updateBanChatUser);
        me.$cbReset.unbind('click').click(me.updateResetChatNameStatus);

    };

    me.registerMenuAction = function () {
        $.contextMenu({
            selector: '#cbBanAll',
            trigger: 'left',
            autoHide: true,
            items: {
                "ban": { name: "Ban All", icon: "ban" },
                "unban": { name: "Un Ban All", icon: "un-ban" },
                "reset": { name: "Reset Chat Name All", icon: "edit" },
                "unreset": { name: "Un-reset Chat Name All", icon: "un-reset" }
            },
            callback: function (key) {
                switch (key) {
                    case 'ban':
                    case 'unban':
                        me.updateBanAllChatUsers(key === 'ban');
                        break;
                    case 'reset':
                    case 'unreset':
                        me.updateResetAllChatNameStatus(key === 'reset');
                        break;
                }

            }
        });
    };

    me.updateBanChatUser = function (event) {
        var $this = $(this);
        me.banChatUsers(me.getChatUserId($this, event), $this.prop('checked'));
    };

    me.updateBanAllChatUsers = function (isBanned) {
        me.banChatUsers(me.getAllChatUserIds().join(','), isBanned);
    };

    me.banChatUsers = function (chatUserIds, isBanned) {
        if (isBanned && !LiveChartsAdmin.confirmUpdateAction()) {
            return;
        }

        LiveChartsAdmin.callAjax({
            url: me.defaultValue.updateBanChatUsersUrl,
            type: 'POST',
            data: {
                chatUserIds: chatUserIds,
                isBanned: isBanned
            },
            done: function (commandResult) {
                if (commandResult.CommandResult.IsSuccessful) {
                    me.banChatUsersOnMemberSite(commandResult, isBanned);
                    me.searchChatUser();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                }
            }
        });
    };

    me.getChatUserId = function ($element, event) {
        if ($element.prop('checked')) {
            event.preventDefault();
        }

        return $element.parent().parent().parent().attr('data-chat-user-id');
    };

    me.getAllChatUserIds = function () {
        var chatUserIds = [],
            chatUserLength = me.chatUsers.length;
        for (var i = 0; i < chatUserLength; i++) {
            chatUserIds.push(me.chatUsers[i].ChatUserId);
        }

        return chatUserIds;
    };

    me.updateResetChatNameStatus = function (event) {
        var $this = $(this);
        me.resetChatNames(me.getChatUserId($this, event), $this.prop('checked'));
    };

    me.updateResetAllChatNameStatus = function (isReset) {
        me.resetChatNames(me.getAllChatUserIds().join(','), isReset);
    };

    me.resetChatNames = function (chatUserIds, isReset) {
        if (isReset && !LiveChartsAdmin.confirmUpdateAction()) {
            return;
        }

        LiveChartsAdmin.callAjax({
            url: me.defaultValue.resetChatNameUrl,
            type: 'POST',
            data: {
                chatUserIds: chatUserIds,
                isResetChatName: isReset
            },
            done: function (commandResult) {
                if (commandResult.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
                    me.searchChatUser();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                }
            }
        });
    };

    me.banChatUsersOnMemberSite = function (commandResult, isBanned) {
        if (isBanned) {
            LiveChartsAdmin.callAjax({
                url: me.defaultValue.memberChartSiteUrl + '/Chat/BanListUsers',
                type: 'POST',
                data: commandResult,
                done: function (commandResult) {
                    if (commandResult.IsSuccessful) {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                    }
                }
            });
        }
        else {
            LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
        }
    };

    me.searchChatUser = function () {
        LiveChartsAdmin.callAjax({
            url: me.defaultValue.searchChatUserUrl,
            type: 'POST',
            data: {
                memberName: me.$txtMemberSearch.val(),
                chatName: me.$txtChatNameSearch.val(),
                chatUserStatus: me.$ddlChatUserStatus.val()
            },
            done: function (chatUsers) {
                me.chatUsers = chatUsers;
                me.renderChatUserGrid(chatUsers);
            }
        });
    };

    me.renderChatNameAction = function (cell) {
        return '<input type="checkbox" class="banned-action" '
            + (cell.Value ? 'checked' : '')
            + ' />';
    };

    me.renderResetChatNameAction = function (cell) {
        return '<input type="checkbox" class="reset-chat-name-action" '
            + (cell.Value ? 'checked' : '')
            + ' />';
    };

    me.chatUserBannedGridOption = {
        columns: [
            { HeaderText: "#", Width: 60, Name: "No", CellAlign: 'center' },
            {
                HeaderText: 'Member Name', Width: 300, Name: 'MemberName', CellAlign: 'center', AllowTruncate: true, CellCssClass: 'user-status'
            },
            { HeaderText: 'Chat Name', Width: 370, Name: 'ChatName', CellAlign: 'center', AllowTruncate: true, CellCssClass: 'user-status-click-search' },
            {
                HeaderText: 'Banned',
                Width: 80,
                Name: 'ChatUserAction',
                CellAlign: 'center',
                CustomCellRender: me.renderChatNameAction
            },
            { 
                HeaderText: 'Reset Chat Name', 
                Width: 80,
                Name: 'ResetChatName',
                CellAlign: 'center',
                CustomCellRender: me.renderResetChatNameAction 
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
        showTitle: true,
        showNumberColumn: true,
        titleInfo: {}
    };

    me.getChatUserGridBodyRows = function (chatUsers) {
        var chatUserLength = chatUsers.length,
            bodyRows = [];
        for (var i = 0; i < chatUserLength; i++) {
            var currentChatUser = chatUsers[i];
            bodyRows.push({
                Columns: [
                   { Name: "No", Value: '' },
                   { Name: "MemberName", Value: currentChatUser.MemberName },

                   { Name: "ChatName", Value: currentChatUser.ChatName },
                   {
                       Name: "ChatUserAction",
                       Value: currentChatUser.IsBanned
                   },
                   { Name: "ResetChatName", Value: currentChatUser.IsResetChatName }
                ],
                CustomAttr: 'data-chat-user-id="' + currentChatUser.ChatUserId + '"'
            });
        }

        return bodyRows;
    };

    me.buildGridTitle = function (chatUserStatus) {
        switch (chatUserStatus) {
            case me.chatUserStatus.banned:
                return LANGUAGE.listUserBanned;
            case me.chatUserStatus.online:
                return LANGUAGE.listOnlineUsers;
            default:
                return LANGUAGE.allChatUsers;
        }
    };

    me.renderChatUserGrid = function (chatUsers) {
        var bodyRows = me.getChatUserGridBodyRows(chatUsers);
        me.chatUserBannedGridOption.bodyRows = bodyRows;
        var currentStatus = parseInt(me.$ddlChatUserStatus.val());
        me.chatUserBannedGridOption.pagerOption.currentPage = 0;
        me.chatUserBannedGridOption.titleInfo.text = me.buildGridTitle(currentStatus);
        me.chatUserBannedGridOption.titleInfo.customButtons = '<span class="edit-action" id="cbBanAll"></span>';
        me.$chatUserGrid.show().fanexGrid(me.chatUserBannedGridOption);
        me.cacheGridElements();
        me.registerGridEvents();
    };
}

$(function () {
    'use strict';
    var chatUserBannerManagement = new ChatUserBannedManagement();
    chatUserBannerManagement.init();
});