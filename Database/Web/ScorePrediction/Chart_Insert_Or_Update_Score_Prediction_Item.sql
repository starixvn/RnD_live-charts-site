USE [bodb_LCC]
GO
/****** Object:  StoredProcedure [dbo].[Chart_Insert_Or_Update_Score_Prediction_Item]    Script Date: 3/3/2014 9:56:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Created by: Ricky
Date: 2014-02-26
Task: Insert or update new Score predicton option then insert new Score Prediction Item

Revisions:
	

*/
ALTER PROCEDURE [dbo].[Chart_Insert_Or_Update_Score_Prediction_Item]
	@MatchId INT,
	@MemberId NVARCHAR(50),
	@HomeScore INT,
	@AwayScore INT,
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ScorePredictedId INT = 0,
			@CurrentScorePredictionOption INT = 0,
			@ScorePredictedOptionId INT = 0
	
	-- Check if current ScorePrediction is same with ScorePrediction in DB of member
	IF(EXISTS(	SELECT TOP 1 * 
				FROM ScorePredictionItem (NOLOCK)
				WHERE MatchId = @MatchId AND HomeScore = @HomeScore AND AwayScore = @AwayScore AND MemberId = @MemberId))
				BEGIN
					SET @ErrCode = 1000
					SET @ErrMsg = 'There is no update'

					RETURN
				END

	-- Check user already vote or not
	SELECT @ScorePredictedId = ScorePredictionId, @ScorePredictedOptionId = ScorePredictionOptionId
	FROM ScorePredictionItem (NOLOCK)
	WHERE MemberId = @MemberId AND MatchId = @MatchId

	
	DECLARE @ExistVoteOption INT = (SELECT ScorePredictionOptionId
						FROM ScorePredictionOption (NOLOCK) 
						WHERE HomeScore = @HomeScore AND AwayScore = @AwayScore AND MatchId = @MatchId)
	
	IF (@ExistVoteOption > 0)
		BEGIN
			UPDATE ScorePredictionOption
			SET ScorePredictionCount = ScorePredictionCount + 1, @CurrentScorePredictionOption = ScorePredictionOptionId
			WHERE HomeScore = @HomeScore AND AwayScore = @AwayScore AND MatchId = @MatchId
		END
	ELSE
		BEGIN
			INSERT INTO ScorePredictionOption([MatchId], [HomeScore], [AwayScore], [ScorePredictionCount])
			VALUES(@MatchId, @HomeScore, @AwayScore, 1)
			SET @CurrentScorePredictionOption = SCOPE_IDENTITY();
		END

	-- Insert new or update ScorePrediction option depend on user already vote or not
	IF (@ScorePredictedId > 0)
		BEGIN
			UPDATE ScorePredictionOption
			SET ScorePredictionCount = ScorePredictionCount - 1
			WHERE ScorePredictionOptionId = @ScorePredictedOptionId
		END


	-- Update or insert ScorePrediction item
	IF(@CurrentScorePredictionOption > 0)
		BEGIN
			IF(@ScorePredictedId > 0)
				BEGIN
					UPDATE ScorePredictionItem
					SET ScorePredictionOptionId = @CurrentScorePredictionOption, ScorePredictionTime = GETDATE(), HomeScore = @HomeScore, AwayScore = @AwayScore
					WHERE ScorePredictionId = @ScorePredictedId
				END
			ELSE
				BEGIN
					INSERT INTO ScorePredictionItem([ScorePredictionOptionId],[ScorePredictionTime],[MemberId], [MatchId], [HomeScore], [AwayScore])
					VALUES(@CurrentScorePredictionOption, GETDATE(), @MemberId, @MatchId, @HomeScore, @AwayScore)
				END
		END
	ELSE
		BEGIN
			SET @ErrCode = 1
			SET @ErrMsg = 'Cannot insert/update ScorePrediction item' 
		END
END
