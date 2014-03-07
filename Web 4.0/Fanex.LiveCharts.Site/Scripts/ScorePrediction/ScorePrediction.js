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
    me.otherColor = '#122121';
    me.homeName = homeName || '';
    me.awayName = awayName || '';
    me.drawName = drawName || '';
    me.oddName = oddName || 'Odd';
    me.evenName = evenName || 'Even';
    me.otherName = 'Others';
    me.minPercent = 4;

    me.getCorrectScoreResult = function (predictionOptionTotal) {
        var resultArray = [],
            optionsLength = predictionOptionTotal.ScorePredictionOptions.length,
            totalPrediction = predictionOptionTotal.TotalPrediction,
            total = 0,
            currentPercent;

        for (var i = 0; i < optionsLength; i++) {
            var currentOption = predictionOptionTotal.ScorePredictionOptions[i];
//            if (i === optionsLength - 1) {
//                currentPercent = parseFloat((100 - total).toFixed(2));
//            }
//            else {
                currentPercent = parseFloat((currentOption.ScorePredictionCount / totalPrediction * 100).toFixed(2));
            //}
            resultArray.push({
                name: '' + currentOption.HomeScore + '-' + currentOption.AwayScore,
                color: me.getColor(currentOption.HomeScore, currentOption.AwayScore, false),
                y: currentPercent
            });

            total += currentPercent;
        }

        return me.combineLowerOptionToOther(me.sortResult(resultArray));
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
        var currentPercent;
        var total = 0;
        for (i in points) {
            var point = points[i];
            currentPercent = parseFloat((point.total / totalPrediction * 100).toFixed(2));


            resultArray.push({
                name: point.name,
                color: point.color,
                y: currentPercent
            });

            total += currentPercent;
        }

//        if (total > 100) {
//            resultArray[resultArray.length -1].y = 
//        }

        return me.combineLowerOptionToOther(me.sortResult(resultArray));
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
                marginBottom: 20
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
                        format: '{y} %'
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
                text: null
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

    me.combineLowerOptionToOther = function (points) {
        var otherOption = {
            name: me.otherName,
            color: me.otherColor,
            y: 0
        };

        for (var i = points.length - 1; i >= 0; i--) {
            var point = points[i];
            if (point.y < me.minPercent) {
                otherOption.y += point.y;
                points.splice(i, 1);
            }
        }

        if (otherOption.y > 0) {
            otherOption.y = parseFloat((otherOption.y).toFixed(2));
            points.push(otherOption);
        }

        return points;
    };

}