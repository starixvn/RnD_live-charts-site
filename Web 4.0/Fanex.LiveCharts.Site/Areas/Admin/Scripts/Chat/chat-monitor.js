/* File Created: April 1, 2014 */
/*global CHAT_MONITOR_DEFAULT, LiveChartsAdmin*/
function ChatMonitor() {
    'use strict';
    var me = this;
    me.chatMonitorDefault = CHAT_MONITOR_DEFAULT;
    me.pager = {
        pageSize: 50,
        pageIndex: 0
    };

    me.cacheDomElements = function () {
        me.$messageGrid = $('#messageGrid');
    };

    me.initMessageGrid = function () {
        me.bodyRows = [];
        LiveChartsAdmin.callAjax({
            url: me.chatMonitorDefault.getChatMessagesUrl,
            type: 'POST',
            data: {
                chatRoomId: me.chatMonitorDefault.chatRoomId
            },
            done: function (chatMessages) {
                me.gridOptions.bodyRows = me.buildGridBody(chatMessages);
                me.$messageGrid.fanexGrid(me.gridOptions);
            }
        });
        
    };

    me.gridOptions = {
        columns: [
                   { HeaderText: 'Time (GMT+8)', Width: 150, Name: 'Time', CellAlign: 'center', FormatDateTime: 'D MM, YYYY hh:mm A' },
                   { HeaderText: 'Username', Width: 150, Name: 'UserName', CellAlign: 'center', CustomCellRender: function (cell) {
                       return '<div class="user-name-content"><span class="remove-action remove-chat-user" data-member-id="' + cell.Value.MemberId + '"></span>' + cell.Value.MemberId + '</div>';
                   }
                   },
                   { HeaderText: 'Chat Name', Width: 150, Name: 'ChatName', CellAlign: 'center' },
                   { HeaderText: 'Chat Message', Width: 490, Name: 'ChatMessage', CustomCellRender: function (cell) {
                       return '<div class="message-container"><span class="remove-action remove-chat-message" data-chat-message-id="'
                                + cell.Value.chatMessageId + '"></span><div class="message-content">'
                                + cell.Value.content + '</div></div>'
                   }
                   }
               ],
        gridExpandHeight: 150,
        showPager: true,
        pagerOption: {
            itemsPerPage: 50,
            currentPage: 0,
            itemsPerPageArray: [50, 100, 500],
            showDetail: true
        },
        showTitle: true,
        titleInfo:
            {
                text: "Monitor Chat Conversation",
                showExcelButton: true,
                exportExcelFunction: function () {
                    alert("Export Excel Button Clicked");
                },
                customButtons: '<input type="button" id="btnReload" class="reload-button">',
                showButtonsWhenNoData: true
            },
        timeZone: '08:00'
    };

    me.buildGridBody = function (chatMessages) {
        var messagesLength = chatMessages.length;
        var bodyRows = [];
        for (var i = 0; i < messagesLength; i++) {
            var currentMessage = chatMessages[i];
            bodyRows.push({
                Columns: [
                   { Name: "Time", Value: currentMessage.CreateTime },
                   {
                       Name: "UserName",
                       Value: currentMessage.ChatUser
                   },
                   { Name: "ChatName", Value: currentMessage.ChatUser.ChatName },
                   {
                       Name: "ChatMessage",
                       Value:
                       { 
                         content: currentMessage.Content,
                         chatMessageId: currentMessage.ChatMessageId
                        }
                   }
                ],
                CustomAttr: 'data-chat-message-id="' + currentMessage.ChatMessageId + '"'
            });
        }

        return bodyRows;
    };

    me.init = function () {
        me.cacheDomElements();
        me.initMessageGrid();
    };
}


$(function () {
    'use strict';
    var chatMonitor = new ChatMonitor();
    chatMonitor.init();
});