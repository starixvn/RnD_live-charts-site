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
    me.evenColor = '#0b72bb';
    me.oddColor = '#ff9400';
    me.homeName = homeName || '';
    me.awayName = awayName || '';
    me.drawName = drawName || '';
    me.oddName = oddName || 'Odd';
    me.evenName = evenName || 'Even';

    me.getCorrectScoreResult = function (predictionOptionTotal) {
        var resultArray = [],
            optionsLength = predictionOptionTotal.ScorePredictionOptions.length,
            totalPrediction = predictionOptionTotal.TotalPrediction;

        for (var i = 0; i < optionsLength; i++) {
            var currentOption = predictionOptionTotal.ScorePredictionOptions[i];
            resultArray.push({
                name: '' + currentOption.HomeScore + '-' + currentOption.AwayScore,
                color: me.getColor(currentOption.HomeScore, currentOption.AwayScore, false),
                y: Math.round(currentOption.ScorePredictionCount / totalPrediction * 100)
            });
        }

        return me.sortResult(resultArray);
    };

    me.getHandicapResult = function (predictionOptionTotal) {
        var resultArray = [],
            points = {},
        //optionsLength = predictionOptionTotal.ScorePredictionOptions.length,
            totalPrediction = predictionOptionTotal.TotalPrediction;

        for (var i in predictionOptionTotal.ScorePredictionOptions) {
            var option = predictionOptionTotal.ScorePredictionOptions[i];
            var handicapValue = option.HomeScore - option.AwayScore;
            if (handicapValue > 4) {
                handicapValue = '>+' + handicapValue;
            }
            else if (handicapValue < -4) {
                handicapValue = '<' + handicapValue;
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

        for (i in points) {
            var point = points[i];
            resultArray.push({
                name: point.name,
                color: point.color,
                y: Math.round(point.total / totalPrediction * 100)
            });
        }

        return me.sortResult(resultArray);
    };

    me.getScoreOption = function (points, name) {

        var sortedCategories = points.map(function (point) {
            return point.name;
        });

        return {
            chart: {
                type: 'bar'
            },
            credits: {
                enabled: false
            },
            title: {
                text: 'Demo'
            },
            xAxis: {
                categories: sortedCategories
            },
            yAxis: {
                min: 0,
                max: 100
            },
            tooltip: {
                valueSuffix: ' %'
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            //legend:
            //    {
            //        enabled: false
            //    },
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
                y: point.total
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
                y: point.total
            });
        }

        return resultArray;
    };

    me.getBarOption = function (points, name) {
        return {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            credits: {
                enabled: false
            },
            title: {
                text: 'Demo'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000',
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %'
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

}