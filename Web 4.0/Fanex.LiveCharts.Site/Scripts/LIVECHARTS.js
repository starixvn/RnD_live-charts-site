Object.keys = Object.keys || function (o) {
    'use strict';
    var result = [];
    for (var name in o) {
        if (o.hasOwnProperty(name))
        { result.push(name); }
    }
    return result;
};

'use strict';
var LIVECHARTS = {
    resolveUrl: function (url) {
        'use strict';
        var liveChartRoot = $('#LiveChartRoot');

        if (liveChartRoot.length === 0) {
            LIVECHARTS.console.error('"LiveChartRoot" does not exist! \
                                              Please add the following code to your page (or _Layout page): @Html.Hidden("LiveChartRoot", Url.Content("~/"))');
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
        'use strict';
        alert(LIVECHARTS.generalErrorMsg);
    },
    handleAjaxError: function (/*jqXHR, textStatus, errorThrown*/) {
        //LIVECHARTS.alertGeneralError();
    },
    console: {
        write: function (msg, consoleFunc) {
            'use strict';
            if (typeof console !== 'undefined') {
                consoleFunc(msg);
            } else {
                alert(msg);
            }
        },
        log: function (msg) { 'use strict'; LIVECHARTS.console.write(msg, console.log); },
        info: function (msg) { 'use strict'; LIVECHARTS.console.write(msg, console.info); },
        warn: function (msg) { 'use strict'; LIVECHARTS.console.write(msg, console.warn); },
        error: function (msg) { 'use strict'; LIVECHARTS.console.write(msg, console.error); }
    },
    messageType: {
        success: 0,
        error: 1
    },
    alertMessage: function (messageType, messageText, timeOut) {
        'use strict';
        if (LIVECHARTS.messageTimeOutId) {
            clearTimeout(LIVECHARTS.messageTimeOutId);
        }

        var $message = $('#messageNotification');
        $message.hide();
        var $body = $('body');
        if ($message.length === 0) {
            $message = $('<div id="messageNotification" style="display:none;"></div>').appendTo($body);
        }
        $message.empty();
        timeOut = timeOut || 3000;
        switch (messageType) {
            case LIVECHARTS.messageType.success:
                $message.removeClass('error-message').addClass('valid-message');
                break;
            case LIVECHARTS.messageType.error:
                $message.removeClass('valid-message').addClass('error-message');
                timeOut = 5000;
                break;
        }

        $message.html(messageText).show().css({
            left: ($body.width() / 2) - ($message.width() / 2 + 30) + 'px'
        });


        LIVECHARTS.messageTimeOutId = setTimeout(function () {
            $message.hide();
            clearTimeout(LIVECHARTS.messageTimeOutId);
        }, timeOut);
    },
    parseJSON: function (value) {
        'use strict';
        return $.parseJSON(value.replaceAll("'", "\""));
    },

    detectBrowser: function () {
        'use strict';
        var matchedBrowser, browserDectect;

        // Use of jQuery.browser is frowned upon.
        // More details: http://api.jquery.com/jQuery.browser
        // jQuery.uaMatch maintained for back-compat
        function ubMatch(ua) {
            ua = ua.toLowerCase();

            var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
                        /(webkit)[ \/]([\w.]+)/.exec(ua) ||
                        /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
                        /(msie) ([\w.]+)/.exec(ua) ||
                        ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
                        [];

            return {
                browserDectect: match[1] || "",
                version: match[2] || "0"
            };
        }

        matchedBrowser = ubMatch(navigator.userAgent);
        browserDectect = {};

        if (matchedBrowser.browserDectect) {
            browserDectect[matchedBrowser.browserDectect] = true;
            browserDectect.version = matchedBrowser.version;
        }

        // Chrome is Webkit, but Webkit is also Safari.
        if (browserDectect.chrome) {
            browserDectect.webkit = true;
        } else if (browserDectect.webkit) {
            browserDectect.safari = true;
        }
        return browserDectect;
    },
    htmlEncode: function (str) {
        'use strict';
        return $('<div/>').text(str).html();
    },
    htmlDecode: function (str) {
        'use strict';
        return $('<div/>').html(str).text();
    },
    cloneObject: function (obj) {
        'use strict';
        var newObj = (obj instanceof Array) ? [] : {};
        for (var i in obj) {
            if (i === 'clone') { continue; }
            if (obj[i] && typeof obj[i] === "object") {
                newObj[i] = LIVECHARTS.cloneObject(obj[i]);
            } else {
                newObj[i] = obj[i];
            }
        }
        return newObj;
    },
    callAjax: function (ajaxOption) {
        'use strict';
        if (ajaxOption.type === 'POST') {
            ajaxOption.data = JSON.stringify(ajaxOption.data);
        }

        if (typeof (ajaxOption.useBlockUI) === 'undefined') {
            ajaxOption.useBlockUI = true;
        }

        $.ajax({
            url: ajaxOption.url,
            type: ajaxOption.type,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ajaxOption.data,
            beforeSend: function () {
                if (ajaxOption.useBlockUI) {
                    try { $.blockUI(); } catch (e) { }
                }
            }
        }).done(function (data) {
            ajaxOption.done(data);
            if (ajaxOption.useBlockUI) {
                try { $.unblockUI(); } catch (e) { }
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            var $kickOut = $('#KickOutPage');
            if (jqXHR.status === 401 && $kickOut.length > 0) {
                window.location.href = $kickOut.val();
            }

            if (typeof (ajaxOption.fail) !== 'undefined') {
                ajaxOption.fail(jqXHR, textStatus, errorThrown);
            }
            if (ajaxOption.useBlockUI) {
                try { $.unblockUI(); } catch (e) { }
            }
            // Handle exception here
        }).always(function () {
            if (typeof (ajaxOption.always) !== 'undefined') {
                ajaxOption.always();
            }
        });
    }
};

/*jshint ignore:start*/
// JSON for old browsers
var JSON; if (!JSON) { JSON = {} } (function () { 'use strict'; function f(n) { return n < 10 ? "0" + n : n } if (typeof Date.prototype.toJSON !== "function") { Date.prototype.toJSON = function (key) { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z" : null }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) { return this.valueOf() } } var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, rep; function quote(string) { escapable.lastIndex = 0; return escapable.test(string) ? '"' + string.replace(escapable, function (a) { var c = meta[a]; return typeof c === "string" ? c : "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) }) + '"' : '"' + string + '"' } function str(key, holder) { var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === "object" && typeof value.toJSON === "function") { value = value.toJSON(key) } if (typeof rep === "function") { value = rep.call(holder, key, value) } switch (typeof value) { case "string": return quote(value); case "number": return isFinite(value) ? String(value) : "null"; case "boolean": case "null": return String(value); case "object": if (!value) { return "null" } gap += indent; partial = []; if (Object.prototype.toString.apply(value) === "[object Array]") { length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || "null" } v = partial.length === 0 ? "[]" : gap ? "[\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "]" : "[" + partial.join(",") + "]"; gap = mind; return v } if (rep && typeof rep === "object") { length = rep.length; for (i = 0; i < length; i += 1) { if (typeof rep[i] === "string") { k = rep[i]; v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } } else { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } } v = partial.length === 0 ? "{}" : gap ? "{\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "}" : "{" + partial.join(",") + "}"; gap = mind; return v } } if (typeof JSON.stringify !== "function") { JSON.stringify = function (value, replacer, space) { var i; gap = ""; indent = ""; if (typeof space === "number") { for (i = 0; i < space; i += 1) { indent += " " } } else { if (typeof space === "string") { indent = space } } rep = replacer; if (replacer && typeof replacer !== "function" && (typeof replacer !== "object" || typeof replacer.length !== "number")) { throw new Error("JSON.stringify") } return str("", { "": value }) } } if (typeof JSON.parse !== "function") { JSON.parse = function (text, reviver) { var j; function walk(holder, key) { var k, v, value = holder[key]; if (value && typeof value === "object") { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v } else { delete value[k] } } } } return reviver.call(holder, key, value) } text = String(text); cx.lastIndex = 0; if (cx.test(text)) { text = text.replace(cx, function (a) { return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) }) } if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) { j = eval("(" + text + ")"); return typeof reviver === "function" ? walk({ "": j }, "") : j } throw new SyntaxError("JSON.parse") } } } ());
/*jshint ignore:end*/