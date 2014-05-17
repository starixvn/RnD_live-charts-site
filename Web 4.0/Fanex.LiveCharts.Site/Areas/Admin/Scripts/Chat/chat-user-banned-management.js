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
    };

    me.registerEvents = function () {
        me.$btnSearchChatUser.unbind('click').click(me.searchChatUser);
    };

    me.registerGridEvents = function () {
        //me.$cbBanAll.unbind('click').click(me.updateBanAllChatUsers);
        me.registerMenuAction();
        me.$cbBan.unbind('click').click(me.updateBanChatUser);
    };

    me.registerMenuAction = function () {
        $.contextMenu({
            selector: '#cbBanAll',
            trigger: 'left',
            autoHide: true,
            items: {
                "ban": { name: "Ban All", icon: "ban" },
                "unban": { name: "Un Ban All", icon: "un-ban" }
            },
            callback: function (key) {
                me.updateBanAllChatUsers(key === 'ban');
            }
        });
    };

    me.updateBanChatUser = function () {
        var $this = $(this);

        var chatUserId = $this.parent().parent().parent().attr('data-chat-user-id');

        me.banChatUsers(chatUserId, $this.prop('checked'));
    };

    me.updateBanAllChatUsers = function (isBanned) {
        var chatUserIds = [],
            chatUserLength = me.chatUsers.length;
        for (var i = 0; i < chatUserLength; i++) {
            chatUserIds.push(me.chatUsers[i].ChatUserId);
        }

        me.banChatUsers(chatUserIds.join(','), isBanned);
    };

    me.banChatUsers = function (chatUserIds, isBanned) {
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


    me.chatUserBannedGridOption = {
        columns: [
            {
                HeaderText: 'MemberName', Width: 305, Name: 'MemberName', CellAlign: 'center', AllowTruncate: true, CellCssClass: 'user-status'
            },
            { HeaderText: 'ChatName', Width: 400, Name: 'ChatName', CellAlign: 'center', AllowTruncate: true, CellCssClass: 'user-status-click-search' },
            {
                HeaderText: 'Banned',
                Width: 115,
                Name: 'ChatUserAction', 
                CellAlign: 'center',
                CustomCellRender: me.renderChatNameAction
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
        titleInfo: {}
    };

    me.getChatUserGridBodyRows = function (chatUsers) {
        var chatUserLength = chatUsers.length,
            bodyRows = [];
        for (var i = 0; i < chatUserLength; i++) {
            var currentChatUser = chatUsers[i];
            bodyRows.push({
                Columns: [
               { Name: "MemberName", Value: currentChatUser.MemberName },

               { Name: "ChatName", Value: currentChatUser.ChatName },
               {
                   Name: "ChatUserAction",
                   Value: currentChatUser.IsBanned
               }
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