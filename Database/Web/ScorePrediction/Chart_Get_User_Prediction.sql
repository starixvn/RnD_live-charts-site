USE [bodb_LCC]
GO
/****** Object:  StoredProcedure [dbo].[Chart_Get_User_Voting]    Script Date: 3/3/2014 9:53:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Created by: Ricky
Date: 2014-02-26
Task: Get history prediction of user

Revisions:
	

*/
ALTER PROCEDURE [dbo].[Chart_Get_User_Prediction]
	@MatchId INT,
	@MemberId NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ScorePredictionOptionId, MatchId, HomeScore, AwayScore
	FROM ScorePredictionItem
	WHERE MemberId = @MemberId AND MatchId = @MatchId
END
