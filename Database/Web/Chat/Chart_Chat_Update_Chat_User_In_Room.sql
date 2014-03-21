USE [bodb_LCC]
GO
/****** Object:  StoredProcedure [dbo].[Chart_Chat_Insert_Chat_Room]    Script Date: 03/21/2014 18:04:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ricky.Nguyen
-- Create date: 03/17/2014
-- Description: Insert new chat room.	
-- Revision:
-- =============================================

ALTER PROCEDURE [dbo].[Chart_Chat_Update_Chat_User_In_Room]
	@ChatUserInRoomId INT,
	@IsLeft BIT
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE ChatUserInRoom
	SET Isleft = @IsLeft, LastOnlineTime = GETDATE()
	WHERE ChatUserInRoomId = @ChatUserInRoomId

END

