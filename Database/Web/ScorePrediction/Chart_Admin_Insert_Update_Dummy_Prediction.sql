-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ricky
-- Create date: 3/13/2013
-- Description:	Update or Insert Dummy Prediction for Match
-- Revision: 
-- =============================================
ALTER PROCEDURE [dbo].[Chart_Admin_Insert_Update_Dummy_Prediction]
	-- Add the parameters for the stored procedure here
	@MatchId INT,
	@HomeScore INT = 0,
	@AwayScore INT = 0,
	@DummyCount INT,
	@ScorePredictionOptionId INT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @CurrentDummyCount INT = 0
	
	IF (@ScorePredictionOptionId > 0)
		BEGIN
			SELECT @CurrentDummyCount = DummyCount 
			FROM ScorePredictionOption
			WHERE ScorePredictionOptionId = @ScorePredictionOptionId
			
			UPDATE ScorePredictionOption
			SET ScorePredictionCount = ScorePredictionCount - @CurrentDummyCount + @DummyCount, DummyCount = @DummyCount
			WHERE ScorePredictionOptionId = @ScorePredictionOptionId
		END
	ELSE
		BEGIN
			INSERT INTO ScorePredictionOption([MatchId], [HomeScore], [AwayScore], [ScorePredictionCount], [DummyCount])
			VALUES(@MatchId, @HomeScore, @AwayScore, @DummyCount, @DummyCount)
			SET @ScorePredictionOptionId = SCOPE_IDENTITY();
		END
END