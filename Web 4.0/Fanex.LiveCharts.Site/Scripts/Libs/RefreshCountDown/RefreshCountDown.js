/// <reference path="../jquery-1.9.1.js" />
(function ($) {

    //// Method expose for user use
    var methods =
    {
        createRefreshCountDown: function (opts) {
            opts = $.extend(true, {}, $.fn.refreshCountDown.defaults, opts);
            return this.each(function () {
                return new Fanex.RefreshCountDown(this, opts);
            });
        },
        stopCountDown: function (opts) {
            var id = this.attr('id');
            if (Fanex.RefreshCountDown.listInterval[id]) {
                clearInterval(Fanex.RefreshCountDown.listInterval[id]);
            }

            return $(this);
        }
    };

    //// Plugin
    $.fn.refreshCountDown = function (options) {
        if (methods[options]) {
            return methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof options === 'object' || !options) {
            return methods.createRefreshCountDown.apply(this, arguments);
        }
        else {
            throw 'Options cannot be null';
        }
    };

    //// Default Options
    $.fn.refreshCountDown.defaults =
    {
        countDownMinutes: 0,
        countDownSeconds: 60,
        refreshFunction: function () { return false; }
    };

    var Fanex = Fanex || {};

    Fanex.RefreshCountDown = function (refreshCountDown, options) {
        this.options = options;
        
        this.$refreshCountDown = $(refreshCountDown);
        this.id = refreshCountDown.id;
        this.calculateTotalTime();
        this.initialize();
        this.isProcessing = false;
        return this.$pager;
    };

    Fanex.RefreshCountDown.listInterval = {};

    Fanex.RefreshCountDown.prototype =
    {
        initialize: function () {
            this.render();
            this.startCountDown(this);
        },
        calculateTotalTime: function ()
        {
            this.sumCountDownSeconds = this.options.countDownMinutes * 60 + this.options.countDownSeconds;
        },
        render: function () {
            var me = this;
            var options = this.options;
            if (!me.$refreshCountDown.hasClass('refresh-countdown')) {
                me.$refreshCountDown.addClass('refresh-countdown');
            }

            me.$refreshCountDown.empty().append('<span class="loading-icon"></span>');
            me.$refreshCountDown.append('<span class="countdown-time">' + me.convertTimeToString(me.sumCountDownSeconds) + '</span>');

            me.$refreshCountDown.unbind('click').bind('click', function () {
                if (!me.isProcessing) {
                    me.isProcessing = !me.isProcessing;
                    me.resetCountDown();
                    me.isProcessing = !me.isProcessing;
                }
            });
        },
        convertTimeToString: function(seconds)
        {
            if (seconds == 0) {
                return seconds;
            }

            if (seconds <= 60) {
                return seconds > 9 ? seconds : '0' + seconds ;
            }

            var minute = Math.round((seconds - 30) / 60);
            if (minute < 10) {
                minute = '0' + minute;
            }

            var secondText = seconds % 60;
            if(secondText < 10)
            {
                secondText = '0' + secondText;
            }

            return minute + ':' + secondText;
        },
        startCountDown: function (currentCountDownObjects) {
            var me = currentCountDownObjects;
            var options = me.options;
            me.$refreshCountDown.find('.wait-loading-icon').addClass('loading-icon').removeClass('wait-loading-icon')
            clearInterval(Fanex.RefreshCountDown.listInterval[me.id]);
            Fanex.RefreshCountDown.listInterval[me.id] = setInterval(function () {
                me.sumCountDownSeconds--;
                me.$refreshCountDown.find('.countdown-time').html(me.convertTimeToString(me.sumCountDownSeconds));
                if (me.sumCountDownSeconds == 0) {
                    me.resetCountDown();
                }
            }, 1000);
        },
        resetCountDown: function () {
            var me = this;
            var options = me.options;

            me.calculateTotalTime();
            me.$refreshCountDown.find('.loading-icon').removeClass('loading-icon').addClass('wait-loading-icon');
            me.$refreshCountDown.find('.countdown-time').html('');
            options.refreshFunction(me, me.startCountDown);
        }
    };
})(jQuery);
