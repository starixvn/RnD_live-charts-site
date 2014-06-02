/* File Created: May 30, 2014 */
/*global BAN_WORD_MANAGEMENT, LiveChartsAdmin, LANGUAGE*/
function BanWordManagement() {
    'use strict';
    var me = this;

    me.banWordDefault = BAN_WORD_MANAGEMENT;

    me.cachedElements = function () {
        me.$ddlStartWithGroups = $('#ddlStartWithGroups');
        me.$txtBanWord = $('#txtBanWord');
        me.$btnSearchBanWord = $('#btnSearchBanWord');
        me.$banWordGrid = $('#banWordGrid');
        me.$addNewBanWordPopUp = $('#addNewBanWordPopUp');
        me.$editBanWordPopUp = $('#editBanWordPopUp');
        me.$btnAddNewBanWord = $('#btnAddNewBanWord');
        me.$dialogs = $('#addNewBanWordPopUp', '#editBanWordPopUp');
        me.$banWordTextsAddNew = $('#banWordTextsAddNew');
        me.$banWordTextsUpdate = $('#banWordTextsUpdate');
        me.$btnSubmitAddNew = $('#btnSubmitAddNew');
        me.$btnSubmitEdit = $('#btnSubmitEdit');
        me.$banWordIdForUpdate = $('#banWordIdForUpdate');
        me.$ddlWordStatus = $('#ddlWordStatus');
    };

    me.registerDialogs = function () {
        me.$addNewBanWordPopUp.dialog({
            autoOpen: false,
            width: 'auto',
            modal: true,
            position: ['center', 160],
            title: 'Add New Banned Words'
        });

        me.$editBanWordPopUp.dialog({
            autoOpen: false,
            width: 'auto',
            modal: true,
            position: ['center', 160],
            title: 'Edit Ban Word'
        });
    };

    me.registerEvents = function () {
        me.registerDialogs();
        me.$btnSearchBanWord.unbind('click').click(me.searchBanWords);
        me.$btnAddNewBanWord.unbind('click').click(function () {
            me.$banWordTextsAddNew.val('');
            me.$addNewBanWordPopUp.dialog("open");
        });
        me.$btnSubmitAddNew.unbind('click').click(me.addNewBanWords);
        me.$btnSubmitEdit.unbind('click').click(me.updateBannedWord);
    };



    me.registerGridEvents = function () {
        $.contextMenu({
            selector: '#banWordGrid .open-menu-word',
            trigger: 'left',
            autoHide: true,
            build: function ($trigger/*, e*/) {
                var items = {},
                    currentStatus = $trigger.attr('data-word-status');
                switch (currentStatus) {
                    case 'deleted':
                        items.recoverThisWord = { name: LANGUAGE.recoverBannedWord, icon: "recover" };
                        items.updateThisWord = { name: LANGUAGE.updateBannedWord, icon: "update" };
                        break;
                    case 'avail':
                        items.removeThisWord = { name: LANGUAGE.deleteBannedWord, icon: "delete" };
                        items.updateThisWord = { name: LANGUAGE.updateBannedWord, icon: "update" };
                        break;
                }
                return {
                    callback: function (key/*, options*/) {
                        var $spanData = $(this);
                        switch (key) {
                            case 'recoverThisWord':
                                me.updateBannedWordStatus(false, $spanData.attr('data-word-id'));
                                break;
                            case 'removeThisWord':
                                me.updateBannedWordStatus(true, $spanData.attr('data-word-id'));
                                break;
                            case 'updateThisWord':
                                me.$banWordTextsUpdate.val($spanData.attr('data-word-text'));
                                me.$banWordIdForUpdate.val($spanData.attr('data-word-id'));
                                me.$editBanWordPopUp.dialog("open");
                                break;

                        }
                    },
                    items: items
                };
            }
        });


    };

    me.updateBannedWordStatus = function (isDelete, bannedWordId) {
        LiveChartsAdmin.callAjax({
            url: me.banWordDefault.updateBanWordStatusUrl,
            type: 'POST',
            data:
            {
                'delete': isDelete,
                'banWordId': bannedWordId
            },
            done: function (result) {
                if (result.IsSuccessful) {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
                    me.searchBanWords();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    me.updateBannedWord = function () {
        LiveChartsAdmin.callAjax({
            url: me.banWordDefault.updateBanWordTextUrl,
            type: 'POST',
            data:
            {
                banWordText: me.$banWordTextsUpdate.val(),
                banWordId: me.$banWordIdForUpdate.val()
            },
            done: function (result) {
                if (result.IsSuccessful) {
                    me.$editBanWordPopUp.dialog("close");
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
                    me.searchBanWords();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };

    me.addNewBanWords = function () {
        LiveChartsAdmin.callAjax({
            url: me.banWordDefault.insertBanWordsUrl,
            type: 'POST',
            data:
            {
                banWords: me.$banWordTextsAddNew.val()
            },
            done: function (result) {
                if (result.IsSuccessful) {
                    me.$addNewBanWordPopUp.dialog("close");
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.updateSuccess);
                    me.searchBanWords();
                }
                else {
                    LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                }
            }
        });
    };


    me.searchBanWords = function () {
        LiveChartsAdmin.callAjax({
            url: me.banWordDefault.getBanWordsUrl,
            type: 'POST',
            data:
            {
                startWith: me.$ddlStartWithGroups.val(),
                banWordText: me.$txtBanWord.val(),
                deleted: me.$ddlWordStatus.val()
            },
            done: function (banWords) {
                me.buildGrid(banWords);
            }
        });
    };

    me.renderWordDeleted = function (cell) {
        return '<span '
            + (cell.Value ? 'class="ico-tick"' : '')
            + '></span>';
    };

    me.renderWordAction = function (cell) {
        var iconHtml = '<span class="edit-action open-menu-word"'
                            + ' data-word-status="' + (cell.Value.Deleted ? 'deleted' : 'avail') + '"'
                            + ' data-word-id="' + cell.Value.BanWordId + '"'
                            + ' data-word-text="' + cell.Value.BanWordText + '"'
                        + '></span>';
        return iconHtml;
    };

    me.buildGrid = function (banWords) {
        var bodyRows = me.getBodyRows(banWords);

        me.banWordGridOption.bodyRows = bodyRows;
        me.$banWordGrid.show().fanexGrid(me.banWordGridOption);
        me.registerGridEvents();
    };

    me.getBodyRows = function (banWords) {
        var length = banWords.length,
            bodyRows = [];

        for (var i = 0; i < length; i++) {
            var currentWord = banWords[i];
            bodyRows.push({
                Columns: [
                   { Name: "No", Value: '' },
                   { Name: "BanWordText", Value: currentWord.BanWordText },
                   { Name: "UpdateTime", Value: currentWord.UpdateTime },
                   { Name: "Deleted", Value: currentWord.Deleted },
                   { Name: "Action", Value: currentWord }
                ]
            });
        }

        return bodyRows;
    };

    me.banWordGridOption =
    {
        columns: [
            { HeaderText: "#", Width: 60, Name: "No", CellAlign: 'center' },
            {
                HeaderText: 'Banned Word',
                Width: 350,
                Name: 'BanWordText',
                CellAlign: 'center',
                CellCssClass: 'user-status'
            },
            {
                HeaderText: 'Update Time (GMT+8)',
                Width: 150,
                Name: 'UpdateTime',
                CellAlign: 'center',
                FormatDateTime: 'DD MMM, YYYY hh:mm A'
            },
            {
                HeaderText: 'Deleted',
                Width: 100,
                Name: 'Deleted',
                CellAlign: 'center',
                CustomCellRender: me.renderWordDeleted
            },
            {
                HeaderText: 'Action',
                Width: 100,
                Name: 'Action',
                CellAlign: 'Center',
                CustomCellRender: me.renderWordAction
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
            text: 'Banned Words List'
        },
        showNumberColumn: true,
        timeZone: '+08:00'
    };

    me.init = function () {
        me.cachedElements();
        me.registerEvents();
    };
}

$(function () {
    'use strict';
    var banWord = new BanWordManagement();
    banWord.init();
});