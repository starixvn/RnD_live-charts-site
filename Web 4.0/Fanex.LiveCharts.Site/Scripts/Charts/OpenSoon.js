/*global LIVECHARTS, totalMiliSecond*/
$(function () {
    'use strict';
    var note = $('#note'),
		ts = (new Date()).getTime() + totalMiliSecond;
    var startMatch = false;

    $('#countdown').countdown({
        timestamp: ts,
        callback: function (days, hours, minutes, seconds) {
            if (startMatch) {
                return;
            }

            var message = "";

            message += days + " day" + (days === 1 ? '' : 's') + ", ";
            message += hours + " hour" + (hours === 1 ? '' : 's') + ", ";
            message += minutes + " minute" + (minutes === 1 ? '' : 's') + " and ";
            message += seconds + " second" + (seconds === 1 ? '' : 's') + " <br />";
            note.html(message);

            if (days === 0 && hours === 0 && minutes === 0 && seconds === 0) {
                startMatch = true;
                window.location.href = window.location.href;
            }

        }
    });

    setInterval(function () {
        $.ajax({
            url: LIVECHARTS.resolveUrl('Charts/KeepAlive'),
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            data: ''
        }).done(function () {
        }).fail(function () {
        }).always(function () {
        });
    }, 60000);

});
