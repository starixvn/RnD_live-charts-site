USE [bodb_LCC]
GO

/****** Object:  Table [dbo].[ChatMessage]    Script Date: 06/12/2014 12:51:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChatMessageHistory](
	[ChatMessageId] [int] IDENTITY(1,1) NOT NULL,
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


