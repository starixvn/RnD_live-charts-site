var EnableCharts = function ($) {
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
                        try { $.blockUI(); } catch (e) { };
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
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    EnableCharts.messageInfo.empty()
                            .removeClass('valid-message').addClass('error-message')
                            .html(data.Message).show()
                            .delay(15000).fadeOut();
                }).always(function () {
                    try { $.unblockUI(); } catch (e) { };
                });
            });
        },
        initDateTimePicker: function () {
            EnableCharts.fromDate.datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 1,
                //dateFormat: "dd/MM/yyyy",
                showOn: "button",
                buttonImage: EnableCharts.dateButtonImage,
                buttonText: "From Date",
                buttonImageOnly: true
            });
            EnableCharts.toDate.datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 1,
                //dateFormat: "dd/MM/yyyy",
                showOn: "button",
                buttonImage: EnableCharts.dateButtonImage,
                buttonText: "To Date",
                buttonImageOnly: true
            });
        },
        showHideDateFields: function () {
            if (EnableCharts.statusId.val() == 'true') {
                EnableCharts.datesArea.attr('style', 'display: inline;');
                EnableCharts.datesArea.fadeIn();
            } else {
                EnableCharts.datesArea.fadeOut(100);
            }
        }
    }
} (jQuery);

$(document).ready(function () {
    EnableCharts.init();
    EnableCharts.dateButtonImage = $('#dateImagePath').val();
    EnableCharts.initDateTimePicker();
    EnableCharts.registerEventHandlers();
});