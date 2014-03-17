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
-- Create date: 03/17/2014
-- Description:	Insert Chat Message
-- =============================================
CREATE PROCEDURE Chart_Chat_Insert_Chat_Message
	@ChatUserId INT,
	@Content NVARCHAR(MAX) = '',
	@ChatRoomId INT,
	@ChatMessageId INT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO ChatMessage([ChatUserId],[ChatRoomId],[Content],[CreateTime], [IsDeleted], [DeleteBy])
	VALUES(@ChatUserId, @ChatRoomId, @Content, GETDATE(), 0, '')
	SET @ChatMessageId = SCOPE_IDENTITY();
END
GO
