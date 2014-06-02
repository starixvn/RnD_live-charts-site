/* File Created: May 30, 2014 */
/*global BAN_WORD_MANAGEMENT, LiveChartsAdmin*/
function BanWordManagement() {
    'use strict';
    var me = this;

    me.banWordDefault = BAN_WORD_MANAGEMENT;

    me.cachedElements = function () {
        me.$ddlStartWithGroups = $('#ddlStartWithGroups');
        me.$txtBanWord = $('#txtBanWord');
        me.$btnSearchBanWord = $('#btnSearchBanWord');
        me.$banWordGrid = $('#banWordGrid');
    };

    me.registerEvents = function () { 
        
    };

    me.registerGridEvents = function () { 
        
    };

    me.searchBanWords = function () {
        LiveChartsAdmin.callAjax({
            url: me.banWordDefault.getBanWordsUrl,
            type: 'POST',
            data: 
            {
                startWith: me.$ddlStartWithGroups.val(),
                banWordText: me.$txtBanWord.val()
            },
            done: function () {
            }
        });
    };

//    me.getBodyRows = function (banWords) {
//        var length = banWords.length,
//            bodyRows = [];

//        for (var i = 0; i < length; i++) {
//            var currentWord = banWords[i];
//            bodyRows.push({
//                Columns: []
//            });
//        }

//    };

    me.banWordGridOption =
    {
        columns: [
            { HeaderText: "#", Width: 60, Name: "No", CellAlign: 'center' },
            {
                HeaderText: 'Banned Word',
                Width: 400,
                Name: 'BanWordText',
                CellAlign: 'center',
                CellCssClass: 'user-status'
            },
            {
                HeaderText: 'Update Time',
                Width: 100,
                Name: 'UpdateTime',
                CellAlign: 'center'
            },
            {
                HeaderText: 'Deleted',
                Width: 100,
                Name: 'Deleted',
                CellAlign: 'center'
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
            text: ''
        },
        showNumberColumn: true
    };

    me.init = function () {
        me.cachedElements();
    };
}

$(function () {
    'use strict';
    var banWord = new BanWordManagement();
    banWord.init();
});