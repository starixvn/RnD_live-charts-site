/*global Modernizr*/
if (typeof jQuery !== 'undefined') {
    // Functions in this namespace require jquery and jquery-ui
    var LiveChartsAdmin = (function ($) {
        'use strict';
        return {
            resolveUrl: function (url) {
                var liveChartRoot = $('#LiveChartRoot');

                if (liveChartRoot.length === 0) {
                    LiveChartsAdmin.console.error('"LiveChartRoot" does not exist! \
                                                    Please add the following code to your page (or _Layout page): \
                                                    @Html.Hidden("LiveChartRoot", Url.Content("~/"))');
                } else {
                    if (url.substr(0, 2) === '~/') {
                        url = url.substr(2);
                    } else if (url.substr(0, 1) === '/') {
                        url = url.substr(1);
                    }

                    return liveChartRoot.val() + url;
                }
            },
            generalErrorMsg: 'Your request is temporarily cannot be processed, please try again later!',
            alertGeneralError: function () {
                alert(LiveChartsAdmin.generalErrorMsg);
            },
            handleAjaxError: function () {
                //LiveChartsAdmin.alertGeneralError();
            },
            console: {
                write: function (msg, consoleFunc) {
                    if (typeof console !== 'undefined') {
                        consoleFunc(msg);
                    } else {
                        alert(msg);
                    }
                },
                log: function (msg) { LiveChartsAdmin.console.write(msg, console.log); },
                info: function (msg) { LiveChartsAdmin.console.write(msg, console.info); },
                warn: function (msg) { LiveChartsAdmin.console.write(msg, console.warn); },
                error: function (msg) { LiveChartsAdmin.console.write(msg, console.error); }
            },

            parseJSON: function (value) {
                return $.parseJSON(value.replaceAll("'", "\""));
            },
            htmlEncode: function (str) {
                return $('<div/>').text(str).html();
            },
            htmlDecode: function (str) {
                return $('<div/>').html(str).text();
            },

            cloneObject: function (obj) {
                var newObj = (obj instanceof Array) ? [] : {};
                for (var i in obj) {
                    if (i === 'clone') { continue; }
                    if (obj[i] && typeof obj[i] === "object") {
                        newObj[i] = Fanex.CloneObject(obj[i]);
                    } else {
                        newObj[i] = obj[i];
                    }
                }
                return newObj;
            },
            setPlaceHolder: function () {
                if (!Modernizr.input.placeholder) {
                    $('[placeholder]').focus(function () {
                        var input = $(this);
                        if (input.val() === input.attr('placeholder')) {
                            input.val('');
                            input.removeClass('placeholder');
                        }
                    }).blur(function () {
                        var input = $(this);
                        if (input.val() === '' || input.val() === input.attr('placeholder')) {
                            input.addClass('placeholder');
                            input.val(input.attr('placeholder'));
                        }
                    }).blur();
                    $('[placeholder]').parents('form').submit(function () {
                        $(this).find('[placeholder]').each(function () {
                            var input = $(this);
                            if (input.val() === input.attr('placeholder')) {
                                input.val('');
                            }
                        });
                    });
                }
            }
        };
    })(jQuery);
}

// -- Utilities -- //
/*jshint ignore:start */
// String Format
String.prototype.format = function () {
    'use strict';
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] !== 'undefined' ? args[number] : match;
    });
};

// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function (from, to) {
    'use strict';
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

// Array Remove All elements matched the criteria
Array.prototype.removeAll = function (criteria) {
    'use strict';
    var length = this.length;

    for (var i = 0; i < length; i++) {
        var element = this[i];

        if (criteria(element)) {
            this.splice(i--, 1);
            length--;
        }
    }
};
/*jshint ignore:end */