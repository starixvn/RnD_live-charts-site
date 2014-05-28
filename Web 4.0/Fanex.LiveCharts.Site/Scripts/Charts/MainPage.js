/* File Created: March 28, 2014 */
/*global MAIN_PAGE_DEFAULT,LIVECHARTS*/
function MainPage() {
    'use strict';
    var me = this;

    me.cachedDomElements = function () {
        me.$parentFrmScorePredictionPage = $('#frmScorePredictionPage').parent();
        me.$parentFrmChatPage = $('#frmChatPage').parent();
        me.$FrmChatPage = $('#frmChatPage');
        me.$allFrame = $('#frmChartPage, #frmScorePredictionPage, #frmChatPage');
        me.$btnCollapseExpand = $('#submitPreditionValue');
        me.$reCheckInUrl = $('#reCheckInUrl');
    };

    me.init = function () {
        me.cachedDomElements();
        var clearBlockUI = false;
        try { $.blockUI(); } catch (e) { }
        me.$allFrame.load(function () {
            clearBlockUI = true;
            try { $.unblockUI(); } catch (e) { }
        });

        var blockUITimeOut = setTimeout(function () {
            if (!clearBlockUI) {
                try { $.unblockUI(); } catch (e) { }
            }

            clearTimeout(blockUITimeOut);
        }, 10000);


        me.$btnCollapseExpand.unbind('click').click(me.collapseExpand);
        me.reCheckIn();
    };

    me.collapseExpand = function () {
        var $this = $(this);
        var isExpand = $this.attr('data-expand');
        if (isExpand === 'true') {
            $this.attr('data-expand', 'false');
            me.$parentFrmScorePredictionPage.addClass('collapse');
            me.$parentFrmChatPage.addClass('expand');
            me.$btnCollapseExpand.addClass('expand');
            me.$FrmChatPage.contents().find('#chatConversation').addClass('expand').height(543);
            me.$FrmChatPage.contents().find('#messageInput').height(20);
            document.getElementById('frmChatPage').contentWindow.$('#roomContainer').height(567).trigger('resizableChatBox', [566, 450]);
        }
        else {
            $this.attr('data-expand', 'true');
            me.$parentFrmScorePredictionPage.removeClass('collapse');
            me.$parentFrmChatPage.removeClass('expand');
            me.$btnCollapseExpand.removeClass('expand');
            me.$FrmChatPage.contents().find('#messageInput').height(20);
            me.$FrmChatPage.contents().find('#chatConversation').removeClass('expand').height(262);
            document.getElementById('frmChatPage').contentWindow.$('#roomContainer').height(286).trigger('resizableChatBox', [285, 170]);
        }
    };

    me.reCheckIn = function () {
        setInterval(function () {
            LIVECHARTS.callAjax({
                url: MAIN_PAGE_DEFAULT.reCheckIn,
                type: 'POST',
                useBlockUI: false,
                data: {},
                done: function () {
                },
                fail: function (jqXHR) {
                    if (jqXHR.status === 401) {
                        window.location.href = MAIN_PAGE_DEFAULT.unAuthorizePage;
                    }
                }
            });
        }, 30000);
    };
}

$(function () {
    'use strict';
    var mainPage = new MainPage();
    mainPage.init();

});