/* File Created: May 18, 2014 */
/*global MEMBER_BAN_BY_DEFAULT, LiveChartsAdmin*/
function MemberBanByDefault() {
    'use strict';
    var me = this;

    me.defaultValue = MEMBER_BAN_BY_DEFAULT;

    me.cachedElements = function () {
        me.$txtMemberSearch = $('#txtMemberSearch');
        me.$btnSearchMember = $('#btnSearchMember');
        me.$memberGrid = $('#memberGrid');
    };

    me.registerEvents = function () {
        me.$btnSearchMember.unbind('click').click(me.searchMember);
    };

//    me.registerGridEvents = function () { 
//    };

    me.init = function () {
        me.cachedElements();
        me.registerEvents();
    };

    me.searchMember = function () {
        LiveChartsAdmin.callAjax({
            url: me.defaultValue.searchMemberUrl,
            type: 'POST',
            data: {
                memberName: me.$txtMemberSearch.val(),
                chatName: me.$txtChatNameSearch.val(),
                chatUserStatus: me.$ddlChatUserStatus.val()
            },
            done: function (members) {
                me.members = members;
                me.renderMemberGrid(members);
            }
        });
    };
}

$(function () {
    'use strict';
    var memberBanByDefault = new MemberBanByDefault();
    memberBanByDefault.init();
});