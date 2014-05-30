/* File Created: May 30, 2014 */
/*global KICK_OUT_DEFUALT, LIVECHARTS*/
function KickOutPage() {
    'use strict';
    var me = this;

    me.init = function () {
        setInterval(me.checkReEnterPage, 30000);
    };

    me.checkReEnterPage = function () {
        LIVECHARTS.callAjax({
            url: KICK_OUT_DEFUALT.getInformationForReEnterUrl,
            type: 'POST',
            data: {},
            done: function (canReEnter) {
                if (canReEnter) {
                    location.href = KICK_OUT_DEFUALT.chatPage;
                }
            }
        });
    };
}


$(function () {
    'use strict';
    var kickOutPage = new KickOutPage();
    kickOutPage.init();
});