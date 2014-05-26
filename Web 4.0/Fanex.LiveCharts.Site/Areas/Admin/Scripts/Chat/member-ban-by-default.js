/* File Created: May 18, 2014 */
/*global MEMBER_BAN_BY_DEFAULT, LANGUAGE, LiveChartsAdmin*/
function MemberBanByDefault() {
    'use strict';
    var me = this;

    me.defaultValue = MEMBER_BAN_BY_DEFAULT;
    me.database = 
    {
        all: 0,
        mainDb: 1,
        lcc: 2
    };

    me.cachedElements = function () {
        me.$txtMemberSearch = $('#txtMemberSearch');
        me.$btnSearchMember = $('#btnSearchMember');
        me.$dllSearchDb = $('#dllSearchDb');
        me.$memberGrid = $('#memberGrid');
    };

    me.cacheGridElements = function () {
        me.$cbBanAll = $('#cbBanAll');
        me.$cbBan = $('#memberGrid .banned-action');
    };

    me.registerEvents = function () {
        me.$btnSearchMember.unbind('click').click(me.searchMember);
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
                me.updateBanAllMemberss(key === 'ban');
            }
        });
    };

    me.updateBanAllMemberss = function (isBanned) {
        var memberIds = [],
            memberLength = me.members.length;
        for (var i = 0; i < memberLength; i++) {
            memberIds.push(me.members[i].MemberId);
        }

        me.banMembers(memberIds.join(','), isBanned);
    };

    me.updateBanMember = function (event) {
        
        var $this = $(this);
        if ($this.prop('checked')) {
            event.preventDefault();
        }

        var memberId = $this.parent().parent().parent().attr('data-member-id');

        me.banMembers(memberId, $this.prop('checked'));
    };

    me.registerGridEvents = function () {
        me.registerMenuAction();
        me.$cbBan.unbind('click').click(me.updateBanMember);
    };

    me.init = function () {
        me.cachedElements();
        me.registerEvents();
    };

    me.banMembers = function (memberIds, isBanned) {
        if (isBanned && !LiveChartsAdmin.confirmUpdateAction()) {
            return;
        }

        LiveChartsAdmin.callAjax({
            url: me.defaultValue.updateBanMemberUrl,
            type: 'POST',
            data: {
                memberIds: memberIds,
                isBanned: isBanned,
                inDatabase: me.currentDb
            },
            done: function (commandResult) {
                if (commandResult.IsSuccessful) {
                    me.searchMember();
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, commandResult.Message);
                }
               
            }
        });
    };

    me.searchMember = function () {
        me.currentDb = parseInt(me.$dllSearchDb.val());
        me.currentMemberName = me.$txtMemberSearch.val();

        if (me.currentDb === me.database.mainDb) {
            if (me.currentMemberName === '') {
                LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, LANGUAGE.searchInMainDbWarning);
                return;
            }

            if (me.currentMemberName && me.currentMemberName.length < 3) {
                LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, LANGUAGE.searchInMainDBCannot);
                return;
            }

            if (me.currentMemberName.indexOf('%') !== -1 && me.currentMemberName.indexOf('%') < 3) {
                LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, LANGUAGE.searchCharacterIsNotValid);
                return;
            }
        }

        LiveChartsAdmin.callAjax({
            url: me.defaultValue.searchMemberUrl,
            type: 'POST',
            data: {
                memberName: me.$txtMemberSearch.val(),
                inDatabase: me.currentDb
            },
            done: function (members) {
                me.members = members;
                me.renderMemberGrid(members);
            }
        });
    };

    me.getMemberGridBodyRows = function (members) {
        var memberLength = members.length,
            bodyRows = [];
        for (var i = 0; i < memberLength; i++) {
            var currentMember = members[i];
            bodyRows.push({
                Columns: [
                { Name: "No", Value: '' },
               { Name: "MemberName", Value: currentMember.MemberName },
               {
                   Name: "MemberAction",
                   Value: currentMember.IsBanned
               },
               { Name: "EnteredChatRoom", Value: currentMember.ChatUserId },
               { Name: "BannedInLiveChartSite", Value: currentMember.IsBannedLccStatus }
                ],
                CustomAttr: 'data-member-id="' + currentMember.MemberId + '"'
            });
        }

        return bodyRows;
    };

    me.renderMemberNameAction = function (cell) {
        return '<input type="checkbox" class="banned-action" '
            + (cell.Value ? 'checked' : '')
            + ' />';
    };

    me.renderChatUserEntered = function (cell) {
        return '<span '
            + (cell.Value > 0 ? 'class="ico-tick"' : '')
            + '></span>';
    };

    me.renderBannedInLcc = function (cell) {
        return '<span '
            + (cell.Value ? 'class="ico-tick"' : '')
            + '></span>';
    };

    me.memberBannedGridOption = {
        columns: [
            { HeaderText: "#", Width: 60, Name: "No", CellAlign: 'center' },
            {
                HeaderText: 'MemberName', Width: 400, Name: 'MemberName', CellAlign: 'center', AllowTruncate: true, CellCssClass: 'user-status'
            },
            {
                HeaderText: 'Banned Default',
                Width: 100,
                Name: 'MemberAction',
                CellAlign: 'center',
                CustomCellRender: me.renderMemberNameAction
            },
            {
                HeaderText: 'Already Entered LiveChart Site',
                Width: 100,
                Name: 'EnteredChatRoom',
                CellAlign: 'center', 
                CustomCellRender: me.renderChatUserEntered
            },
            {
                HeaderText: 'Banned In LiveChart Site', Width: 100, Name: 'BannedInLiveChartSite', CellAlign: 'center', CustomCellRender: me.renderBannedInLcc
            }
        ],
        gridExpandHeight: 215,
        showPager: true,
        pagerOption: {
            itemsPerPage: 50,
            currentPage: 0,
            itemsPerPageArray: [50, 100, 500],
            showDetail: true,
            afterPagedFunction: me.registerGridEvents
        },
        showTitle: true,
        titleInfo: {
            text: 'Member List'
        },
        showNumberColumn: true
    };

    me.renderMemberGrid = function (members) {
        var bodyRows = me.getMemberGridBodyRows(members);
        me.memberBannedGridOption.bodyRows = bodyRows;

        me.memberBannedGridOption.pagerOption.currentPage = 0;
        me.memberBannedGridOption.titleInfo.customButtons = '<span class="edit-action" id="cbBanAll"></span>';
        me.$memberGrid.show().fanexGrid(me.memberBannedGridOption);
        me.cacheGridElements();
        me.registerGridEvents();
    };
}

$(function () {
    'use strict';
    var memberBanByDefault = new MemberBanByDefault();
    memberBanByDefault.init();
});