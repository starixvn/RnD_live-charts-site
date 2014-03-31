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

    me.handleBrowserStatus = function () {
        function onchange(evt) {
            var v = 'visible',
                h = 'hidden',
                evtMap = {
                    focus: v,
                    focusin: v,
                    pageshow: v,
                    blur: h,
                    focusout: h,
                    pagehide: h
                };

            evt = evt || window.event;
            if (evt.type in evtMap) {
                document.title = evtMap[evt.type];
            }
            else {
                document.title = this[hidden] ? "hidden" : "visible";
            }
        }

        var hidden = "hidden";

        // Standards:
        if (hidden in document) {
            document.addEventListener("visibilitychange", onchange);
        }
        else if ((hidden = "mozHidden") in document) {
            document.addEventListener("mozvisibilitychange", onchange);
        }
        else if ((hidden = "webkitHidden") in document) {
            document.addEventListener("webkitvisibilitychange", onchange);
        }
        else if ((hidden = "msHidden") in document) {
            document.addEventListener("msvisibilitychange", onchange);
        }
        // IE 9 and lower:
        else if ('onfocusin' in document) {
            document.onfocusin = document.onfocusout = onchange;
        }
        // All others:
        else {
            window.onpageshow = window.onpagehide
            = window.onfocus = window.onblur = onchange;
        }
    };
}

$(function () {
    'use strict';
    var mainPage = new MainPage();
    mainPage.init();

});