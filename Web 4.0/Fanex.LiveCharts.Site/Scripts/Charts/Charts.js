/// <reference path="../Libs/jquery-1.9.1.min.js" />
/// <reference path="../Libs/HighChart/highcharts.src.js" />

/// <reference path="../LIVECHARTS.js" />
/// <reference path="../Libs/BlockUI/jquery.blockUI.js" />
/// <reference path="../../Views/MovementCharts/Index.cshtml" />
/// <reference path="../Libs/RefreshCountDown/RefreshCountDown.js" />
/*global DEFAULT_OBJECT, LANGUAGE, LIVECHARTS*/
if (jQuery !== undefined) {
    var FILTER;
    var MAIN_PAGE = (function ($) {
        'use strict';
        //// Private Variables
        this.currentIndex = 12;
        //// Public
        return {
            constants: {
                getMissedPointInterval: 10000,
                getPointInterval: 60000,
                reCheckInInterval: 600000,
                isFirstTime: true,
                homeColor: '#FF0000',
                awayColor: '#00FF00'
            },
            step: {
                live: 1,
                deadball: 15
            },
            interval: {
                missedPrice: null,
                missedMarket: null,
                getPoint: null
            },
            lastTime: {
                price: 0,
                market: 0
            },
            currentChart: null,
            currentOddChangeTime: null,
            kickOffTime: null,
            global: {
                homeName: '',
                awayName: ''
            },
            ajaxObject: {
                priceChartRequest: null,
                marketChartRequest: null,
                getChartRequest: null
            },
            cacheElements: function () {
                /*jshint maxstatements:false*/
                MAIN_PAGE.elements = {};
                MAIN_PAGE.elements.$Sport = $('#ddlSportTypes');
                MAIN_PAGE.elements.$League = $('#ddlLeagues');
                MAIN_PAGE.elements.$Date = $('#txtDatePicker');
                MAIN_PAGE.elements.$Match = $('#ddlMatch');
                MAIN_PAGE.elements.$BetType = $('#ddlBetType');
                MAIN_PAGE.elements.$BettingOdds = $('#ddlBettingOdds');
                MAIN_PAGE.elements.$BettingOdds = $('#ddlBettingOdds');
                MAIN_PAGE.elements.$BtnRefresh = $('#btnRefresh');
                MAIN_PAGE.elements.$BtnLoadChart = $('#btnLoadChart');
                MAIN_PAGE.elements.$Interval = $('#ddlInterval');
                MAIN_PAGE.elements.$DeadballInterval = $('#ddlDeadBallInterval');
                MAIN_PAGE.elements.$CbkPriceMovement = $('#cbkPriceMovement');
                MAIN_PAGE.elements.$CbkMarketMovement = $('#cbkMarketMovement');
                MAIN_PAGE.elements.$DivMainChart = $('#div-main-chart');
                MAIN_PAGE.elements.$ChartContainer = $('#chart-container');
                MAIN_PAGE.elements.$PriceOddChoiceGroup = $('.price-odd-choices-group');
                MAIN_PAGE.elements.$MarketOddChoiceGroup = $('.market-odd-choices-group');
                MAIN_PAGE.elements.$PriceGroupHandicapChoice = $('#price-odd-choice-handicap');
                MAIN_PAGE.elements.$MarketGroupHandicapChoice = $('#market-odd-choice-handicap');
                MAIN_PAGE.elements.$PriceGroupOneXTwoChoice = $('#price-odd-choice-1x2');
                MAIN_PAGE.elements.$PriceGroupOverUnderChoice = $('#price-odd-choice-overunder');
                MAIN_PAGE.elements.$MarketGroupHandicapChoice = $('#market-odd-choice-handicap');
                MAIN_PAGE.elements.$MarketGroupOneXTwoChoice = $('#market-odd-choice-1x2');
                MAIN_PAGE.elements.$MarketGroupOverUnderChoice = $('#market-odd-choice-overunder');
                MAIN_PAGE.elements.$CbkPriceFavorite = $('#cbkPriceFavorite');
                MAIN_PAGE.elements.$CbkPriceUnderdog = $('#cbkPriceUnderdog');
                MAIN_PAGE.elements.$CbkPriceOver = $('#cbkPriceOver');
                MAIN_PAGE.elements.$CbkPriceUnder = $('#cbkPriceUnder');
                MAIN_PAGE.elements.$CbkPriceHome = $('#cbkPriceHome');
                MAIN_PAGE.elements.$CbkPriceDraw = $('#cbkPriceDraw');
                MAIN_PAGE.elements.$CbkPriceAway = $('#cbkPriceAway');
                MAIN_PAGE.elements.$CbkMarketFavorite = $('#cbkMarketFavorite');
                MAIN_PAGE.elements.$CbkMarketUnderdog = $('#cbkMarketUnderdog');
                MAIN_PAGE.elements.$CbkMarketOver = $('#cbkMarketOver');
                MAIN_PAGE.elements.$CbkMarketUnder = $('#cbkMarketUnder');
                MAIN_PAGE.elements.$CbkMarketHome = $('#cbkMarketHome');
                MAIN_PAGE.elements.$CbkMarketDraw = $('#cbkMarketDraw');
                MAIN_PAGE.elements.$CbkMarketAway = $('#cbkMarketAway');
                MAIN_PAGE.elements.$AllPriceOddChoices = $('.price-odd-choices-group input:checkbox');
                MAIN_PAGE.elements.$AllMarketOddChoices = $('.market-odd-choices-group input:checkbox');
                MAIN_PAGE.elements.$LeagueName = $('#filter-information .league-name');
                MAIN_PAGE.elements.$MatchName = $('#filter-information .match-name');
                MAIN_PAGE.elements.$KickOffTime = $('#filter-information .kick-off-time');
                MAIN_PAGE.elements.$MatchStatus = $('#filter-information .match-status');
                MAIN_PAGE.elements.$BetTypeName = $('#filter-information .bet-type-name');
                MAIN_PAGE.elements.$OddsName = $('#filter-information .odds-name');
                MAIN_PAGE.elements.$FilterInformation = $('#filter-information');
                MAIN_PAGE.elements.$BtnCollapse = $('.filter-body .btn-collapse');
                MAIN_PAGE.elements.$BtnExpand = $('.filter-body .btn-expand');
                MAIN_PAGE.elements.$FilterContent = $('.filter-body .filter-body-content');
                MAIN_PAGE.elements.$ChartStyle = $('#chart-style-select');
                MAIN_PAGE.elements.$PriceChartStyle = $('#price-style-select');
                MAIN_PAGE.elements.$MarketChartStyle = $('#market-style-select');
            },
            url: {
                getChartsUrl: LIVECHARTS.resolveUrl('api/ChartApi/GetCharts'),
                getLeaguesUrl: LIVECHARTS.resolveUrl('api/ChartApi/GetLeagues'),
                getPointsUrl: LIVECHARTS.resolveUrl('api/ChartApi/GetPoints'),
                getMarketChartPoints: LIVECHARTS.resolveUrl('api/ChartApi/GetMarketChartPoints'),
                getPriceChartPoints: LIVECHARTS.resolveUrl('api/ChartApi/GetPriceChartPoints'),
                getMatches: LIVECHARTS.resolveUrl('api/ChartApi/GetMatches'),
                getOdds: LIVECHARTS.resolveUrl('api/ChartApi/GetOdds'),
                reCheckIn: LIVECHARTS.resolveUrl('User/ReCheckIn'),
                chartContent: LIVECHARTS.resolveUrl('Charts/ChartContent')
            },
            betTypeEnum: {
                Handicap: 1,
                OverUnder: 3,
                OneXTwo: 5
            },
            oddsChoiceEnum: {
                Home: 1,
                Draw: 2,
                Away: 3,
                All: 4
            },
            initPage: function () {
                MAIN_PAGE.cacheElements();
                FILTER.initMainFilter();
                //                setInterval(function () {
                //                    $.ajax({
                //                        url: MAIN_PAGE.url.reCheckIn,
                //                        type: 'GET',
                //                        contentType: "application/json; charset=utf-8",
                //                        dataType: "json",
                //                        data: ''
                //                    }).done(function (data) {
                //                    }).fail(function (jqXHR, textStatus, errorThrown) {
                //                        MAIN_PAGE.handleAjaxException(jqXHR, textStatus, errorThrown);
                //                    }).always(function () {
                //                    });
                //                }, MAIN_PAGE.constants.reCheckInInterval);
            },
            loadDefaultView: function () {
                FILTER.initSubFilter();
                MAIN_PAGE.criteria = {
                    BetTypeId: parseInt(MAIN_PAGE.elements.$BetType.val()),
                    MatchId: parseInt(MAIN_PAGE.elements.$Match.val()),
                    OddsId: parseInt(MAIN_PAGE.elements.$BettingOdds.val()),
                    KickOffTime: MAIN_PAGE.elements.$Match.find("option:selected").attr('kick-off-time')
                };
                var selectedOdds = MAIN_PAGE.elements.$BettingOdds.find('option:selected');
                if (selectedOdds.attr('is-live') === 'true') {
                    MAIN_PAGE.criteria.IsLive = true;
                    MAIN_PAGE.criteria.PriceChartTimeIndex = MAIN_PAGE.lastTime.price = DEFAULT_OBJECT.liveStartTime;
                    MAIN_PAGE.criteria.MarketChartTimeIndex = MAIN_PAGE.lastTime.market = DEFAULT_OBJECT.liveStartTime;
                    MAIN_PAGE.elements.$Interval.val('1');
                }
                else {
                    MAIN_PAGE.criteria.IsLive = false;
                    MAIN_PAGE.criteria.PriceChartTimeIndex = MAIN_PAGE.lastTime.price = DEFAULT_OBJECT.deadballStartTime;
                    MAIN_PAGE.criteria.MarketChartTimeIndex = MAIN_PAGE.lastTime.market = DEFAULT_OBJECT.deadballStartTime;
                    MAIN_PAGE.elements.$DeadballInterval.val('15');
                }
                MAIN_PAGE.currentOddChangeTime = selectedOdds.attr('change-time');
                MAIN_PAGE.loadChart();
            },
            handleAjaxException: function (jqXHR, textStatus, errorThrown) {
                try { $.unblockUI(); } catch (e) { }
                if (textStatus !== 'abort') {
                    LIVECHARTS.handleAjaxError(jqXHR, textStatus, errorThrown);
                }
            }
        };
    }) (jQuery);

    FILTER = (function ($) {
        'use strict';
        return {
            initMainFilter: function () {
                //// Main Filter
                MAIN_PAGE.elements.$Date.datepicker({
                    showOn: 'button',
                    buttonImage: DEFAULT_OBJECT.datePickerImage,
                    buttonImageOnly: true,
                    dateFormat: "d MM, yy",
                    //maxDate: "+1d",
                    onSelect: function (date) {
                        FILTER.loadLeagues(date);
                    }
                });
                MAIN_PAGE.elements.$League.change(function () {
                    FILTER.loadMatches(parseInt($(this).val()), MAIN_PAGE.elements.$Date.val());
                    FILTER.removeError($(this));
                });
                MAIN_PAGE.elements.$Match.change(function () {
                    FILTER.removeError($(this));
                });
                MAIN_PAGE.elements.$BetType.change(function () {
                    FILTER.removeError($(this));
                });
                MAIN_PAGE.elements.$BettingOdds.change(function () {
                    FILTER.removeError($(this));
                });
                MAIN_PAGE.elements.$BtnLoadChart.click(function () {
                    if (FILTER.validInputs()) {
                        //kickOffTime = MAIN_PAGE.elements.$Match.find("option:selected").attr('kick-off-time');
                        var url = MAIN_PAGE.url.chartContent + '?' + $.param({
                            matchId: parseInt(MAIN_PAGE.elements.$Match.val())
                        });
                        $('#frmChartContent').attr('src', url).show();
                    }
                });
                MAIN_PAGE.elements.$BtnCollapse.click(function () {
                    $(this).hide();
                    MAIN_PAGE.elements.$LeagueName.text(MAIN_PAGE.elements.$League.find("option:selected").text());
                    MAIN_PAGE.elements.$MatchName.text(MAIN_PAGE.elements.$Match.find("option:selected").text());
                    MAIN_PAGE.elements.$FilterInformation.show();
                    MAIN_PAGE.elements.$BtnExpand.show(100);
                    MAIN_PAGE.elements.$FilterContent.hide(100);

                });
                MAIN_PAGE.elements.$BtnExpand.click(function () {
                    $(this).hide();
                    MAIN_PAGE.elements.$FilterInformation.hide();
                    MAIN_PAGE.elements.$BtnCollapse.show(100);
                    MAIN_PAGE.elements.$FilterContent.show(100);
                });


            },
            buildBriefInformation: function () {
                MAIN_PAGE.elements.$LeagueName.text(MAIN_PAGE.elements.$League.find("option:selected").text());
                var matchSelected = MAIN_PAGE.elements.$Match.find("option:selected");
                MAIN_PAGE.elements.$MatchName.text(matchSelected.text());
                MAIN_PAGE.kickOffTime = matchSelected.attr('kick-off-time');
                MAIN_PAGE.kickOffTimeString = matchSelected.attr('kick-off-time-string');
                MAIN_PAGE.global.homeName = matchSelected.attr('home-name');
                MAIN_PAGE.global.awayName = matchSelected.attr('away-name');
                MAIN_PAGE.elements.$KickOffTime.text(' on ' + MAIN_PAGE.kickOffTimeString);
                MAIN_PAGE.elements.$BetTypeName.text(MAIN_PAGE.elements.$BetType.find("option:selected").text());
                var oddSelected = MAIN_PAGE.elements.$BettingOdds.find("option:selected");
                MAIN_PAGE.elements.$OddsName.text(oddSelected.text());
                if (oddSelected.attr('is-live') === 'true') {
                    MAIN_PAGE.elements.$MatchStatus.addClass('match-status-live');
                }
                else {
                    MAIN_PAGE.elements.$MatchStatus.removeClass('match-status-live');
                }
            },
            removeError: function (element) {
                if (element.hasClass('custom-error') && parseInt(element.val()) !== 0) {
                    element.removeClass('custom-error');
                    element.popover('destroy');
                }
            },

            validInputs: function () {
                if (parseInt(MAIN_PAGE.elements.$League.val()) === 0) {
                    MAIN_PAGE.elements.$League.popover({
                        title: "Warning",
                        content: DEFAULT_OBJECT.chooseALeague
                    });
                    MAIN_PAGE.elements.$League.popover('show');
                    MAIN_PAGE.elements.$League.addClass('custom-error');
                    return false;
                }

                if (parseInt(MAIN_PAGE.elements.$Match.val()) === 0) {
                    MAIN_PAGE.elements.$Match.popover({
                        title: "Warning",
                        content: DEFAULT_OBJECT.chooseAMatch
                    });
                    MAIN_PAGE.elements.$Match.popover('show');
                    MAIN_PAGE.elements.$Match.addClass('custom-error');
                    return false;
                }



                return true;
            },
            loadLeagues: function (date) {
                $.ajax({
                    url: MAIN_PAGE.url.getLeaguesUrl,
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: $.param({
                        matchDate: date
                    }),
                    beforeSend: function () {
                        try { $.blockUI(); } catch (e) { }
                    }
                }).done(function (data) {
                    var length = data.length,
                        leagueString = [],
                        isSelected = false;

                    leagueString.push('<option value="0">' + LANGUAGE.selectLeague + '</option>');
                    for (var i = 0; i < length; i++) {
                        var currentLeague = data[i];
                        leagueString.push('<option value="' + currentLeague.Id + '" ' + (!isSelected ? 'selected="selected"' : '') + '>'
                                                + currentLeague.LeagueName
                                            + '</option>');
                        isSelected = true;
                    }
                    MAIN_PAGE.elements.$League.empty().html(leagueString.join(''));
                    MAIN_PAGE.elements.$League.trigger('change');
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    MAIN_PAGE.handleAjaxException(jqXHR, textStatus, errorThrown);
                }).always(function () {
                    //                    try { $.unblockUI(); } catch (e) { };
                });
            },
            loadMatches: function (leagueId, date) {
                $.ajax({
                    url: MAIN_PAGE.url.getMatches,
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: $.param({
                        leagueId: leagueId,
                        matchDate: date
                    }),
                    beforeSend: function () {
                        try { $.blockUI(); } catch (e) { }
                    }
                }).done(function (data) {
                    var length = data.length,
                        matchString = [],
                        isSelected = false;

                    matchString.push('<option value="0">' + LANGUAGE.selectMatch + '</option>');
                    for (var i = 0; i < length; i++) {
                        var currentMatch = data[i];
                        matchString.push('<option value="' + currentMatch.Id
                            + '" kick-off-time="' + currentMatch.KickOffTime
                            + '" home-name="' + currentMatch.HomeName
                            + '" away-name="' + currentMatch.AwayName
                            + '" home-id="' + currentMatch.HomeId
                            + '" away-id="' + currentMatch.AwayId
                            + '" kick-off-time-string="' + currentMatch.KickOffTimeString
                            + '" ' + (!isSelected ? 'selected="selected"' : '') + '>'
                            + currentMatch.MatchName
                            + '</option>');
                        isSelected = true;
                    }
                    MAIN_PAGE.elements.$Match.empty().html(matchString.join(''));
                    if (length > 0) {
                        MAIN_PAGE.elements.$BtnLoadChart.trigger('click');
                        MAIN_PAGE.elements.$Match.unbind('change').change(function () {
                            MAIN_PAGE.elements.$BtnLoadChart.trigger('click');
                        }); 
                    }

                }).fail(function (jqXHR, textStatus, errorThrown) {
                    MAIN_PAGE.handleAjaxException(jqXHR, textStatus, errorThrown);
                }).always(function () {
                    try { $.unblockUI(); } catch (e) { }
                });
            }
        };
    }) (jQuery);


    $(function () {
        'use strict';
        MAIN_PAGE.initPage();
    });
}
