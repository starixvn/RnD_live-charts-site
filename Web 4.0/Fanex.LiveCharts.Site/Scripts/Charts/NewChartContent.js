/// <reference path="../../Views/Views/Shared/_Layout.cshtml" />
/* File Created: July 15, 2013 */
/// <reference path="../Libs/jquery-1.9.1.js" />
/// <reference path="../Libs/HighChart/highcharts.js" />
/// <reference path="Chart.js" />
/// <reference path="../LIVECHARTS.js" />
/// <reference path="../../Views/MovementCharts/ChartContent.cshtml" />

/*global BROWSER, CHART_COLOR, CRITERIA, DEFAULT_OBJECT, LANGUAGE, LIVECHARTS*/
/*export CHART*/


if (jQuery) {
    var Fanex = Fanex || {}; // jshint ignore:line

    Fanex.Chart = Fanex.Chart || {};
    var CHART;
    Fanex.ChartContent = (function () {
        'use strict';
        return {
            currentBrowser: BROWSER,
            currentLivePeriod: 0,
            constants: {
                getMissedPointInterval: 10000,
                getPointInterval: 60000,
                reCheckInInterval: 60000,
                matchStop: false,
                matchTime:
                {
                    endHaftTime: 45,
                    endSecondTime: 90
                },
                chartTime:
                {
                    startSecondTime: 60,
                    endSecondTime: 105
                }
            },
            oddsList: [],
            startUpdateData: false,
            timeStatus:
                {
                    first: {
                        value: 1,
                        index: -1
                    },
                    haftTime: {
                        value: 2,
                        index: -1
                    },
                    second: {
                        value: 3,
                        index: -1
                    },
                    extraTime: {
                        value: 4,
                        index: -1
                    },
                    over: {
                        value: 5,
                        index: -1
                    }
                },
            interval: {
                missedPrice: null,
                missedMarket: null,
                getPoints: null
            },
            refreshCountDownObject: null,
            step: {
                live: 1,
                deadball: 15
            },
            indicatorType:
                {
                    goal: {
                        value: 100,
                        image: DEFAULT_OBJECT.imageFolderUrl + 'Soccer_ball.png'
                    },
                    yellowCard: {
                        value: 101,
                        image: DEFAULT_OBJECT.imageFolderUrl + 'Yellow_card.png'
                    },
                    redCard: {
                        value: 102,
                        image: DEFAULT_OBJECT.imageFolderUrl + 'Red_card.png'
                    },
                    first: {
                        value: 201,
                        image: ''
                    },
                    hT: {
                        value: 202,
                        image: ''
                    },
                    second: {
                        value: 203,
                        image: ''
                    },
                    extraTime: {
                        value: 204,
                        image: ''
                    },
                    over: {
                        value: 205,
                        image: ''
                    }
                },
            lastTime: {
                market: 0,
                event: 0,
                timeLine: 0
            },
            ajaxObject: {
                priceChartRequest: [],
                marketChartRequest: null,
                indicatorRequest: null,
                getChartRequest: null,
                getMatchData: null,
                reCheckIn: null
            },
            betTypeEnum: {
                Handicap: 1,
                OverUnder: 3,
                OneXTwo: 5
            },
            teamChoosing: {
                home: 0,
                away: 1,
                homeAway: 2
            },
            cacheElements: function () {
                this.elements = {};
                //// Team element
                this.elements.$homeScore = $('#matchName #homeScore');
                this.elements.$awayScore = $('#matchName #awayScore');
                this.elements.$allTeam = $('#ddlTeamChoice .dropdown-menu a');
                this.elements.$teamChosen = $('#ddlTeamChoice .team-chosen');
                this.elements.$betTypeChosen = $('#ddlBetType .bettype-chosen');
                //// Bet type element
                this.elements.$betTypeGroup = $('#ddlBetType .dropdown-menu a');
                this.elements.$oddsChoiceGroup = $('#oddsChoice');

                //// Information Elements
                this.elements.$lastInformation = $('#lastInformation');
                this.elements.$homePercent = $('#lastInformation #homePercent');
                this.elements.$awayPercent = $('#lastInformation #awayPercent');
                this.elements.$currentHdpValue = $('#lastInformation #currentHdpValue');
                this.elements.$currentOddsValue = $('#lastInformation #currentOddsValue');
                this.elements.$currentTime = $('#matchName #currentTime');
                this.elements.$helpIcon = $('#helpIcon');

                //// Chart Information
                this.elements.$selectedHdpValue = $('#priceValue .hdp-value');
                this.elements.$selectedOddsValue = $('#priceValue .odds-value');
                this.elements.$selectedTimeValue = $('#priceValue .current-time');
                this.elements.$selectedMarketValue = $('#marketPercent .hdp-value');
                this.elements.$hideElementWhenStopMatch = $('#matchName #currentTime, #lastInformation, #matchName .icon-time');

                //// Chart Container
                this.elements.$priceChartContainer = $('#priceChartContainer');
                this.elements.$marketChartcontainer = $('#marketChartContainer');
                this.elements.$eventContainer = $('#eventContainer');
                this.elements.$chartContainers = $('#priceChartContainer, #marketChartContainer, #eventContainer');
                this.elements.$refreshCountDown = $('#refreshCountDown');


            },
            url: {
                getChartsUrl: LIVECHARTS.resolveUrl('api/ChartApi/GetCharts'),
                getSingleChartUrl: LIVECHARTS.resolveUrl('api/ChartApi/GetChart'),
                getMarketChartPoints: LIVECHARTS.resolveUrl('api/ChartApi/GetMarketChartPoints'),
                getPriceChartPoints: LIVECHARTS.resolveUrl('api/ChartApi/GetPriceChartPoints'),
                getCurrentMatchTime: LIVECHARTS.resolveUrl('api/ChartApi/GetCurrentMatchTime'),
                getOdds: LIVECHARTS.resolveUrl('api/ChartApi/GetOdds'),
                reCheckIn: LIVECHARTS.resolveUrl('User/ReCheckIn'),
                getIndicatorPoints: LIVECHARTS.resolveUrl('api/ChartApi/GetIndicatorPoints'),
                getShowBetType: LIVECHARTS.resolveUrl('Charts/ShowBetType'),
                getKickOffTime: LIVECHARTS.resolveUrl('api/ChartApi/GetKickOffTime'),
                unAuthorizedUrl: LIVECHARTS.resolveUrl('User/Unauthorized'),
                viewOneChartUrl: LIVECHARTS.resolveUrl('User/ShowOnlyOneMatch'),
                helpPageUrl: LIVECHARTS.resolveUrl('Introduction?key=Chart')
            },
            init: function () {
                if (CRITERIA.showChart) {
                    var self = this;
                    CRITERIA.defaultStopTime = CRITERIA.isLive ? DEFAULT_OBJECT.liveStopTime : DEFAULT_OBJECT.deadballStopTime;
                    CRITERIA.stopTime = CRITERIA.isLive ? DEFAULT_OBJECT.liveStopTime : DEFAULT_OBJECT.deadballStopTime;
                    this.cacheElements();
                    this.registerEvents();
                    this.reloadOdds();

                    setInterval(function () {
                        if (self.ajaxObject.reCheckIn != null) {
                            self.ajaxObject.reCheckIn.abort();
                        }

                        self.ajaxObject.reCheckIn = $.ajax({
                            url: self.url.reCheckIn,
                            type: 'POST',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({
                                matchId: CRITERIA.matchId
                            })
                        }).done(function (data) {
                            if (data != null) {
                                if (data.KickOut) {
                                    window.location.href = self.url.unAuthorizedUrl;
                                }

                                if (data.IsViewTwoChart) {
                                    window.location.href = self.url.viewOneChartUrl;
                                }
                            }
                        }).fail(function (jqXHR) {
                            if (jqXHR.status === 401) {
                                window.location.href = self.url.unAuthorizedUrl;
                            }
                        }).always(function () {
                        });
                    }, self.constants.reCheckInInterval);
                    self.getCurrentMatchData(self);
                    setInterval(function () {
                        self.getCurrentMatchData(self);
                    }, self.constants.getPointInterval);
                }
            },
            getCurrentMatchData: function (self) {
                if (!self.constants.matchStop) {
                    if (self.ajaxObject.getMatchData != null) {
                        self.ajaxObject.getMatchData.abort();
                    }

                    self.ajaxObject.getMatchData = $.ajax({
                        url: self.url.getCurrentMatchTime,
                        type: 'GET',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: $.param({
                            matchId: CRITERIA.matchId
                        })
                    }).done(function (data) {
                        if (data) {
                            switch (data.LivePeriod) {
                                case 0:
                                    if (self.currentLivePeriod === 1 || self.timeStatus.haftTime.index > 0) {
                                        self.elements.$currentTime.text('HT');
                                    }
                                    break;
                                case 1:
                                    self.currentLivePeriod = data.LivePeriod;
                                    if (data.CurrentMinute > self.constants.matchTime.endHaftTime) {
                                        self.elements.$currentTime.text(self.constants.matchTime.endHaftTime
                                            + "'+"
                                            + (data.CurrentMinute - self.constants.matchTime.endHaftTime) + "'");
                                    }
                                    else {
                                        self.elements.$currentTime.text(data.CurrentMinute + "'");
                                    }

                                    break;
                                case 2:
                                    self.currentLivePeriod = data.LivePeriod;
                                    if (data.CurrentMinute > self.constants.matchTime.endSecondTime) {
                                        self.elements.$currentTime.text(self.constants.matchTime.endSecondTime
                                            + "'+"
                                            + (data.CurrentMinute - self.constants.matchTime.endSecondTime) + "'");
                                    }
                                    else {
                                        self.elements.$currentTime.text(data.CurrentMinute + "'");
                                    }
                                    break;
                            }

                            self.elements.$homeScore.text(data.HomeScore);
                            self.elements.$awayScore.text(data.AwayScore);
                        }
                    });
                }
            },
            registerEvents: function () {
                var self = this;
                self.elements.$betTypeGroup.click(function () {
                    var $this = $(this);
                    if ($this.attr('data-selected') === 'true') {
                        return;
                    }

                    self.elements.$betTypeChosen.text($this.text());
                    self.elements.$betTypeGroup.attr('data-selected', false);
                    $this.attr('data-selected', true);

                    if (self.betTypeEnum.Handicap === parseInt($this.data('bettype'))) {
                        self.elements.$teamChosen.text(CRITERIA.homeName).css('color', CHART_COLOR.homeColor);
                        self.elements.$allTeam.filter('[data-teamid=0]').text(CRITERIA.homeName);
                        self.elements.$allTeam.filter('[data-teamid=1]').text(CRITERIA.awayName);
                    }
                    else {
                        self.elements.$teamChosen.text(LANGUAGE.over).css('color', CHART_COLOR.homeColor);
                        self.elements.$allTeam.filter('[data-teamid=0]').text(LANGUAGE.over);
                        self.elements.$allTeam.filter('[data-teamid=1]').text(LANGUAGE.under);
                    }

                    self.elements.$allTeam.attr('data-selected', false);
                    self.elements.$allTeam.filter('[data-teamid=0]').attr('data-selected', true);

                    self.reloadOdds();
                });

                self.elements.$allTeam.click(function () {
                    var $this = $(this);
                    if ($this.attr('data-selected') === 'true') {
                        return;
                    }
                    
                    self.elements.$teamChosen.text($this.text()).css('color', $this.css('color'));
                    self.elements.$allTeam.attr('data-selected', false);
                    $this.attr('data-selected', true);

                    if (CRITERIA.chooseHandicap) {
                        var chooseHome = parseInt($this.data('teamid')) === self.teamChoosing.home;

                        self.elements.$oddsChoiceGroup.find('span').each(function () {
                            var $this = $(this);
                            if (chooseHome) {
                                $this.text($this.attr('data-home-hdp'));
                            }
                            else {
                                $this.text($this.attr('data-away-hdp'));
                            }
                        });
                    }


                    self.reloadChart(true);
                });

                self.elements.$helpIcon.click(function () {
                    window.open(
                        self.url.helpPageUrl,
                        'LiveChartHelpPage', 
                        'width=660,height=640,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0,scrollbars=yes,resizable=no');
                });
            },
            reloadOdds: function () {
                this.updateCriteria();
                this.loadOdds(true);
            },
            reloadChart: function (partialLoad) {
                this.updateCriteria(partialLoad);
                this.loadChart(partialLoad);
            },
            updateCriteria: function (partialLoad) {
                //// Update Score and time index
                CRITERIA.currentHomeScore = CRITERIA.currentAwayScore = 0;
                CRITERIA.isLive = true;
                CRITERIA.interval = CRITERIA.isLive ? this.step.live : this.step.deadball;
                CRITERIA.startTime = CRITERIA.isLive ? DEFAULT_OBJECT.liveStartTime : DEFAULT_OBJECT.deadballStartTime;

                //// Update BetType choose
                var selectedBetType = this.elements.$betTypeGroup.filter('[data-selected=true]').data('bettype');
                CRITERIA.betTypeId = parseInt(selectedBetType);
                CRITERIA.chooseHandicap = CRITERIA.betTypeId === this.betTypeEnum.Handicap;
                CRITERIA.chooseOverUnder = CRITERIA.betTypeId === this.betTypeEnum.OverUnder;

                //// Update Team choose
                var selectedTeam = this.elements.$allTeam.filter('[data-selected=true]').data('teamid');
                CRITERIA.teamChoosing = parseInt(selectedTeam);
                CRITERIA.chooseHome = CRITERIA.teamChoosing === this.teamChoosing.home;
                CRITERIA.chooseAway = CRITERIA.teamChoosing === this.teamChoosing.away;
                CRITERIA.chooseAllTeam = CRITERIA.teamChoosing === this.teamChoosing.homeAway;

                this.updateFilter(partialLoad);
            },
            updateFilter: function (partialLoad) {
                var self = this;
                partialLoad = partialLoad || false;
                if (!partialLoad) {
                    self.lastTime.market = CRITERIA.startTime;
                    self.lastTime.timeLine = CRITERIA.startTime;
                    self.lastTime.event = CRITERIA.startTime;
                    //self.elements.$homeScore.text('0');
                    //self.elements.$awayScore.text('0')
                    self.elements.$refreshCountDown.empty();
                    //// Reset information
                    self.elements.$selectedHdpValue.empty();
                    self.elements.$selectedMarketValue.empty();
                    self.elements.$selectedOddsValue.empty();
                    self.elements.$selectedTimeValue.empty();
                    self.elements.$currentHdpValue.empty();
                    self.elements.$currentOddsValue.empty();
                    //self.elements.$currentTime.empty();
                    self.elements.$homePercent.css('width', '0%').empty();
                    self.elements.$awayPercent.css('width', '0%').empty();
                }
                self.oddsIds = [];
                self.elements.$oddsChoiceGroup.find('span[data-selected=true]').each(function () {
                    var $this = $(this);
                    var oddsId = $this.data('odds-id');
                    CRITERIA.oddsId = oddsId;
                    self.oddsIds.push(oddsId);
                    if (self.lastTime[oddsId]) { self.lastTime[oddsId] = CRITERIA.startTime; }
                });

                self.clearAllRequest();
            },
            loadChart: function (partialLoad) {
                var self = this;
                if (partialLoad) {
                    self.elements.$priceChartContainer.empty();
                    self.ajaxObject.getChartRequest = $.ajax({
                        url: self.url.getSingleChartUrl,
                        type: 'GET',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: $.param(
                        {
                            matchId: CRITERIA.matchId,
                            betTypeId: CRITERIA.betTypeId,
                            oddsId: CRITERIA.oddsId,
                            isLive: CRITERIA.isLive,
                            teamChoosing: CRITERIA.teamChoosing,
                            kickOffTime: CRITERIA.kickOffTime,
                            chartId: CHART.type.price
                        }),
                        beforeSend: function () {
                            try { $.blockUI(); } catch (e) { }
                        }
                    }).done(function (data) {
                        CHART.buildPriceChart(data);
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        self.handleAjaxException(jqXHR, textStatus, errorThrown);
                    }).always(function () {
                        try { $.unblockUI(); } catch (e) { }
                    });
                }
                else {
                    //// If no odds was chosen, then return and render empty.
                    if (self.oddsIds.length === 0) {
                        self.elements.$priceChartContainer.empty();
                        self.elements.$marketChartcontainer.empty();
                        self.elements.$eventContainer.empty();
                        return;
                    }

                    //// Get all chart equivalent with market and price chart choice.
                    self.ajaxObject.getChartRequest = $.ajax({
                        url: self.url.getChartsUrl,
                        type: 'GET',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: $.param(
                        {
                            matchId: CRITERIA.matchId,
                            betTypeId: CRITERIA.betTypeId,
                            oddsIds: self.oddsIds.join(','),
                            isLive: CRITERIA.isLive,
                            kickOffTime: CRITERIA.kickOffTime
                        }),
                        beforeSend: function () {
                            try { $.blockUI(); } catch (e) { }
                        }
                    }).done(function (data) {
                        CHART.build(data);
                        try { $.unblockUI(); } catch (e) { }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        try { $.unblockUI(); } catch (e) { }
                        self.handleAjaxException(jqXHR, textStatus, errorThrown);
                    }).always(function () {

                    });
                }
            },
            handleAjaxException: function (jqXHR, textStatus, errorThrown) {
                try { $.unblockUI(); } catch (e) { }
                if (textStatus !== 'abort') {
                    LIVECHARTS.handleAjaxError(jqXHR, textStatus, errorThrown);
                }
            },
            stopAjax: function () {
                //// Stop all ajax requests.
                var self = this;
                if (self.ajaxObject.marketChartRequest) { self.ajaxObject.marketChartRequest.abort(); }
                if (self.ajaxObject.getChartRequest) { self.ajaxObject.getChartRequest.abort(); }
                if (self.ajaxObject.priceChartRequest.length > 0) {
                    for (var i = 0; i < self.ajaxObject.priceChartRequest.length; i++) {
                        if (self.ajaxObject.priceChartRequest[i]) { self.ajaxObject.priceChartRequest[i].abort(); }
                    }
                    self.ajaxObject.priceChartRequest = [];
                }
                if (self.ajaxObject.indicatorRequest) { self.ajaxObject.indicatorRequest.abort(); }

            },
            clearAllRequest: function () {
                this.stopAjax();
                clearInterval(this.interval.getPoints);
            },
            loadOdds: function (initOdds) {
                var self = this, i;
                $.ajax({
                    url: self.url.getOdds,
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: $.param({
                        matchId: CRITERIA.matchId,
                        betTypeId: CRITERIA.betTypeId,
                        isLive: CRITERIA.isLive
                    }),
                    beforeSend: function () {
                        try { $.blockUI(); } catch (e) { }
                    }
                }).done(function (oddsArray) {
                    if (initOdds) {
                        self.elements.$oddsChoiceGroup.empty();
                        self.elements.$chartContainers.empty();

                        if (oddsArray && oddsArray.length > 0) {
                            var html = [];
                            CHART.oddsChartIndex = {};
                            self.oddsList = [];

                            for (i = 0; i < oddsArray.length; i++) {
                                var odds = oddsArray[i];
                                if (i === 0) { CRITERIA.oddsId = odds.Id; }

                                self.oddsList.push(odds.Id);
                                self.lastTime[odds.Id] = CRITERIA.startTime;

                                html.push(self.buildOddsChoice(i, odds));
                            }

                            //// Show odds, and bind events.
                            self.elements.$oddsChoiceGroup.html(html.join(''));
                            self.elements.$oddsChoiceGroup.find('span').unbind('click').click(function () {
                                var $this = $(this);
                                if ($this.attr('data-selected') === 'true') {
                                    return;
                                }
                                self.elements.$oddsChoiceGroup.find('span').removeClass('active-item').attr('data-selected', false);
                                $this.addClass('active-item').attr('data-selected', true);
                                self.reloadChart(true);
                            });
                            self.reloadChart();
                        }
                        else {
                            self.elements.$chartContainers.empty().html('<div class="no-infor">' + LANGUAGE.chartUnavai + '</div>');
                        }
                    }
                    else {
                        if (oddsArray && oddsArray.length > 0) {
                            //// Update new odds data.
                            for (i = 0; i < oddsArray.length; i++) {
                                self.updateOddsHandicap(oddsArray[i]);
                            }
                        }
                    }

                }).fail(function (jqXHR, textStatus, errorThrown) {
                    self.handleAjaxException(jqXHR, textStatus, errorThrown);
                }).always(function () {
                    try { $.unblockUI(); } catch (e) { }
                });
            },
            buildOddsChoice: function (index, odds) {
                var homeHdpValue = odds.AwayHandicap > 0 ? odds.AwayHandicap : (0 - odds.HomeHandicap),
                    awayHdpValue = odds.AwayHandicap > 0 ? (0 - odds.AwayHandicap) : odds.HomeHandicap;

                if (CRITERIA.chooseOverUnder) {
                    homeHdpValue = odds.HomeHandicap;
                    awayHdpValue = odds.AwayHandicap;
                }

                var html = '<span class="' + (index === 0 ? 'active-item' : '') + '"'
                    + ' data-odds-id="' + odds.Id + '" '
                    + ' data-home-hdp="' + homeHdpValue + '" '
                    + ' data-away-hdp="' + awayHdpValue + '" '
                    + 'data-selected="' + (index === 0 ? 'true' : 'false') + '">' + (CRITERIA.chooseHome ? homeHdpValue : awayHdpValue) + '</span>';
                return html;
            },
            updateOddsHandicap: function (odds) {
                var self = this;
                var homeHdpValue = odds.AwayHandicap > 0 ? odds.AwayHandicap : (0 - odds.HomeHandicap),
                    awayHdpValue = odds.AwayHandicap > 0 ? (0 - odds.AwayHandicap) : odds.HomeHandicap,
                    oddsValue = CRITERIA.chooseHome ? homeHdpValue : awayHdpValue;

                if (CRITERIA.chooseOverUnder) {
                    homeHdpValue = odds.HomeHandicap;
                    awayHdpValue = odds.AwayHandicap;
                    oddsValue = Math.max(homeHdpValue, awayHdpValue);
                }

                var currentOddsElement = self.elements.$oddsChoiceGroup.find('span[data-odds-id=' + CRITERIA.oddsId + ']');
                currentOddsElement.attr('data-home-hdp', homeHdpValue);
                currentOddsElement.attr('data-away-hdp', awayHdpValue);
                currentOddsElement.text(oddsValue);
            },
            checkKickOffTime: function () {
                var self = this;
                $.ajax({
                    url: self.url.getKickOffTime,
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: $.param({
                        matchId: CRITERIA.matchId
                    })
                }).done(function (kickOffTimeString) {
                    if (kickOffTimeString !== '' && kickOffTimeString !== CRITERIA.kickOffTime) {
                        window.location.href = window.location.href;
                    }
                });
            }
        };
    }) ();

    Fanex.Chart.Helper = (function () {
        'use strict';
        return {
            mainChart: Fanex.ChartContent,
            convertTimeLine: function (timeIndex) {
                if (timeIndex >= 0) {
                    return '';
                }
                else {
                    //// Format Hour for Deadball
                    var hours = Math.round((Math.abs(timeIndex) / 60) - 0.5);
                    if (hours < 10) { hours = '0' + hours; }
                    var minute = Math.round(Math.abs(timeIndex) % 60);
                    if (minute < 10) { minute = '0' + minute; }
                    return (timeIndex < 0 ? '-' : '') + hours + ':' + minute;
                }
            },
            convertTimeToShow: function (timeIndex) {
                var self = this;
                var timeString = null;
                if (timeIndex <= self.mainChart.constants.matchTime.endHaftTime) {
                    timeString = timeIndex + "'";
                }
                else if (timeIndex < self.mainChart.constants.chartTime.startSecondTime) {
                    timeString = "HT+" + (timeIndex - self.mainChart.constants.matchTime.endHaftTime) + "'";
                }
                else if (timeIndex <= self.mainChart.constants.chartTime.endSecondTime) {
                    timeString = timeIndex - 15 + "'";
                }
                else {
                    timeString = "90'+" + (timeIndex - self.mainChart.constants.chartTime.endSecondTime) + "'";
                }
                return timeString;
            },
            getTime: function (pointIndex, isShow) {
                /*jshint maxdepth:false*/
                isShow = isShow || false;
                var self = this,
                    extraTimeIndex = 0,
                    secondTimePoint,
                    timeStringObject =
                    {
                        isShow: false,
                        isHaftTime: false,
                        timeString: pointIndex,
                        timeValue: pointIndex,
                        selectedTimeString: pointIndex,
                        width: 4,
                        isRenderInOthersChart: true
                    };

                if (pointIndex === self.mainChart.timeStatus.haftTime.index) {
                    if (pointIndex !== self.mainChart.constants.matchTime.endHaftTime) {
                        extraTimeIndex = pointIndex - self.mainChart.constants.matchTime.endHaftTime;
                        timeStringObject.selectedTimeString = 
                        timeStringObject.timeString = self.mainChart.constants.matchTime.endHaftTime + "'+" + extraTimeIndex + "'";
                        timeStringObject.isShow = true;
                        timeStringObject.isHaftTime = true;
                        timeStringObject.timeValue = self.mainChart.constants.matchTime.endHaftTime;
                        return timeStringObject;
                    }
                }
                else if (pointIndex > self.mainChart.constants.matchTime.endHaftTime) {
                    if (self.mainChart.timeStatus.second.index <= 0) {
                        timeStringObject.timeString = '';
                        if (self.mainChart.timeStatus.haftTime.index > 0
                                && pointIndex > self.mainChart.timeStatus.haftTime.index) {
                            timeStringObject.selectedTimeString = "HT+" + (pointIndex - self.mainChart.timeStatus.haftTime.index);
                        }
                        else {
                            timeStringObject.selectedTimeString = "45'+" + (pointIndex - self.mainChart.constants.matchTime.endHaftTime);
                        }
                    }
                    else {
                        if (pointIndex <= self.mainChart.timeStatus.second.index) {
                            extraTimeIndex = pointIndex - self.mainChart.constants.matchTime.endHaftTime;
                            timeStringObject.selectedTimeString = 
                            timeStringObject.timeString = self.mainChart.constants.matchTime.endHaftTime + "'+" + extraTimeIndex;
                            if (self.mainChart.timeStatus.haftTime.index > 0
                                && pointIndex > self.mainChart.timeStatus.haftTime.index) {
                                timeStringObject.selectedTimeString = "HT+" + (pointIndex - self.mainChart.timeStatus.haftTime.index);
                            }
                            timeStringObject.width = 2;
                            secondTimePoint = self.mainChart.constants.matchTime.endHaftTime;
                        }
                        else {
                            var breakTime = self.mainChart.timeStatus.second.index - self.mainChart.constants.matchTime.endHaftTime;
                            if (pointIndex > self.mainChart.timeStatus.second.index) {
                                secondTimePoint = self.mainChart.constants.matchTime.endHaftTime + pointIndex - self.mainChart.timeStatus.second.index;
                                if (secondTimePoint <= self.mainChart.constants.matchTime.endSecondTime) {
                                    timeStringObject.selectedTimeString = timeStringObject.timeString = secondTimePoint;
                                    isShow = isShow || timeStringObject.timeString === 46;
                                }
                                else {
                                    extraTimeIndex = pointIndex - breakTime - self.mainChart.constants.matchTime.endSecondTime;
                                    timeStringObject.selectedTimeString = 
                                    timeStringObject.timeString = self.mainChart.constants.matchTime.endSecondTime + "'+" + extraTimeIndex;
                                }

                                timeStringObject.timeValue = secondTimePoint;
                            }
                        }
                    }
                }


                    if ((typeof (timeStringObject.timeString) === 'number'
                        && timeStringObject.timeString % 5 === 0 
                        && pointIndex !== self.mainChart.timeStatus.second.index)
                                    || isShow
                                    || timeStringObject.timeString === 46) {
                    timeStringObject.isShow = true;

                }
                timeStringObject.timeString += "'";
                timeStringObject.selectedTimeString += "'";
                return timeStringObject;
            },
            computeHandicap: function (point) {
                var hdpValue = 0;
                if (CRITERIA.chooseHandicap) {
                    if (CRITERIA.chooseHome) {
                        hdpValue = point.AwayHandicap > 0 ? point.AwayHandicap : (0 - point.HomeHandicap);
                    }
                    else {
                        hdpValue = point.AwayHandicap > 0 ? (0 - point.AwayHandicap) : point.HomeHandicap;
                    }
                }
                else {
                    hdpValue = point.AwayHandicap > 0 ? point.AwayHandicap : point.HomeHandicap;
                }

                return hdpValue;
            },
            getTeamText: function () {
                var self = this;
                return '<span style="color:' + self.mainChart.elements.$teamChosen.css('color') + '">'
                            + self.mainChart.elements.$teamChosen.text() 
                        + '</span> ';
            }
        };
    }) ();

    CHART = (function () {
        'use strict';
        return {
            type: {
                market: 1,
                price: 2,
                all: 3,
                event: 4
            },
            customDrawCss: {
                'color': '#606060',
                'font-size': '9px'
            },
            oddsChartIndex: {},
            mainPage: Fanex.ChartContent,
            helper: Fanex.Chart.Helper,
            lastHoverIndex: {
                price: 0,
                market: 0,
                event: 0
            },
            oddsValueThreshold: {
                index: 95,
                value: 2
            },
            isInBreakTime: false,
            isOdd: false,
            refreshCountDownTimeOut: null,
            updateHoverPriceChart: function (xValue, isHighLight) {
                var self = this;
                var currentChart = self.mainPage.elements.$priceChartContainer.highcharts();
                currentChart.series[0].points[self.lastHoverIndex.market].setState();
                var currentPoint = currentChart.series[0].points[xValue];
                if (currentPoint) {
                    if (isHighLight) {
                        currentPoint.setState('hover');
                        currentChart.tooltip.refresh(currentPoint);
                    }
                    var currentValue = 0;
                    if (currentPoint.y != null) {
                        currentValue = currentPoint.y;
                    }

                    self.updateHoverPriceChartValue(currentPoint.name);
                    self.lastHoverIndex.market = xValue;
                }
            },
            updateHoverPriceChartValue: function (point) {
                var self = this, currentOdds = CRITERIA.chooseHome ? point.HomeOdds : point.AwayOdds;
                self.mainPage.elements.$selectedOddsValue.html(currentOdds);
                self.mainPage.elements.$selectedTimeValue.html(self.helper.getTime(point.TimeIndex).selectedTimeString);
                self.mainPage.elements.$selectedHdpValue.html(self.helper.getTeamText() + self.helper.computeHandicap(point));
            },
            updateHoverMarketChart: function (xValue, isHighLight) {
                var self = this;
                var currentChart = self.mainPage.elements.$marketChartcontainer.highcharts();
                currentChart.series[0].points[self.lastHoverIndex.price].setState();
                var currentPoint = currentChart.series[0].points[xValue];
                if (currentPoint) {
                    if (isHighLight) {
                        currentPoint.setState('hover');
                        currentChart.tooltip.refresh(currentPoint);
                    }
                    var currentValue = '0%';
                    if (currentPoint.y != null) {
                        currentValue = currentPoint.y + '%';
                    }
                    self.updateHoverMarketChartValue(currentValue);
                    self.lastHoverIndex.price = xValue;
                }
            },
            updateHoverMarketChartValue: function (value) {
                var teamHtml = '<span style="color:' + CHART_COLOR.homeColor + '">'
                                    + (CRITERIA.chooseHandicap ? CRITERIA.homeName : LANGUAGE.over)
                                + '</span> ';
                this.mainPage.elements.$selectedMarketValue.html(teamHtml + value);
            },
            updateHoverEventChart: function (xValue, isHighLight) {
                var self = this;
                var currentChart = self.mainPage.elements.$eventContainer.highcharts();
                currentChart.series[0].points[self.lastHoverIndex.event].setState();
                var currentPoint = currentChart.series[0].points[xValue];
                if (currentPoint) {
                    if (isHighLight) {
                        currentPoint.setState('hover');
                        currentChart.tooltip.refresh(currentPoint);
                    }
                    self.lastHoverIndex.event = xValue;
                }
            },
            updateCurrentMaketData: function (point) {
                var self = this;
                var homePercent = Math.round(point.HomeTickets * 100 / point.TotalTickets);
                var awayPercent = 100 - homePercent;
                self.mainPage.elements.$homePercent.text(homePercent + '%').css('width', homePercent + '%');
                self.mainPage.elements.$awayPercent.text(awayPercent + '%').css('width', awayPercent + '%');
                self.updateHoverMarketChartValue(homePercent + '%');
            },
            updateCurrentPriceData: function (point) {
                var self = this;
                var oddsValue = CRITERIA.chooseHome ? point.HomeOdds : point.AwayOdds;
                self.mainPage.elements.$currentOddsValue.html(oddsValue);
                self.mainPage.elements.$currentHdpValue.html(self.helper.getTeamText() + self.helper.computeHandicap(point));
                self.updateHoverPriceChartValue(point);
            },
            updateCurrentEventTime: function (point) {
                var self = this;
                var timeString = self.helper.getTime(point.TimeIndex).selectedTimeString;
                //self.mainPage.elements.$currentTime.html(timeString);
                self.mainPage.elements.$selectedTimeValue.html(timeString);
            },
            defaultPriceChartOption: function () {
                var self = this;
                return {
                    chart: {
                        marginLeft: 30,
                        type: 'area',
                        borderRadius: 0,
                        alignTicks: false,
                        marginTop: 10,
                        marginRight: 50,
                        marginBottom: 25
                    },
                    credits: {
                        enabled: false
                    },
                    title: '',
                    yAxis: [{
                        title: null,
                        min: 0,
                        opposite: true
                    }],
                    series: [],
                    xAxis: {
                        labels: {
                            enabled: false
                        },
                        max: CRITERIA.defaultStopTime,
                        min: CRITERIA.isLive ? 0 : (DEFAULT_OBJECT.deadballStartTime + self.mainPage.step.deadball),
                        gridLineWidth: 0,
                        tickLength: 0
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            point: {
                                events: {
                                    mouseOver: function () {
                                        self.updateHoverPriceChart(this.x, true);
                                        self.updateHoverMarketChart(this.x, true);
                                        self.updateHoverEventChart(this.x, true);
                                    }
                                }
                            }
                        },
                        area: {
                            lineWidth: 2,
                            states: {
                                hover: {
                                    lineWidth: 3
                                }
                            },
                            marker: {
                                //enabled: false,
                                radius: 2
                            },
                            dataLabels: {
                                enabled: true,
                                useHTML: true,
                                formatter: function () {
                                    var beginTime = CRITERIA.startTime + CRITERIA.interval;
                                    var currentIndex = Math.round((this.x - beginTime) / CRITERIA.interval);
                                    if (currentIndex > 1) {
                                        var prePoint = this.series.points[currentIndex - 1].name;

                                        var currentHandicapValue = self.helper.computeHandicap(this.key);
                                        if (prePoint) {
                                            var paddingRight = '';
                                            if (self.mainPage.currentBrowser.isChrome) {
                                                paddingRight = 'padding-right:10px;';
                                            }
                                            var prePointHandicapValue = self.helper.computeHandicap(prePoint);
                                            if (prePointHandicapValue !== currentHandicapValue
                                                && !self.mainPage.constants.matchStop
                                                && self.mainPage.startUpdateData) {
                                                self.mainPage.updateOddsHandicap(this.key);
                                            }

                                            if (prePointHandicapValue > currentHandicapValue) {
                                                return '<div style="width: 10px; height:15px;">'
                                                            + '<img style="' + paddingRight + '" src="' + DEFAULT_OBJECT.imageFolderUrl + 'down.png"></img>'
                                                        + '</div>';
                                            }
                                            else if (prePointHandicapValue < currentHandicapValue) {
                                                return '<div style="width: 10px; height:15px;">'
                                                            + '<img style="' + paddingRight + '" src="' + DEFAULT_OBJECT.imageFolderUrl + 'up.png"></img>'
                                                        + '</div>';
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    tooltip: {
                        crosshairs: true,
                        formatter: function () {
                            return false;
                        }
                    }
                };
            },
            defaultMarketChartOption: function () {
                var self = this;
                return {
                    chart: {
                        marginLeft: 30,
                        type: 'area',
                        borderRadius: 0,
                        alignTicks: false,
                        marginTop: 10,
                        marginRight: 50,
                        marginBottom: 20
                    },
                    credits: {
                        enabled: false
                    },
                    title: '',
                    tooltip: {
                        crosshairs: true,
                        formatter: function () {
                            return false;
                        }
                    },
                    yAxis: [{
                        title: null,
                        min: 0,
                        opposite: true,
                        max: 100,
                        plotLines: [{
                            color: '#FFFF00',
                            width: 2,
                            value: 50
                        }],
                        labels: {
                            formatter: function () {
                                return this.value + '%';
                            }
                        },
                        tickInterval: 50
                    }],
                    series: [],
                    xAxis: {
                        labels: {
                            enabled: false
                        },
                        max: CRITERIA.defaultStopTime,
                        min: CRITERIA.isLive ? 0 : (DEFAULT_OBJECT.deadballStartTime + self.mainPage.step.deadball),
                        gridLineWidth: 0,
                        tickLength: 0
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            point: {
                                events: {
                                    mouseOver: function () {
                                        self.updateHoverPriceChart(this.x, true);
                                        self.updateHoverMarketChart(this.x, true);
                                        self.updateHoverEventChart(this.x, true);
                                    }
                                }
                            }
                        },
                        area: {
                            lineWidth: 2,
                            states: {
                                hover: {
                                    lineWidth: 3
                                }
                            },
                            marker: {
                                radius: 2
                            }
                        }

                    }
                };
            },
            defaultTimeChartOption: function () {
                var self = this;
                return {
                    chart: {
                        marginLeft: 30,
                        type: 'line',
                        borderRadius: 0,
                        alignTicks: false,
                        marginTop: 34,
                        marginRight: 50,
                        backgroundColor: '#F2F2F2'
                        //marginBottom: 40
                    },
                    title: '',
                    credits: {
                        enabled: false
                    },
                    tooltip: {
                        crosshairs: true,
                        formatter: function () {
                            return false;
                        }
                    },
                    yAxis: [{
                        title: null,
                        min: 0,
                        opposite: true,
                        gridLineWidth: 0,
                        labels: {
                            enabled: false
                        }
                    }],
                    series: [],
                    xAxis: {
                        labels: {
                            enabled: false
                        },
                        lineColor: '#FFFFF',
                        max: CRITERIA.defaultStopTime,
                        min: CRITERIA.isLive ? 0 : (DEFAULT_OBJECT.deadballStartTime + self.mainPage.step.deadball),
                        tickLength: 0
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            point: {
                                events: {
                                    mouseOver: function () {
                                        self.updateHoverPriceChart(this.x, true);
                                        self.updateHoverMarketChart(this.x, true);
                                        self.updateHoverEventChart(this.x, true);
                                    }
                                }
                            }
                        },
                        line: {
                            lineWidth: 2,
                            states: {
                                hover: {
                                    lineWidth: 3
                                }
                            },
                            marker: {
                                //enabled: false,
                                radius: 1
                            }
                        }

                    }
                };
            },
            build: function (charts) {
                var self = this;
                if (charts && charts.length > 2) {
                    var priceChartOptions = self.defaultPriceChartOption(),
                        marketChartOptions = self.defaultMarketChartOption(),
                        timeChartOptions = self.defaultTimeChartOption(),
                        priceOddChartArray = [];


                    for (var i in charts[1].Points) {
                        var point = charts[1].Points[i];
                        if (point.EventType === self.mainPage.indicatorType.second.value) {
                            CRITERIA.stopTime = point.TimeIndex + 50;
                            break;
                        }
                    }



                    //// Build serie, first serie is market chart, and the rest on is price charts
                    var marketChartObject = self.buildSeries(self.type.market, charts[0]);
                    var timeChartObject = self.buildSeries(self.type.event, charts[1]);


                    for (i = 2; i < charts.length; i++) {
                        priceOddChartArray.push(self.buildSeries(self.type.price, charts[i]));
                    }

                    //// Start pust market data to Chart.
                    for (i = 0; i < marketChartObject.series.length; i++) {
                        marketChartObject.series[i].yAxis = 0;
                        marketChartOptions.series.push(marketChartObject.series[i]);
                    }

                    //// Start pust market data to Chart.
                    for (i = 0; i < timeChartObject.series.length; i++) {
                        timeChartObject.series[i].yAxis = 0;
                        timeChartOptions.series.push(timeChartObject.series[i]);
                    }


                    //// Start pust price chart data.
                    var priceSerieIndex = 0;
                    for (var j = 0; j < priceOddChartArray.length; j++) {
                        var currentChartData = priceOddChartArray[j];
                        for (i = 0; i < currentChartData.series.length; i++) {
                            var currentSerie = currentChartData.series[i];
                            currentSerie.yAxis = 0;
                            priceChartOptions.series.push(currentSerie);
                            //// Set index for odds chart to add data after
                            self.oddsChartIndex[currentSerie.oddsId] = priceSerieIndex++;
                        }
                    }

                    //// Render HighChart
                    self.mainPage.elements.$priceChartContainer.highcharts(priceChartOptions);
                    self.mainPage.elements.$marketChartcontainer.highcharts(marketChartOptions);
                    self.mainPage.elements.$eventContainer.highcharts(timeChartOptions);
                    self.updateIndicatorPoint(charts[1].Points);
                    self.renderChoiceText();
                    self.startCountDown();
                }
                else {
                    self.elements.$chartContainers.empty().html('<div class="no-infor">' + LANGUAGE.chartUnavai + '</div>');
                }
            },
            buildPriceChart: function (chart) {
                if (chart) {

                    var self = this, priceChartOptions = self.defaultPriceChartOption();
                    var priceChartSeries = self.buildSeries(self.type.price, chart);

                    //// Start pust price chart data.
                    for (var i = 0; i < priceChartSeries.series.length; i++) {
                        var currentSerie = priceChartSeries.series[i];
                        currentSerie.yAxis = 0;
                        priceChartOptions.series.push(currentSerie);
                        //// Set index for odds chart to add data after
                        self.oddsChartIndex[currentSerie.oddsId] = i;
                    }

                    //// Render HighChart
                    self.mainPage.elements.$priceChartContainer.highcharts(priceChartOptions);
                    self.startCountDown();
                    self.updatePriceTimeLine(chart.Points);
                }
            },
            startCountDown: function () {
                var self = this,
                    maxTime = CRITERIA.stopTime;
                if (self.mainPage.lastTime.market >= maxTime) {
                    //// If match is played, then hide refresh count down
                    self.mainPage.elements.$refreshCountDown.parent().hide();
                    self.mainPage.elements.$hideElementWhenStopMatch.hide();
                    self.mainPage.constants.matchStop = true;
                }
                else {
                    self.mainPage.startUpdateData = true;
                    self.mainPage.elements.$hideElementWhenStopMatch.show();
                    self.mainPage.elements.$refreshCountDown.parent().show();
                    self.mainPage.elements.$refreshCountDown.refreshCountDown('stopCountDown')
                    .refreshCountDown({
                        countDownMinutes: CRITERIA.interval,
                        countDownSeconds: 0,
                        refreshFunction: function (countDownObject, countDownStart) {
                            self.mainPage.stopAjax();
                            self.updatePoints();
                            setTimeout(function () {
                                countDownStart(countDownObject);
                            }, 1000);
                        }
                    });
                }
            },
            buildSeries: function (chartType, chartData) {
                /*jshint maxdepth:false, maxstatements:false*/
                var self = this;
                var isPriceChart = (chartType === self.type.price),
                    isMarketChart = (chartType === self.type.market),
                    isEventChart = (chartType === self.type.event);

                var chartObject =
                    {
                        series: []
                    };

                var homeSerie = null,
                    awaySerie = null,
                    homeOddsIndex = chartData.OddsId + 'Home',
                    awayOddsIndex = chartData.OddsId + 'Away';

                homeSerie = {
                    name: '',
                    isPriceChart: isPriceChart,
                    data: [],
                    oddsId: CRITERIA.chooseAway ? awayOddsIndex : homeOddsIndex,
                    color: '#2F7ED8'
                };

                if (isPriceChart && CRITERIA.chooseAllTeam) {
                    awaySerie = {
                        name: '',
                        isPriceChart: isPriceChart,
                        data: [],
                        oddsId: awayOddsIndex,
                        color: isPriceChart ? CHART_COLOR.awayColor : CHART_COLOR.marketColor
                    };
                }

                var i = null,
                    index = 0;

                //// Sort Object Property, because default sort properties of Chrome different with other browsers
                var keysSorted = Object.keys(chartData.Points).sort(function (timeA, timeB) {
                    return chartData.Points[timeA].TimeIndex - chartData.Points[timeB].TimeIndex;
                });
                var showAway = CRITERIA.chooseAway;
                var point = null, lastHasValuePoint = null;


                for (i in keysSorted) {
                    index = parseInt(keysSorted[i]);
                    if (index > CRITERIA.stopTime) {
                        break;
                    }

                    point = chartData.Points[keysSorted[i]];
                    var isNoData = (point.TotalTickets === 0);

                    point.IsPriceChart = isPriceChart;
                    point.BetTypeId = CRITERIA.betTypeId;

                    if (isPriceChart && index > self.mainPage.lastTime[chartData.OddsId]) {
                        self.mainPage.lastTime[chartData.OddsId] = index;
                    }
                    else if (!isPriceChart && index > self.mainPage.lastTime.market) {
                        self.mainPage.lastTime.market = index;
                    } else if (!isEventChart && index > self.mainPage.lastTime.event) {
                        self.mainPage.lastTime.event = index;
                    }

                    if (index === 0 && isPriceChart) {
                        continue;
                    }

                    var yValue = 50;
                    if (homeSerie != null) {
                        var homePoint = LIVECHARTS.cloneObject(point);
                        homePoint.IsHome = true;


                        if (isPriceChart) {
                            if (index !== 0) {
                                yValue = showAway ? homePoint.AwayOdds : homePoint.HomeOdds;
                                if (index > self.oddsValueThreshold.index && yValue > self.oddsValueThreshold.value) {
                                    yValue = self.oddsValueThreshold.value;
                                }

                                if (index === 1) {
                                    homeSerie.data.push({
                                        name: homePoint,
                                        x: 0,
                                        y: yValue
                                    });
                                }
                            }
                        } else if (isMarketChart) {
                            yValue = isNoData ? null : Math.round(point.HomeTickets * 100 / point.TotalTickets);
                        } else if (isEventChart) {
                            switch (point.EventType) {
                                case self.mainPage.indicatorType.hT.value:
                                    self.isInBreakTime = true;
                                    break;
                                case self.mainPage.indicatorType.second.value:
                                    yValue = null;
                                    self.isInBreakTime = false;
                                    break;
                                default:
                                    break;
                            }

                            if (point.SubPoints.length > 0) {
                                for (var j = 0; j < point.SubPoints.length; j++) {
                                    switch (point.SubPoints[j].EventType) {
                                        case self.mainPage.indicatorType.hT.value:
                                            self.isInBreakTime = true;
                                            break;
                                        case self.mainPage.indicatorType.second.value:
                                            self.isInBreakTime = false;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            if (self.isInBreakTime
                            && self.mainPage.timeStatus.haftTime.index <= point.TimeIndex) {
                                if (self.isOdd) {
                                    yValue = null;
                                }
                                self.isOdd = !self.isOdd;
                            }
                        }


                        homeSerie.data.push({
                            name: homePoint,
                            x: homePoint.TimeIndex,
                            y: yValue
                        });

                        if (!isPriceChart && !isNoData) {
                            lastHasValuePoint = homePoint;
                        }
                    }
                    if (awaySerie != null) {
                        var awayPoint = LIVECHARTS.cloneObject(point);
                        awayPoint.IsAway = true;
                        awaySerie.data.push({
                            name: awayPoint,
                            x: awayPoint.TimeIndex,
                            y: isPriceChart ? awayPoint.AwayOdds : (isNoData ? null : Math.round(point.AwayTickets * 100 / point.TotalTickets))
                        });
                    }
                }

                if (isPriceChart && point) {
                    self.updateCurrentPriceData(point);
                } else if (isMarketChart && lastHasValuePoint) {
                    self.updateCurrentMaketData(lastHasValuePoint);
                } else if (isEventChart && point) {
                    self.updateCurrentEventTime(point);
                }

                if (homeSerie != null) { chartObject.series.push(homeSerie); }
                if (awaySerie != null) { chartObject.series.push(awaySerie); }

                return chartObject;
            },
            updatePoints: function () {
                var self = this;


                //// Market Chart
                var criteria = LIVECHARTS.cloneObject(CRITERIA);
                //// Get Market Points
                criteria.chartId = self.type.market;
                self.mainPage.ajaxObject.marketChartRequest = self.getPoints({
                    url: self.mainPage.url.getMarketChartPoints,
                    criteria: criteria
                });

                criteria = LIVECHARTS.cloneObject(CRITERIA);
                //// Get Market Points
                criteria.chartId = self.type.event;
                self.mainPage.ajaxObject.indicatorRequest = self.getPoints({
                    url: self.mainPage.url.getIndicatorPoints,
                    criteria: criteria
                });

                //// Get Price Points
                var oddsListLength = self.mainPage.oddsIds.length;
                for (var i = 0; i < oddsListLength; i++) {
                    criteria = LIVECHARTS.cloneObject(CRITERIA);
                    criteria.oddsId = self.mainPage.oddsIds[i];
                    criteria.chartId = self.type.price;
                    self.mainPage.ajaxObject.priceChartRequest.push(self.getPoints({
                        url: self.mainPage.url.getPriceChartPoints,
                        criteria: criteria
                    }));
                }

                if (self.mainPage.lastTime.event < 15) {
                    self.mainPage.checkKickOffTime();
                }
            },
            getPoints: function (params) {
                var self = this;
                var maxTime = CRITERIA.stopTime;
                if ((params.criteria.chartId === self.type.market && self.mainPage.lastTime.market >= maxTime)
                    || (params.criteria.chartId === self.type.price && self.mainPage.lastTime[params.criteria.oddsId] >= maxTime)
                    || (params.criteria.chartId === self.type.event && self.mainPage.lastTime.event >= maxTime)) {
                    self.mainPage.constants.matchStop = true;
                    self.mainPage.elements.$hideElementWhenStopMatch.hide();
                    self.mainPage.elements.$refreshCountDown.refreshCountDown('stopCountDown');
                    self.mainPage.elements.$refreshCountDown.parent().hide();
                    return;
                }
                var getDataParams =
                    {
                        matchId: params.criteria.matchId,
                        betTypeId: params.criteria.betTypeId,
                        oddsId: params.criteria.oddsId,
                        isLive: params.criteria.isLive,
                        chartId: params.criteria.chartId,
                        kickOffTime: params.criteria.kickOffTime
                    };

                switch (params.criteria.chartId) {
                    case self.type.market:
                        getDataParams.marketChartTimeIndex = self.mainPage.lastTime.market;
                        break;
                    case self.type.price:
                        getDataParams.priceChartTimeIndex = self.mainPage.lastTime[params.criteria.oddsId];
                        break;
                    case self.type.event:
                        getDataParams.eventChartTimeIndex = self.mainPage.lastTime.event;
                        break;
                }

                var ajaxRequest = $.ajax({
                    url: params.url,
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: $.param(getDataParams)
                }).done(function (points) {
                    switch (params.criteria.chartId) {
                        case self.type.market:
                            self.addMarketPoint(params.criteria, points);
                            break;
                        case self.type.price:
                            self.addPricePoint(params.criteria, points);
                            break;
                        case self.type.event:
                            self.addEventPoint(params.criteria, points);
                            break;
                    }

                }).fail(function (jqXHR, textStatus, errorThrown) {
                    self.mainPage.handleAjaxException(jqXHR, textStatus, errorThrown);
                });

                return ajaxRequest;
            },
            addPricePoint: function (criteria, points) {
                /*jshint maxdepth:false*/
                var self = this,
                    chart = self.mainPage.elements.$priceChartContainer.highcharts(),
                    index = 0,
                    point = null;

                for (var i in points) {
                    point = points[i];
                    index = parseInt(i);

                    if (self.mainPage.lastTime[criteria.oddsId] < index) {
                        self.mainPage.lastTime[criteria.oddsId] = index;

                        point.IsPriceChart = true;
                        point.BetTypeId = criteria.betTypeId;

                        if (criteria.chooseAllTeam) {
                            var homeSerieIndex = self.oddsChartIndex[criteria.oddsId + 'Home'];
                            chart.series[homeSerieIndex].addPoint({
                                name: point,
                                x: point.TimeIndex,
                                y: point.HomeOdds
                            });
                            chart.series[homeSerieIndex + 1].addPoint({
                                name: point,
                                x: point.TimeIndex,
                                y: point.AwayOdds
                            });
                        }
                        else {
                            var yValue = criteria.chooseHome ? point.HomeOdds : point.AwayOdds;

                            if (index !== 0) {
                                if (index > self.oddsValueThreshold.index && yValue > self.oddsValueThreshold.value) {
                                    yValue = self.oddsValueThreshold.value;
                                }

                                if (index === 1) {
                                    chart.series[0].addPoint({
                                        name: point,
                                        x: 0,
                                        y: yValue
                                    });
                                }
                            }

                            if (index === 0) {
                                continue;
                            }

                            chart.series[0].addPoint({
                                name: point,
                                x: point.TimeIndex,
                                y: yValue
                            });
                        }
                    }
                }

                if (point && index !== 0) {
                    self.updateCurrentPriceData(point);
                }
            },
            addMarketPoint: function (criteria, points) {
                var self = this;
                var chart = self.mainPage.elements.$marketChartcontainer.highcharts(),
                    index = 0;

                for (var i in points) {
                    var point = points[i];
                    index = parseInt(i);
                    if (index > self.mainPage.lastTime.market) {
                        self.mainPage.lastTime.market = parseInt(i);
                        point.IsPriceChart = false;
                        point.BetTypeId = criteria.betTypeId;
                        var isNoData = (point.TotalTickets === 0);

                        var homePoint = LIVECHARTS.cloneObject(point);
                        homePoint.IsHome = true;
                        chart.series[0].addPoint({
                            name: homePoint,
                            x: homePoint.TimeIndex,
                            y: isNoData ? null : Math.round(homePoint.HomeTickets * 100 / homePoint.TotalTickets)
                        });

                        if (!isNoData) {
                            self.updateCurrentMaketData(homePoint);
                        }
                    }
                }


            },
            addEventPoint: function (criteria, points) {
                var self = this;
                var chart = self.mainPage.elements.$eventContainer.highcharts(),
                    index = 0,
                    yValue = 50,
                    point = null;

                for (var i in points) {
                    point = points[i];
                    index = parseInt(i);
                    if (index > self.mainPage.lastTime.event) {
                        self.mainPage.lastTime.event = parseInt(i);
                        switch (point.EventType) {
                            case self.mainPage.indicatorType.hT.value:
                                self.isInBreakTime = true;
                                break;
                            case self.mainPage.indicatorType.second.value:
                                self.isInBreakTime = false;
                                break;
                            default:
                                break;
                        }

                        if (point.SubPoints.length > 0) {
                            for (var j = 0; j < point.SubPoints.length; j++) {
                                switch (point.SubPoints[j].EventType) {
                                    case self.mainPage.indicatorType.hT.value:
                                        self.isInBreakTime = true;
                                        break;
                                    case self.mainPage.indicatorType.second.value:
                                        self.isInBreakTime = false;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        if (self.isInBreakTime
                        && self.mainPage.timeStatus.haftTime.index <= point.TimeIndex) {
                            if (self.isOdd) {
                                yValue = null;
                            }
                            self.isOdd = !self.isOdd;
                        }

                        chart.series[0].addPoint({
                            x: point.TimeIndex,
                            y: yValue
                        });
                    }
                }

                self.updateIndicatorPoint(points);
            },
            addIndicatorPoint: function (chart, indicatorPoint, currentPoint, pointLevel) {
                var self = this,
                    isHome = indicatorPoint.TeamId === CRITERIA.homeId,
                            left = currentPoint.plotX + chart.plotLeft - 9,
                            top = isHome ? (125 + pointLevel * 28) : (25 - pointLevel * 28),
                            addLeft = 0,
                            paddingRight = 3,
                            addTop = 15,
                            text = indicatorPoint.Name,
                            imageWidth = 14,
                            imageHeight = 17,
                            minusTop = -11,
                            rendetText = true, bottomPoint = 85, addImageTop = (isHome ? -28 : 28), addHaftTimeTop = 0,
                            hasValue = indicatorPoint.Value && indicatorPoint.Value.length > 1;

                switch (indicatorPoint.EventType) {
                    case self.mainPage.indicatorType.goal.value:
                        if (indicatorPoint.TimeIndex > self.mainPage.constants.matchTime.endHaftTime
                            && !isHome
                            && (self.mainPage.timeStatus.second.index < 0 || indicatorPoint.TimeIndex < self.mainPage.timeStatus.second.index)) {
                            addHaftTimeTop = -10;
                        }
                        chart.renderer.image(self.mainPage.indicatorType.goal.image, left, top + addImageTop + addHaftTimeTop, imageHeight, imageHeight).add();
                        if (hasValue) {
                            chart.renderer.text(indicatorPoint.Value, left + imageWidth + 3, top + addImageTop + imageWidth - 2)
                                .attr({ zIndex: 5 })
                                .css(self.customDrawCss)
                                .add();
                        }
                        addLeft += 3 + (indicatorPoint.TimeIndex < 10 ? 3 : 0);
                        text = '' + self.helper.getTime(indicatorPoint.TimeIndex, true).selectedTimeString;
                        top += addTop - (isHome ? imageHeight : minusTop) + addHaftTimeTop;

                        break;
                    case self.mainPage.indicatorType.redCard.value:
                        chart.renderer.image(self.mainPage.indicatorType.redCard.image, left + 2, top + addImageTop, imageWidth, imageHeight).add();
                        if (hasValue) {
                            chart.renderer.text(indicatorPoint.Value, left + imageWidth + 3, top + addImageTop + imageWidth - 2)
                                .attr({ zIndex: 5 })
                                .css(self.customDrawCss)
                                .add();
                        }
                        addLeft += 3 + (indicatorPoint.TimeIndex < 10 ? 3 : 0);
                        text = '' + self.helper.getTime(indicatorPoint.TimeIndex, true).timeString;
                        top += addTop - (isHome ? imageHeight : minusTop);
                        break;
                    case self.mainPage.indicatorType.yellowCard.value:
                        chart.renderer.image(self.mainPage.indicatorType.yellowCard.image, left + 2, top + addImageTop, imageWidth, imageHeight).add();
                        addLeft += 3 + (indicatorPoint.TimeIndex < 10 ? 3 : 0);
                        text = '' + self.helper.getTime(indicatorPoint.TimeIndex, true).timeString;
                        top += addTop - (isHome ? imageHeight : minusTop);
                        break;
                    case self.mainPage.indicatorType.hT.value:
                        self.mainPage.timeStatus.haftTime.index = indicatorPoint.TimeIndex;
                        top = bottomPoint - 3;
                        text = 'HT';
                        addLeft += 3;
                        break;
                    case self.mainPage.indicatorType.first.value:
                        self.mainPage.timeStatus.first.index = indicatorPoint.TimeIndex;
                        top = bottomPoint;
                        text = '1st';
                        rendetText = false;
                        break;
                    case self.mainPage.indicatorType.second.value:
                        self.mainPage.timeStatus.second.index = indicatorPoint.TimeIndex;
                        CRITERIA.stopTime = indicatorPoint.TimeIndex + 50;
                        top = bottomPoint - 3;
                        text = '2H';
                        left += 8;
                        break;
                    case self.mainPage.indicatorType.extraTime.value:
                        self.mainPage.timeStatus.extraTime.index = indicatorPoint.TimeIndex;
                        top = bottomPoint + addTop + minusTop;
                        text = 'extra time ' + self.helper.getTime(indicatorPoint.TimeIndex, true).timeString;
                        rendetText = false;
                        break;
                    case self.mainPage.indicatorType.over.value:
                        self.mainPage.timeStatus.over.index = indicatorPoint.TimeIndex;
                        top = bottomPoint + addTop + minusTop;
                        break;
                }

                if (rendetText) {
                    chart.renderer.text(text, left + paddingRight, top)
                        .attr({ zIndex: 5 })
                        .css(self.customDrawCss)
                        .add();
                }
            },
            updateIndicatorPoint: function (points) {
                var self = this;
                var indicatorPoint = null;
                for (var i in points) {
                    indicatorPoint = points[i];
                    var index = parseInt(i);
                    var chart = self.mainPage.elements.$eventContainer.highcharts(),
                        currentPoint = chart.series[0].data[index];
                    //// Get current for get left and top to showing indicator point.
                    if (currentPoint) {
                        //// Get last time index for query data later
                        self.mainPage.lastTime.event = indicatorPoint.TimeIndex;

                        self.addIndicatorPoint(chart, indicatorPoint, currentPoint, 0);

                        if (indicatorPoint.SubPoints.length > 0) {
                            var homeIndex = (indicatorPoint.TeamId === CRITERIA.homeId) ? 0 : -1,
                                awayIndex = (indicatorPoint.TeamId === CRITERIA.awayId) ? 0 : -1,
                                chooseHome = false, chooseAway = false;
                            for (var j = 0; j < indicatorPoint.SubPoints.length; j++) {
                                var currentSubPoint = indicatorPoint.SubPoints[j];
                                chooseHome = currentSubPoint.TeamId === CRITERIA.homeId;
                                chooseAway = currentSubPoint.TeamId === CRITERIA.awayId;
                                self.addIndicatorPoint(chart, currentSubPoint, currentPoint, chooseHome ? ++homeIndex : ++awayIndex);
                            }
                        }

                        self.updateTimeLine(index);

                    }
                }

                if (indicatorPoint) {
                    self.updateCurrentEventTime(indicatorPoint);
                }
            },
            updateTimeLine: function (pointIndex) {
                if (CRITERIA.isLive) {
                    var self = this;
                    var eventChart = self.mainPage.elements.$eventContainer.highcharts(),
                        priceChart = self.mainPage.elements.$priceChartContainer.highcharts(),
                        marketChart = self.mainPage.elements.$marketChartcontainer.highcharts(),
                        currentPoint = eventChart.series[0].data[pointIndex],
                        eventBottomPoint = 95, priceBottomPoint = 110, marketBottomPoint = 186;

                    if (currentPoint) {
                        var left = currentPoint.plotX + eventChart.plotLeft,
                            currentMatchPoint = self.helper.getTime(pointIndex),
                            paddingWidth = currentMatchPoint.timeValue >= 10 ? (currentMatchPoint.timeValue >= 100 ? 9.5 : 6.5) : 3.5;

                        if (currentMatchPoint.isShow) {
                            if (currentMatchPoint.isHaftTime) {
                                eventBottomPoint -= 23;
                                //left += 2;
                            }

                            left -= paddingWidth;

                            self.renderTimeValue(eventChart, currentMatchPoint.timeString, left, eventBottomPoint);
                            if (!currentMatchPoint.isHaftTime) {
                                self.renderTimeValue(marketChart, currentMatchPoint.timeString, left, priceBottomPoint);
                                self.renderTimeValue(priceChart, currentMatchPoint.timeString, left, marketBottomPoint);
                                self.renderCrossLine(marketChart, pointIndex);
                                self.renderCrossLine(priceChart, pointIndex);
                            }
                        }
                    }
                }
            },
            updatePriceTimeLine: function (points) {
                var self = this;
                for (var i in points) {
                    var pointIndex = parseInt(i);
                    var priceChart = self.mainPage.elements.$priceChartContainer.highcharts(),
                        currentPoint = priceChart.series[0].data[pointIndex],
                        bottomPoint = 186;

                    if (currentPoint) {
                        var left = currentPoint.plotX + priceChart.plotLeft,
                            currentMatchPoint = self.helper.getTime(pointIndex),
                            paddingWidth = currentMatchPoint.timeValue >= 10 ? (currentMatchPoint.timeValue >= 100 ? 9 : 6) : 3;

                        if (currentMatchPoint.isShow && !currentMatchPoint.isHaftTime) {
                            self.renderTimeValue(priceChart, currentMatchPoint.timeString, left - paddingWidth, bottomPoint);
                            self.renderCrossLine(priceChart, pointIndex);
                        }
                    }
                }
            },
            renderTimeValue: function (chart, timeString, left, top) {
                var self = this;
                chart.renderer.text(timeString, left, top)
                            .attr({ zIndex: 5 })
                            .css(self.customDrawCss)
                            .add();
            },
            renderChoiceText: function () {
                var self = this,
                    marketChart = self.mainPage.elements.$marketChartcontainer.highcharts(),
                    eventChart = self.mainPage.elements.$eventContainer.highcharts(),
                    awayColor = CHART_COLOR.awayColor, homeColor = CHART_COLOR.homeColor;

                if (marketChart && eventChart) {

                    var left = marketChart.plotLeft,
                        awayText = CRITERIA.chooseOverUnder ? LANGUAGE.under : CRITERIA.awayName,
                        homeText = CRITERIA.chooseOverUnder ? LANGUAGE.over : CRITERIA.homeName,
                        marketTop = 22, marketBottom = 98, eventTop = 9, eventBottom = 188, colorOpacity = 0.7;

                    //self.renderPriceText();s

                    marketChart.renderer.text(awayText, left, marketTop)
                        .attr({ zIndex: 5 })
                        .css({
                            color: awayColor,
                            opacity: colorOpacity
                        }).add();

                    marketChart.renderer.text(homeText, left, marketBottom)
                        .attr({ zIndex: 5 })
                        .css({
                            color: homeColor,
                            opacity: colorOpacity
                        }).add();

                    eventChart.renderer.text(CRITERIA.awayName, left, eventTop)
                       .attr({ zIndex: 5 })
                       .css({
                           color: awayColor,
                           opacity: colorOpacity
                       }).add();

                    eventChart.renderer.text(CRITERIA.homeName, left, eventBottom)
                        .attr({ zIndex: 5 })
                        .css({
                            color: homeColor,
                            opacity: colorOpacity
                        }).add();
                }
            },
            renderPriceText: function () {
                var self = this,
                    priceChart = self.mainPage.elements.$priceChartContainer.highcharts(),
                    awayColor = CHART_COLOR.awayColor, homeColor = CHART_COLOR.homeColor;

                if (priceChart) {
                    var left = priceChart.plotLeft,
                        awayText = CRITERIA.chooseOverUnder ? LANGUAGE.under : CRITERIA.awayName,
                        homeText = CRITERIA.chooseOverUnder ? LANGUAGE.over : CRITERIA.homeName;

                    priceChart.renderer.text(
                        CRITERIA.chooseHome ? homeText : awayText,
                        left,
                        18)
                        .attr({ zIndex: 5 })
                        .css({
                            color: CRITERIA.chooseHome ? homeColor : awayColor
                        }).add();
                }
            },
            renderCrossLine: function (chart, timeIndex) {
                if (chart) {
                    chart.xAxis[0].addPlotLine({
                        value: timeIndex,
                        width: 1,
                        color: '#C0c0c0'
                    });
                }
            }
        };
    }) ();



    $(function () {
        'use strict';
        Fanex.ChartContent.init();
        $('.dropdown-toggle').dropdown();
    });
}
else {
    alert('jQuery is required.');
}