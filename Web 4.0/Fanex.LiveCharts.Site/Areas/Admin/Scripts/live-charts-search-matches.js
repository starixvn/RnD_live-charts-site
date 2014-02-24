var SearchMatches = function ($) {
    return {
        matchDate: null,
        sportId: null,
        leagueId: null,
        dateButtonImage: null,

        init: function () {
            SearchMatches.matchDate = $('#MatchDate');
            SearchMatches.sportId = $('#SportId');
            SearchMatches.leagueId = $('#LeagueId');
            SearchMatches.keyWord = $('#Keyword');
            SearchMatches.kickedOff = $('#KickedOff');
            SearchMatches.cbEnableMatch = $('#grid-user .enable-checkbox');
            SearchMatches.cbEnableShowing = $('#grid-user .enable-showing-status-checkbox');
            SearchMatches.btnSearchMatch = $('#btnSearchMatch');
            SearchMatches.messageInfo = $('#messageNotification');
        },
        registerEventHandlers: function () {
            SearchMatches.matchDate.on('change', SearchMatches.matchDateChange);
            SearchMatches.btnSearchMatch.click(function () {
                var searchMatchParams = {
                    date: SearchMatches.matchDate.val(),
                    sportId: 1,
                    leagueId: SearchMatches.leagueId.val() == '' ? 0 : SearchMatches.leagueId.val(),
                    Keyword: SearchMatches.keyWord.val(),
                    KickedOff: SearchMatches.kickedOff.is(':checked')
                };

                $.ajax({
                    url: LiveChartsAdmin.resolveUrl('Match/MatchList'),
                    type: 'GET',
                    data: searchMatchParams,
                    cache: false,
                    beforeSend: function () {
                        try { $.blockUI(); } catch (e) { };
                    }
                }).done(function (data) {
                    $('#grid-user').empty().html(data);
                    SearchMatches.enableChartEvent();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    SearchMatches.handleAjaxException(jqXHR, textStatus, errorThrown);
                }).always(function () {
                    try { $.unblockUI(); } catch (e) { };
                });

            });
            SearchMatches.enableChartEvent();
        },
        enableChartEvent: function () {
            $('#grid-user .enable-checkbox').unbind('click').click(function () {
                var $this = $(this);
                var currentMatchId = parseInt($this.val());
                var ajaxRequest = $.ajax({
                    url: LiveChartsAdmin.resolveUrl('Match/ImportMatchQueue'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        matchId: currentMatchId,
                        kickofftime: $this.attr('data-kick-off-time'),
                        chartShowingStatus: 1
                    }),
                    beforeSend: function () {
                        try { $.blockUI(); } catch (e) { };
                    }
                }).done(function (data) {
                    if (data && data.Code > 0) {
                        alert(data.Message);
                        $this.prop('checked', false);
                    }
                    else {
                        var $parent = $this.parent();
                        $this.hide();
                        $parent.find('.update-enable-success').show();
                        $parent.find('.kicked-off-text').hide();
                        $parent.parent().find('.match-value').hide();
                        $parent.parent().find('.link-enable-chart').show();

                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    SearchMatches.handleAjaxException(jqXHR, textStatus, errorThrown);
                }).always(function () {
                    try { $.unblockUI(); } catch (e) { };
                });
                return ajaxRequest;
            });

            $('#grid-user .enable-showing-status-checkbox').unbind('click').click(function () {
                var $this = $(this);
                var currentMatchId = parseInt($this.val());
                var ajaxRequest = $.ajax({
                    url: LiveChartsAdmin.resolveUrl('Match/UpdateChartShowingStatus'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        matchId: currentMatchId,
                        chartShowingStatus: $this.prop('checked') ? 1 : 0
                    }),
                    beforeSend: function () {
                        try { $.blockUI(); } catch (e) { };
                    }
                }).done(function (data) {
                    if (data && data.Code > 0) {
                        SearchMatches.messageInfo.empty()
                            .removeClass('valid-message').addClass('error-message')
                            .html(data.Message).show()
                            .delay(15000).fadeOut();
                    }
                    else {
                        SearchMatches.messageInfo.empty()
                            .removeClass('error-message').addClass('valid-message')
                            .html(SearchMatches.messageInfo.attr('data-success-message')).show()
                            .delay(3000).fadeOut();
                    }
                }).fail(function () {
//                    SearchMatches.messageInfo.empty()
//                            .removeClass('valid-message').addClass('error-message')
//                            .html(data.Message).show()
//                            .delay(15000).fadeOut();
                }).always(function () {
                    try { $.unblockUI(); } catch (e) { };
                });
                return ajaxRequest;
            });
        },
        initDateTimePicker: function () {
            $("#MatchDate").datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 1,
                //dateFormat: "MMM dd,yyyy",
                showOn: "button",
                buttonImage: SearchMatches.dateButtonImage,
                buttonText: "Match Date",
                buttonImageOnly: true
            });
        },
        matchDateChange: function () {
            var selectedDate = SearchMatches.matchDate.val();

            if (selectedDate) {
                var params = { url: 'Match/GetLeagues',
                    date: selectedDate,
                    sportId: SearchMatches.sportId.val()
                };
                SearchMatches.loadLeagues(params);
            }
        },
        loadLeagues: function (params) {
            var ajaxRequest = $.ajax({
                url: LiveChartsAdmin.resolveUrl(params.url),
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    date: params.date,
                    sportId: params.sportId
                }),
                beforeSend: function () {
                    try { $.blockUI(); } catch (e) { };
                }
            }).done(function (data) {
                SearchMatches.updateDdlLeagues(data);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                SearchMatches.handleAjaxException(jqXHR, textStatus, errorThrown);
            }).always(function () {
                try { $.unblockUI(); } catch (e) { };
            });
            return ajaxRequest;
        },
        updateDdlLeagues: function (data) {
            SearchMatches.leagueId.find('option').remove();
            for (var i = 0; i < data.length; i++) {
                SearchMatches.leagueId.append($('<option>', { value: data[i].Value, text: data[i].Text }));
            }
        },
        handleAjaxException: function (jqXHR, textStatus, errorThrown) {
            try { $.unblockUI(); } catch (e) { };
            if (textStatus != 'abort') {
                LiveChartsAdmin.handleAjaxError(jqXHR, textStatus, errorThrown);
            }
        }
    }
} (jQuery);

$(document).ready(function () {
    SearchMatches.dateButtonImage = $('#dateImagePath').val();
    SearchMatches.initDateTimePicker();
    SearchMatches.init();
    SearchMatches.registerEventHandlers();
    SearchMatches.dateButtonImage.Time;
});