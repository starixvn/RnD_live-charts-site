USE [bodb_LCC]
GO

/****** Object:  Table [dbo].[MemberBanned]    Script Date: 05/23/2014 14:27:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MemberBanned1](
	[MemberBannedId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[MemberName] [nvarchar](50) NULL,
	[IsBanned] [bit] NOT NULL,
	[UpdateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_MemberBanned1] PRIMARY KEY CLUSTERED 
(
	[MemberBannedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ChatRoom
ADD Muted1 BIT NULL
CONSTRAINT [DF_ChatRoom_Muted1]  DEFAULT ((0)) FOR [Muted1]

ALTER TABLE ChatUserInRoom
ADD Muted1 BIT NULL
CONSTRAINT [DF_ChatUserInRoom_Muted1]  DEFAULT ((0)) FOR [Muted1]



