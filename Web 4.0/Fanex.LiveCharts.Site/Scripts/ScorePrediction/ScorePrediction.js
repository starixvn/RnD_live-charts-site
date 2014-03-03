/* File Created: March 3, 2014 */
'use strict';

function ScorePrediction() {
    var me = this;

    me.homeColor = '#0b72bb';
    me.awayColor = '#ff9400';
    me.drawColor = '#C3D69B';

    me.getCorrectScoreResult = function (predictionOptionTotal) {
        var resultArray = [],
            optionsLength = predictionOptionTotal.ScorePredictionOptions.length,
            totalPrediction = predictionOptionTotal.TotalPrediction;

        for (var i = 0; i < optionsLength; i++) {
            var currentOption = predictionOptionTotal.ScorePredictionOptions[i];
            resultArray.push({
                name: '' + currentOption.HomeScore + '-' + currentOption.AwayScore,
                color: me.getColor(currentOption.HomeScore, currentOption.AwayScore),
                y: Math.round(currentOption.ScorePredictionCount / totalPrediction * 100)
            });
        }

        return me.sortResult(resultArray);
    };

    me.getScoreOption = function (series) {
        return {
            chart: {
                type: 'bar'
            },
            title: {
                text: 'Demo'
            },
            yAxis: {
                min: 0,
                labels: {
                    overflow: 'justify'
                }
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
            series: series
        };
    };

    me.getOddEvenResult = function (predictionOptionTotal) {

    }

    me.getHandicapResult = function (predictionOptionTotal) {

    }

    me.get1x2Result = function (predictionOptionTotal) {

    }

    me.getColor = function (homeScore, awayScore) {
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

    me.sortResult = function (resultArray) {
        var dataSorted = resultArray.sort(function (optionA, optionB) {
            return optionA.ScorePredictionCount - optionB.ScorePredictionCount;
        });

        var xAxisArray
        for (var i = 0; i < dataSorted.length; i++) {

        }
    };
}