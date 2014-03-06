/// <reference path="../../../Scripts/ScorePrediction/ScorePrediction.js" />

/* File Created: March 3, 2014 */
/*global describe, beforeEach, it, ScorePrediction, expect*/
'use strict';

describe('Score Prediction', function () {
    var scorePrediction;
    var fakeData;

    beforeEach(function () {
        scorePrediction = new ScorePrediction();
        fakeData = { "ScorePredictionOptions": [{
            "ScorePredictionOptionId": 5,
            "MatchId": 4888888,
            "HomeScore": 1,
            "AwayScore": 1,
            "ScorePredictionCount": 14
        }, {
            "ScorePredictionOptionId": 6,
            "MatchId": 4888888,
            "HomeScore": 2,
            "AwayScore": 1,
            "ScorePredictionCount": 11
        }, {
            "ScorePredictionOptionId": 7,
            "MatchId": 4888888,
            "HomeScore": 1,
            "AwayScore": 2,
            "ScorePredictionCount": 1
        }, {
            "ScorePredictionOptionId": 8,
            "MatchId": 4888888,
            "HomeScore": 2,
            "AwayScore": 2,
            "ScorePredictionCount": 1
        }, {
            "ScorePredictionOptionId": 9,
            "MatchId": 4888888,
            "HomeScore": 3,
            "AwayScore": 1,
            "ScorePredictionCount": 2
        }, {
            "ScorePredictionOptionId": 10,
            "MatchId": 4888888,
            "HomeScore": 0,
            "AwayScore": 0,
            "ScorePredictionCount": 9
        }, {
            "ScorePredictionOptionId": 11,
            "MatchId": 4888888,
            "HomeScore": 1,
            "AwayScore": 3,
            "ScorePredictionCount": 0
        }, {
            "ScorePredictionOptionId": 12,
            "MatchId": 4888888,
            "HomeScore": 1,
            "AwayScore": 4,
            "ScorePredictionCount": 0
        }, {
            "ScorePredictionOptionId": 13,
            "MatchId": 4888888,
            "HomeScore": 1,
            "AwayScore": 0,
            "ScorePredictionCount": 1
        }, {
            "ScorePredictionOptionId": 19,
            "MatchId": 4888888,
            "HomeScore": 2,
            "AwayScore": 0,
            "ScorePredictionCount": 1
        }],
            "TotalPrediction": 40
        };
    });

    it('should create correct score result array', function () {
        var result = scorePrediction.getCorrectScoreResult(fakeData);

        //// Largest item have percent equal with 35
        expect(result[0].y).toEqual(35);
        //// Allower than 4% will be combine in one others
        expect(result.length).toEqual(5);
    });

    it('should create 1x2 result array', function () {
        var result = scorePrediction.getCorrectScoreResult(fakeData);

        //// Largest item have percent equal with 35
        expect(result[0].y).toEqual(35);
        //// Allower than 4% will be combine in one others
        expect(result.length).toEqual(5);
    });
});