USE [bodb_LCC]
GO
/****** Object:  StoredProcedure [dbo].[Chart_Get_Voting]    Script Date: 3/3/2014 9:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Created by: Ricky
Date: 2014-02-26
Task: Get total vote of voting options

Revisions:
	

*/
ALTER PROCEDURE [dbo].[Chart_Get_All_Score_Prediction]
	@MatchId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ScorePredictionOptionId, MatchId, HomeScore, AwayScore, ScorePredictionCount
	FROM ScorePredictionOption (NOLOCK)
	WHERE MatchId = @MatchId
END
