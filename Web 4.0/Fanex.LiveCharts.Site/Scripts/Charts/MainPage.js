/* File Created: March 28, 2014 */

function MainPage() {
    'use strict';
    var me = this;

    me.cachedDomElements = function () {
        me.$parentFrmScorePredictionPage = $('#frmScorePredictionPage').parent();
        me.$parentFrmChatPage = $('#frmChatPage').parent();
        me.$FrmChatPage = $('#frmChatPage');
        me.$allFrame = $('#frmChartPage, #frmScorePredictionPage, #frmChatPage');
        me.$btnCollapseExpand = $('#submitPreditionValue');
    };

    me.init = function () {
        me.cachedDomElements();
        try { $.blockUI(); } catch (e) { }
        me.$allFrame.load(function () {
            try { $.unblockUI(); } catch (e) { }
        });


        me.$btnCollapseExpand.unbind('click').click(me.collapseExpand);
    };

    me.collapseExpand = function () {
        var $this = $(this);
        var isExpand = $this.attr('data-expand');
        if (isExpand === 'true') {
            $this.attr('data-expand', 'false');
            me.$parentFrmScorePredictionPage.addClass('collapse');
            me.$parentFrmChatPage.addClass('expand');
            me.$btnCollapseExpand.addClass('expand');
            me.$FrmChatPage.contents().find('#chatConversation').addClass('expand');
        }
        else {
            $this.attr('data-expand', 'true');
            me.$parentFrmScorePredictionPage.removeClass('collapse');
            me.$parentFrmChatPage.removeClass('expand');
            me.$btnCollapseExpand.removeClass('expand');
            me.$FrmChatPage.contents().find('#chatConversation').removeClass('expand');
        }
    };
}

$(function () {
    'use strict';
    var mainPage = new MainPage();
    mainPage.init();

});