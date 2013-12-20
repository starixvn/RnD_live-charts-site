var MatchDetail = function ($) {
    return {
        eventList: null,
        eventForm: null,
        matchId: null,
        specialEvents: {},
        eventTypes: {},

        ddlHaves: null,
        txtRecordedTime: null,
        ddlEventTypes: null,
        ddlTeam: null,
        txtDetail: null,
        gridEvent: null,
        gridFooter: null,
        spEventId: null,
        errorSummary: null,
        successSummary: null,
        btnSubmit: null,
        btnDelete: null,
        btnCancel: null,

        url: {
            getEventUrl: LiveChartsAdmin.resolveUrl('Event/GetEvent'),
            getEventsUrl: LiveChartsAdmin.resolveUrl('Event/GetEvents'),
            getEventTypesUrl: LiveChartsAdmin.resolveUrl('Event/GetEventTypes'),
            updateEventUrl: LiveChartsAdmin.resolveUrl('Event/UpdateEvent'),
            insertEventUrl: LiveChartsAdmin.resolveUrl('Event/InsertEvent'),
            deleteEventUrl: LiveChartsAdmin.resolveUrl('Event/DeleteEvent'),
            updateOddsStatusUrl: LiveChartsAdmin.resolveUrl('Match/UpdateOddsListStatus'),
            getOddsListCriteria: LiveChartsAdmin.resolveUrl('Match/GetOddsList')
        },

        init: function () {
            MatchDetail.eventList = $('.grid');
            MatchDetail.matchId = $('#matchId').val();
            MatchDetail.ddlHaves = $('#ddlHaves');
            MatchDetail.txtRecordedTime = $('#txtRecordedTime');
            MatchDetail.ddlEventTypes = $('#ddlEventTypes');
            MatchDetail.ddlTeam = $('#ddlTeam');
            MatchDetail.txtDetail = $('#txtDetail');
            MatchDetail.gridEvent = $('#gridEvent');
            MatchDetail.gridFooter = $('#gridFooter');
            MatchDetail.spEventId = $('#spEventId');
            MatchDetail.errorSummary = $('#errorSummary ul');
            MatchDetail.successSummary = $('#successSummary');
            MatchDetail.errorUpdateOdds = $('#errorUpdateOdds ul');
            MatchDetail.successUpdateOdds = $('#successUpdateOdds');
            MatchDetail.eventForm = $('.filter-form');
            MatchDetail.btnSubmit = $('#btnSubmit');
            MatchDetail.btnDelete = $('#btnDelete');
            MatchDetail.btnCancel = $('#btnCancel');
            MatchDetail.ddlBetType = $('#ddlBetType');
            MatchDetail.cbLive = $('#cbLive');
            MatchDetail.oddsListContainer = $('#oddListContainer');
            MatchDetail.btnSubmitOddsChange = $('#btnSubmitOddsChange');
            MatchDetail.chartFrame = $('#frmChartContent');
            MatchDetail.bettypeCheckAll = $('.bettype-group input[type=checkbox]');
            MatchDetail.oddsStatus = $('.odds-item input.cb-odds');
        },
        loadEvents: function (params) {
            var ajaxRequest = $.ajax({
                url: params.url,
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: $.param(
                        {
                            matchId: params.matchId
                        })
            }).done(function (data) {
                MatchDetail.updateEventsTable(params, data);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                MatchDetail.handleAjaxException(jqXHR, textStatus, errorThrown);
            });

            return ajaxRequest;
        },
        updateEventsTable: function (params, data) {
            MatchDetail.gridEvent.find('tr').remove();
            MatchDetail.specialEvents = {};
            if (data.length > 0) {
                var cssClass;
                for (var i = 0; i < data.length; i++) {
                    cssClass = (i % 2 == 1) ? "class='alt'" : "";
                    MatchDetail.addEventTableRow(MatchDetail.gridEvent, data[i], cssClass);
                    if (data[i].EventTypeId > 200) {
                        MatchDetail.specialEvents[data[i].EventTypeId] = data[i];
                    }
                }
            }
        },
        addEventTableRow: function (tbody, data, cssClass) {
            var tr = "<tr {0}><td>";
//            if (data.EventTypeId == 201 || data.EventTypeId == 202 || data.EventTypeId == 203) {
//                tr += "{1}";
//            }
//            else {
                tr += "<a href='javascript:;' class='link-event-info'>{1}</a>";
            //}
            tr += "</td><td>{2}H {3}:{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>";
            tr = tr.format(cssClass, data.EventId, data.Half, data.Minute, data.Second, data.Name, data.TeamName, data.Detail);
            tbody.append(tr);
        },
        loadEventTypes: function (params) {
            var ajaxRequest = $.ajax({
                url: params.url,
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (data) {
                MatchDetail.updateEventTypeDropDown(data);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                MatchDetail.handleAjaxException(jqXHR, textStatus, errorThrown);
            });

            return ajaxRequest;
        },
        loadEvent: function (params) {
            var ajaxRequest = $.ajax({
                url: params.url,
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { id: params.id }
            }).done(function (data) {
                MatchDetail.updateEventForm(data);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                MatchDetail.handleAjaxException(jqXHR, textStatus, errorThrown);
            });
            return ajaxRequest;
        },
        updateEventForm: function (data) {
            MatchDetail.resetEventForm();
            MatchDetail.spEventId.text(data.EventId);
            MatchDetail.ddlHaves.val(data.Half);
            MatchDetail.ddlEventTypes.val(data.EventTypeId);
            MatchDetail.ddlTeam.val(data.TeamId);
            MatchDetail.txtDetail.val(data.Detail);
            MatchDetail.txtRecordedTime.val(data.Minute + ':' + data.Second);
            MatchDetail.btnDelete.removeAttr('disabled');
        },
        updateEventTypeDropDown: function (data) {
            if (data.length > 0) {
                MatchDetail.eventTypes = {};
                for (var i = 0; i < data.length; i++) {
                    MatchDetail.ddlEventTypes.append($('<option>', { value: data[i].EventTypeId, text: data[i].Name }));
                    MatchDetail.eventTypes[data[i].EventTypeId] = data[i];
                }
            }
        },
        updateOddsStatus: function () {
            var updateOddCriteria = {
                oddsEnableList: [],
                oddsDisableList: []
            };

            $('.cb-odds').each(function () {
                var $this = $(this);
                if ($this.is(':checked')) {
                    updateOddCriteria.oddsEnableList.push($this.val());
                }
                else {
                    updateOddCriteria.oddsDisableList.push($this.val());
                }
            });

            updateOddCriteria.oddsDisableList = updateOddCriteria.oddsDisableList.join(',');
            updateOddCriteria.oddsEnableList = updateOddCriteria.oddsEnableList.join(',');

            var ajaxRequest = $.ajax({
                url: MatchDetail.url.updateOddsStatusUrl,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(updateOddCriteria),
                beforeSend: function () {
                    try { $.blockUI(); } catch (e) { };
                }
            }).done(function (data) {
                if (data && data.Code > 0) {
                    MatchDetail.errorUpdateOdds.empty().html(data.Message);
                    MatchDetail.errorUpdateOdds.parent().show();
                }
                else {
                    MatchDetail.errorUpdateOdds.parent().hide();
                    MatchDetail.chartFrame.attr('src', MatchDetail.chartFrame.attr('src'));
                    MatchDetail.successUpdateOdds.fadeIn();
                    MatchDetail.successUpdateOdds.delay(3000).fadeOut();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                MatchDetail.handleAjaxException(jqXHR, textStatus, errorThrown);
            }).always(function () {
                try { $.unblockUI(); } catch (e) { };
            });
            return ajaxRequest;
        },
        getOddsList: function () {
            var ajaxRequest = $.ajax({
                url: MatchDetail.url.getOddsListCriteria,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    MatchId: $('#matchId').val(),
                    BetTypeId: MatchDetail.ddlBetType.find('option:selected').val(),
                    IsLive: MatchDetail.cbLive.is(':checked')
                }),
                beforeSend: function () {
                    try { $.blockUI(); } catch (e) { };
                }
            }).done(function (data) {
                if (data && data.length > 0) {
                    var html = [];
                    for (var i = 0; i < data.length; i++) {
                        var currentOdds = data[i];
                        html.push('<div class="lbl-field width-auto"><label for="odds' + currentOdds.Id + '">');
                        html.push('<input class="cb-odds" type="checkbox" id="odds' + currentOdds.Id + '" value="' + currentOdds.Id + '" ' + (currentOdds.ChartStatus ? 'checked="checked"' : '') + ' />');
                        html.push(currentOdds.Id + ' - ' + (currentOdds.AwayHandicap > 0 ? currentOdds.AwayHandicapString : currentOdds.HomeHandicapString) + '</label></div>');
                    }

                    MatchDetail.oddsListContainer.empty().html(html.join(''));
                    MatchDetail.btnSubmitOddsChange.show();
                }
                else {
                    MatchDetail.btnSubmitOddsChange.hide();
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                MatchDetail.handleAjaxException(jqXHR, textStatus, errorThrown);
            }).always(function () {
                try { $.unblockUI(); } catch (e) { };
            });
            return ajaxRequest;
        },
        updateEvent: function (params) {
            var ajaxRequest = $.ajax({
                url: params.url,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(params),
                beforeSend: function () {
                    try { $.blockUI(); } catch (e) { };
                }
            }).done(function (data) {
                MatchDetail.finishUpdatingEvent(data);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                MatchDetail.handleAjaxException(jqXHR, textStatus, errorThrown);
            }).always(function () {
                try { $.unblockUI(); } catch (e) { };
            });
            return ajaxRequest;
        },
        finishUpdatingEvent: function (data) {
            MatchDetail.successSummary.fadeIn();
            MatchDetail.successSummary.delay(3000).fadeOut();
            MatchDetail.loadEvents({ url: MatchDetail.url.getEventsUrl,
                matchId: MatchDetail.matchId
            });
            MatchDetail.resetEventForm();
            MatchDetail.chartFrame.attr('src', MatchDetail.chartFrame.attr('src'));
        },
        submitEvent: function () {
            if (!FormValidator.validate(MatchDetail.errorSummary, MatchDetail.eventForm))
                return false;
            var recordedTime = MatchDetail.txtRecordedTime.val();
            var eventId = MatchDetail.spEventId.text();
            var minute = null, second = null;
            var url = null;

            if (recordedTime !== 'undefined') {
                var delim = recordedTime.indexOf(':');
                minute = recordedTime.substr(0, delim);
                second = recordedTime.substr(delim + 1, recordedTime.length - 1);
            }

            if (eventId != '') {
                url = MatchDetail.url.updateEventUrl;
            } else {
                url = MatchDetail.url.insertEventUrl;
            }
            var params = {
                url: url,
                EventId: eventId,
                MatchId: MatchDetail.matchId,
                TeamId: MatchDetail.ddlTeam.val(),
                EventTypeId: MatchDetail.ddlEventTypes.val(),
                Half: MatchDetail.ddlHaves.val(),
                Minute: minute,
                Second: second,
                Detail: MatchDetail.txtDetail.val()
            };
            MatchDetail.updateEvent(params);
        },
        deleteEvent: function () {
            var eventId = MatchDetail.spEventId.text();
            var params = {
                url: MatchDetail.url.deleteEventUrl,
                EventId: eventId,
                IsDeleted: true
            };
            MatchDetail.updateEvent(params);
        },
        resetEventForm: function () {
            MatchDetail.ddlHaves.val('1');
            MatchDetail.ddlEventTypes.val('0');
            MatchDetail.ddlTeam.val('0');
            MatchDetail.txtRecordedTime.val('');
            MatchDetail.txtDetail.val('');
            MatchDetail.spEventId.text('');
            MatchDetail.btnSubmit.attr('disabled', 'true');
            MatchDetail.btnDelete.attr('disabled', 'true');
            FormValidator.clearErrorMsg(MatchDetail.errorSummary, MatchDetail.eventForm);
        },
        oddsStatusEvents: function () {
            MatchDetail.bettypeCheckAll.click(function () {
                var $this = $(this),
                    $allOddsItem = $this.parent().next().find('input.cb-odds');
                $allOddsItem.prop('checked', $this.prop('checked'));
            });

            MatchDetail.oddsStatus.click(function () {
                var $this = $(this),
                    $parent = $this.parent().parent().parent(),
                    $allChoices = $parent.find('input.cb-odds'),
                    $allChecked = $parent.find('input.cb-odds:checked'),
                    $betTypeCheckAll = $parent.prev().find('input[type=checkbox]');

                $betTypeCheckAll.prop('checked', $allChoices.length == $allChecked.length);

            });

        },
        bindEventHandlers: function () {
            MatchDetail.btnSubmit.on('click', MatchDetail.submitEvent);
            MatchDetail.btnDelete.on('click', MatchDetail.deleteEvent);
            MatchDetail.btnCancel.on('click', MatchDetail.resetEventForm);

            MatchDetail.ddlHaves.on('change', MatchDetail.enableSubmitButton);
            MatchDetail.txtRecordedTime.on('focus', MatchDetail.enableSubmitButton);
            MatchDetail.ddlEventTypes.on('change', MatchDetail.enableSubmitButton);
            MatchDetail.ddlTeam.on('change', MatchDetail.enableSubmitButton);
            MatchDetail.txtDetail.on('focus', MatchDetail.enableSubmitButton);

            MatchDetail.eventList.on('click', '.link-event-info', function (e) {
                var $this = $(this),
			    eventId = $this.text();
                MatchDetail.loadEvent({ url: MatchDetail.url.getEventUrl, id: eventId });
            });
            MatchDetail.cbLive.click(MatchDetail.getOddsList);
            MatchDetail.ddlBetType.change(MatchDetail.getOddsList);
            MatchDetail.btnSubmitOddsChange.click(MatchDetail.updateOddsStatus);
            MatchDetail.oddsStatusEvents();
        },
        enableSubmitButton: function () {
            MatchDetail.btnSubmit.removeAttr('disabled');
        },
        handleAjaxException: function (jqXHR, textStatus, errorThrown) {
            try { $.unblockUI(); } catch (e) { };
            if (textStatus != 'abort') {
                LiveChartsAdmin.handleAjaxError(jqXHR, textStatus, errorThrown);
            }
        }
    };
} (jQuery);

var FormValidator = function ($) {
    return {
        validate: function (errorSummary, form) {
            FormValidator.clearErrorMsg(errorSummary, form);

            form.find('.edit-field input[data-val-required]').each(function () {
                if ($(this).val() == '') {
                    $(this).attr('class', 'input-validation-error');
                    errorSummary.append($('<li>', { text: $(this).attr('data-val-required') }));
                }
            });

            form.find('.edit-field input[data-val-regex-pattern]').each(function () {
                var value = $(this).val();
                var regex = new RegExp($(this).attr('data-val-regex-pattern'));
                if (!regex.test(value)) {
                    $(this).attr('class', 'input-validation-error');
                    errorSummary.append($('<li>', { text: $(this).attr('data-val-regex') }));
                }
            });

            form.find('.edit-field select[data-val-required]').each(function () {
                if ($(this).val() == '0') {
                    $(this).attr('class', 'input-validation-error');
                    errorSummary.append($('<li>', { text: $(this).attr('data-val-required') }));
                }
            });

            FormValidator.validateTeamIdByEventType(errorSummary);
            FormValidator.validateHalfValueByEventType(errorSummary);
            FormValidator.validateDuplicateEventType(errorSummary);
            FormValidator.validateSpecialEventsOrder(errorSummary);

            if (errorSummary.find('li').length > 0) {
                errorSummary.parent().fadeIn();
                return false;
            } else {
                return true;
            }
        },
        validateTeamIdByEventType: function (errorSummary) {
            var eventTypeId = MatchDetail.ddlEventTypes.val();
            var teamId = MatchDetail.ddlTeam.val();
            if ((eventTypeId < 200) && (teamId == 0)) {
                MatchDetail.ddlTeam.attr('class', 'input-validation-error');
                errorSummary.append($('<li>', { text: MatchDetail.ddlTeam.attr('data-val-depends-on-event') }));
            }
        },
        validateHalfValueByEventType: function (errorSummary) {
            var eventId = MatchDetail.spEventId.text();
            var eventTypeId = MatchDetail.ddlEventTypes.val();

            if ((eventId == 0) && (eventTypeId > 200)) {
                var halfValue = MatchDetail.ddlHaves.val();

                if ((eventTypeId < 204) && (halfValue == 2)) {
                    MatchDetail.ddlHaves.attr('class', 'input-validation-error');
                    MatchDetail.ddlEventTypes.attr('class', 'input-validation-error');
                    errorSummary.append($('<li>', { text: '1H' + MatchDetail.ddlHaves.attr('data-val-value') }));
                } else if ((eventTypeId >= 204) && (halfValue == 1)) {
                    MatchDetail.ddlHaves.attr('class', 'input-validation-error');
                    MatchDetail.ddlEventTypes.attr('class', 'input-validation-error');
                    errorSummary.append($('<li>', { text: '2H' + MatchDetail.ddlHaves.attr('data-val-value') }));
                }
            }
        },
        validateDuplicateEventType: function (errorSummary) {
            var eventTypeId = MatchDetail.ddlEventTypes.val();
            var eventId = MatchDetail.spEventId.text();
            if ((MatchDetail.specialEvents[eventTypeId] != null)
                && (MatchDetail.specialEvents[eventTypeId].EventId != eventId)) {
                MatchDetail.ddlEventTypes.attr('class', 'input-validation-error');
                errorSummary.append($('<li>', { text: MatchDetail.ddlEventTypes.attr('data-val-duplicate-event') }));
            }
        },
        validateSpecialEventsOrder: function (errorSummary) {
            var eventTypeId = MatchDetail.ddlEventTypes.val();
            if (eventTypeId > 201) {
                eventTypeId = (eventTypeId < 205) ? eventTypeId : 204;

                for (var index = 201; index < eventTypeId; index++) {
                    if (MatchDetail.specialEvents[index] == null) {
                        MatchDetail.ddlEventTypes.attr('class', 'input-validation-error');
                        errorSummary.append($('<li>', { text: MatchDetail.eventTypes[index].Name + MatchDetail.ddlEventTypes.attr('data-val-special-events-order') }));
                        break;
                    }
                }
            }
        },
        clearErrorMsg: function (errorSummary, form) {
            form.find('.edit-field .input-validation-error').each(function () {
                $(this).removeAttr('class');
            });

            errorSummary.parent().fadeOut('fast');
            errorSummary.find('li').remove();
        }
    }
} (jQuery);

$(document).ready(function () {
    MatchDetail.init();
    LiveChartsAdmin.setPlaceHolder();
    MatchDetail.bindEventHandlers();
    MatchDetail.loadEvents({
        url: MatchDetail.url.getEventsUrl,
        matchId: MatchDetail.matchId
    });
    MatchDetail.loadEventTypes({ url: MatchDetail.url.getEventTypesUrl });
    MatchDetail.resetEventForm();
    //MatchDetail.getOddsList();
});