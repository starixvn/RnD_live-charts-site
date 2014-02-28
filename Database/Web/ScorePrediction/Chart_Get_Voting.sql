USE [LCC]
GO
/****** Object:  StoredProcedure [dbo].[Chart_Insert_Or_Update_Voting_Option]    Script Date: 2/28/2014 11:19:27 AM ******/
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
ALTER PROCEDURE [dbo].[Chart_Get_Voting]
	@MatchId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT VotingOptionId, MatchId, HomeScore, AwayScore, VotingCount
	FROM VotingOption (NOLOCK)
	WHERE MatchId = @MatchId
END
