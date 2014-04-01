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
-- Create date: 2014/03/31
-- Description:	Get Current Announcement message to post to user
-- =============================================
CREATE PROCEDURE Chart_Chat_Get_Announcement_Message
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [VID] SettingId, [VName] SettingName, [VValue] SettingValue
	FROM Settings
	WHERE VName = 'AnnouncementMessage'
END
GO
