USE [bodb_LCC]
GO
/****** Object:  StoredProcedure [dbo].[Chart_Chat_Insert_Chat_User]    Script Date: 3/17/2014 3:19:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ricky.Nguyen
-- Create date: 03/17/2014
-- Description: Update chat room.	
-- Revision:
-- =============================================

CREATE PROCEDURE [dbo].[Chart_Chat_Update_Chat_Room]
	@ChatRoomId INT,
	@WelcomeMessage NVARCHAR(500)
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE ChatRoom
	SET WelcomeMessage = @WelcomeMessage
	WHERE ChatRoomId = @ChatRoomId

END

