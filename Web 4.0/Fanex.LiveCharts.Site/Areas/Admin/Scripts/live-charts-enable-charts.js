/*global LiveChartsAdmin, LANGUAGE,GET_ENABLE_CHAT_DEFAULT*/
var EnableCharts = (function ($) {
    'use strict';
    return {
        datesArea: null,
        fromDate: null,
        toDate: null,
        statusId: null,
        dateButtonImage: null,

        init: function () {
            EnableCharts.datesArea = $('#datesArea');
            EnableCharts.fromDate = $('#FromDate');
            EnableCharts.toDate = $('#ToDate');
            EnableCharts.statusId = $('#StatusId');
            EnableCharts.messageInfo = $('#messageNotification');
            EnableCharts.showHideDateFields();
        },
        registerEventHandlers: function () {
            EnableCharts.statusId.on('change', EnableCharts.showHideDateFields);
            $('input.on-production-status').click(function () {
                var $this = $(this);
                $.ajax({
                    url: LiveChartsAdmin.resolveUrl('Match/UpdateChartShowingStatus'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        matchId: $this.attr("data-match-id"),
                        chartShowingStatus: $this.prop('checked') ? 1 : 0
                    }),
                    beforeSend: function () {
                        try { $.blockUI(); } catch (e) { }
                    }
                }).done(function (data) {
                    if (data && data.Code > 0) {
                        EnableCharts.messageInfo.empty()
                            .removeClass('valid-message').addClass('error-message')
                            .html(data.Message).show()
                            .delay(15000).fadeOut();
                    }
                    else {
                        EnableCharts.messageInfo.empty()
                            .removeClass('error-message').addClass('valid-message')
                            .html(EnableCharts.messageInfo.attr('data-success-message')).show()
                            .delay(3000).fadeOut();
                    }
                }).fail(function () {
                    //                    EnableCharts.messageInfo.empty()
                    //                            .removeClass('valid-message').addClass('error-message')
                    //                            .html(data.Message).show()
                    //                            .delay(15000).fadeOut();
                }).always(function () {
                    try { $.unblockUI(); } catch (e) { }
                });
            });

            $('input.cb-disable-chat').click(function (event) {
                var $this = $(this),
                    disable = $this.prop('checked');

                if (disable && !LiveChartsAdmin.confirmUpdateAction()) {
                    event.preventDefault();
                    return;
                }

                EnableCharts.updateChatRoomStatusInAdminSite($this.attr('data-match-id'), disable);

            });
        },
        updateChatRoomStatusInAdminSite: function (matchId, disable) {

            LiveChartsAdmin.callAjax({
                url: LiveChartsAdmin.resolveUrl('ChatAdmin/UpdateChatRoomStatusByMatchId'),
                type: 'POST',
                data: {
                    matchId: matchId,
                    disable: disable
                },
                done: function (updateChatRoomStatusViewModel) {
                    if (updateChatRoomStatusViewModel.CommandResult.IsSuccessful) {
                        if (disable) {
                            EnableCharts.kickOutChatUserWhenDisableRoom(updateChatRoomStatusViewModel);
                        }
                        else {
                            LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.successMessage);
                        }
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, updateChatRoomStatusViewModel.CommandResult.Message);
                    }
                }
            });
        },
        kickOutChatUserWhenDisableRoom: function (updateChatRoomStatusViewModel) {
            LiveChartsAdmin.callAjax({
                url: GET_ENABLE_CHAT_DEFAULT.memberChartSiteUrl + '/Chat/DisableChatRoom',
                type: 'POST',
                data: updateChatRoomStatusViewModel,
                done: function (result) {
                    if (result.IsSuccessful) {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.success, LANGUAGE.successMessage);
                    }
                    else {
                        LiveChartsAdmin.alertMessage(LiveChartsAdmin.messageType.error, result.Message);
                    }
                }
            });
        },
        initDateTimePicker: function () {
            EnableCharts.fromDate.datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 1,
                showOn: "button",
                buttonImage: EnableCharts.dateButtonImage,
                buttonText: "From Date",
                buttonImageOnly: true
            });
            EnableCharts.toDate.datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 1,
                showOn: "button",
                buttonImage: EnableCharts.dateButtonImage,
                buttonText: "To Date",
                buttonImageOnly: true
            });
        },
        showHideDateFields: function () {
            if (EnableCharts.statusId.val() === 'true') {
                EnableCharts.datesArea.attr('style', 'display: inline;');
                EnableCharts.datesArea.fadeIn();
            } else {
                EnableCharts.datesArea.fadeOut(100);
            }
        }
    };
})(jQuery);

$(document).ready(function () {
    'use strict';
    EnableCharts.init();
    EnableCharts.dateButtonImage = $('#dateImagePath').val();
    EnableCharts.initDateTimePicker();
    EnableCharts.registerEventHandlers();
});