USE [bodb_LCC]
GO

/****** Object:  Table [dbo].[ChatMessageHistory]    Script Date: 06/13/2014 13:10:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChatMessageHistory](
	[ChatMessageId] [int] NOT NULL,
	[ChatUserId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ChatRoomId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedBy] [int] NOT NULL,
 CONSTRAINT [PK_ChatMessageHistory] PRIMARY KEY CLUSTERED 
(
	[ChatMessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ChatMessageHistory] ADD  CONSTRAINT [DF_ChatMessageHistory_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[ChatMessageHistory] ADD  CONSTRAINT [DF_ChatMessageHistory_DeletedBy]  DEFAULT ((0)) FOR [DeletedBy]
GO


