ALTER TABLE [dbo].[ChatUser]
ADD IsResetChatName BIT NULL
ALTER TABLE [dbo].[ChatUser] ADD  CONSTRAINT [DF_ChatRoom_IsResetChatName]  DEFAULT ((0)) FOR [IsResetChatName]
GO

ALTER TABLE [dbo].[ChatRoom]
ADD MutePeriod INT
ALTER TABLE [dbo].[ChatRoom] ADD  CONSTRAINT [DF_ChatRoom_MutePeriod]  DEFAULT ((0)) FOR [MutePeriod]
GO

ALTER TABLE [dbo].[ChatRoom]
ADD StartMuteTime SMALLDATETIME
GO

ALTER TABLE [dbo].[ChatUserInRoom]
ADD MutePeriod INT
ALTER TABLE [dbo].[ChatUserInRoom] ADD  CONSTRAINT [DF_ChatUserInRoom_MutePeriod]  DEFAULT ((0)) FOR [MutePeriod]
GO

ALTER TABLE [dbo].[ChatUserInRoom]
ADD StartMuteTime SMALLDATETIME
GO