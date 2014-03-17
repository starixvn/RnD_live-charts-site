/* File Created: March 3, 2014 */
'use strict';
/* exported ScorePrediction */
function ScorePrediction (
    homeName,
    awayName,
    drawName,
    oddName,
    evenName) {

    var me = this;

    me.homeColor = '#0b72bb';
    me.awayColor = '#ff9400';
    me.drawColor = '#C3D69B';
    me.evenColor = '#92CDDD';
    me.oddColor = '#E6B9B8';
    me.otherColor = '#BFBFBF';
    me.homeName = homeName || '';
    me.awayName = awayName || '';
    me.drawName = drawName || '';
    me.oddName = oddName || 'Odd';
    me.evenName = evenName || 'Even';
    me.otherName = 'Others';
    me.minPercent = 4;
    me.isAdminSite = false;


    me.getCorrectScoreResult = function (predictionOptionTotal) {
        var resultArray = [],
            optionsLength = predictionOptionTotal.ScorePredictionOptions.length,
            totalPrediction = predictionOptionTotal.TotalPrediction,
            currentPercent;

        for (var i = 0; i < optionsLength; i++) {
            var currentOption = predictionOptionTotal.ScorePredictionOptions[i];
            currentPercent = me.formatValue(currentOption.ScorePredictionCount / totalPrediction * 100);
            resultArray.push({
                name: '' + currentOption.HomeScore + '-' + currentOption.AwayScore,
                color: me.getColor(currentOption.HomeScore, currentOption.AwayScore, false),
                y: currentPercent,
                customData: {
                    currentPrediction: currentOption.ScorePredictionCount,
                    totalPrediction: totalPrediction
                }
            });

        }

        return me.combineLowerOptionToOther(me.sortResult(resultArray));
    };

    me.getHandicapResult = function (predictionOptionTotal) {
        var resultArray = [],
            points = {},
            totalPrediction = predictionOptionTotal.TotalPrediction;

        for (var i in predictionOptionTotal.ScorePredictionOptions) {
            var option = predictionOptionTotal.ScorePredictionOptions[i];
            var handicapValue = option.HomeScore - option.AwayScore;
            if (handicapValue > 4) {
                handicapValue = '>+4';
            }
            else if (handicapValue < -4) {
                handicapValue = '<-4';
            }
            else {
                handicapValue = handicapValue > 0 ? ('+' + handicapValue) : ('' + handicapValue);
            }

            if (typeof (points[handicapValue]) === 'undefined') {
                points[handicapValue] = {
                    total: 0,
                    color: me.getColor(option.HomeScore, option.AwayScore, false),
                    name: handicapValue
                };
            }

            points[handicapValue].total += option.ScorePredictionCount;
        }

        var currentPercent;
        for (i in points) {
            var point = points[i];
            currentPercent = me.formatValue((point.total / totalPrediction) * 100);


            resultArray.push({
                name: point.name,
                color: point.color,
                y: currentPercent,
                customData: {
                    currentPrediction: point.total,
                    totalPrediction: totalPrediction
                }
            });
        }
        return me.sortResult(resultArray);
    };

    me.getScoreOption = function (points, name) {

        var sortedCategories = [];
        for (var i in points) {
            var point = points[i];
            sortedCategories.push(point.name);
        }

        return {
            chart: {
                type: 'bar',
                marginRight: 42,
                marginBottom: 20,
                borderRadius: 0
            },
            credits: {
                enabled: false
            },
            title: {
                text: null
            },
            xAxis: {
                categories: sortedCategories
            },
            yAxis: {
                min: 0,
                max: 100,
                labels: {
                    formatter: function () {
                        return '' + this.value + '%';
                    }
                }
            },
            tooltip: {
                valueSuffix: ' %'
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            var data = this,
                                textFormat = '' + data.y + '%';

                            if (me.isAdminSite) {
                                textFormat += ' ('
                                    + data.point.customData.currentPrediction + '/'
                                    + data.point.customData.totalPrediction + ')';
                            }
                            return textFormat;
                        }
                    }
                }
            },
            legend:
                {
                    enabled: false
                },
            series: [
                {
                    name: name,
                    data: points
                }
            ]
        };
    };

    me.getOddEvenResult = function (predictionOptionTotal) {
        var resultArray = [], points = {};

        for (var i in predictionOptionTotal.ScorePredictionOptions) {
            var option = predictionOptionTotal.ScorePredictionOptions[i],
                totalScore = option.HomeScore + option.AwayScore,
                optionName = '';

            if (totalScore % 2 === 0) {
                optionName = me.evenName;
            }
            else {
                optionName = me.oddName;
            }

            if (typeof (points[optionName]) === 'undefined') {
                points[optionName] = {
                    total: 0,
                    color: me.getColor(option.HomeScore, option.AwayScore, true),
                    name: optionName
                };
            }

            points[optionName].total += option.ScorePredictionCount;
        }

        for (i in points) {
            var point = points[i];
            resultArray.push({
                name: point.name,
                color: point.color,
                y: point.total,
                customData: {
                    currentPrediction: point.total,
                    totalPrediction: predictionOptionTotal.TotalPrediction
                }
            });
        }

        return resultArray;
    };

    me.get1X2Result = function (predictionOptionTotal) {
        var resultArray = [], points = {};

        for (var i in predictionOptionTotal.ScorePredictionOptions) {
            var option = predictionOptionTotal.ScorePredictionOptions[i],
                handicapValue = option.HomeScore - option.AwayScore,
                optionName = '';

            if (handicapValue > 0) {
                optionName = me.homeName;
            }
            else if (handicapValue < 0) {
                optionName = me.awayName;
            }
            else {
                optionName = me.drawName;
            }

            if (typeof (points[optionName]) === 'undefined') {
                points[optionName] = {
                    total: 0,
                    color: me.getColor(option.HomeScore, option.AwayScore, false),
                    name: optionName
                };
            }

            points[optionName].total += option.ScorePredictionCount;
        }

        for (i in points) {
            var point = points[i];
            resultArray.push({
                name: point.name,
                color: point.color,
                y: point.total,
                customData: {
                    currentPrediction: point.total,
                    totalPrediction: predictionOptionTotal.TotalPrediction
                }
            });
        }

        return resultArray;
    };

    me.getBarOption = function (points, name) {
        return {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                borderRadius: 0
            },
            credits: {
                enabled: false
            },
            title: {
                text: null
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000',
                        //format: '<b>{point.name}</b>: {point.percentage:.2f} %'
                        formatter: function () {
                            var data = this,
                                textData = '<b>' + data.point.name + '</b>: ' + me.formatValue(data.percentage) + '%';
                            if (me.isAdminSite) {
                                textData += ' ('
                                    + data.point.customData.currentPrediction + '/'
                                    + data.point.customData.totalPrediction + ')';
                            }

                            return textData;
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                name: name,
                data: points
            }]
        };
    };

    me.getColor = function (homeScore, awayScore, forOddEven) {
        if (forOddEven) {
            return (homeScore + awayScore) % 2 === 0 ? me.evenColor : me.oddColor;
        }
        else {
            if (homeScore > awayScore) {
                return me.homeColor;
            }
            else if (homeScore === awayScore) {
                return me.drawColor;
            }
            else {
                return me.awayColor;
            }
        }
    };

    me.sortResult = function (resultArray) {
        return resultArray.sort(function (pointA, pointB) {
            return pointB.y - pointA.y;
        });
    };

    me.combineLowerOptionToOther = function (points) {
        var otherOption = {
            name: me.otherName,
            color: me.otherColor,
            y: 0,
            customData: {
                currentPrediction: 0,
                totalPrediction: 0
            }
        };

        for (var i = points.length - 1; i >= 0; i--) {
            var point = points[i];
            if (point.y < me.minPercent) {
                otherOption.y += point.y
                otherOption.customData.currentPrediction += point.customData.currentPrediction;
                if (otherOption.customData.totalPrediction === 0) {
                    otherOption.customData.totalPrediction = point.customData.totalPrediction;
                }
                points.splice(i, 1);
            }
        }

        if (otherOption.y > 0) {
            otherOption.y = me.formatValue(otherOption.y);
            points.push(otherOption);
        }

        return points;
    };


    me.formatValue = function (value) {
        return parseFloat(value.toFixed(2));
    }
}