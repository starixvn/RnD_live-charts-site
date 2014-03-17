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
-- Author:		Ricky.Nguyen
-- Create date: 03/17/2014
-- Description:	Update chat user
-- =============================================
CREATE PROCEDURE Chart_Chat_Update_Chat_User
	@ChatUserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE ChatUser
	SET LastOnlineTime = GETDATE()
	WHERE ChatUserId = @ChatUserId
END
GO
