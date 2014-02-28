USE [LCC]
GO

/****** Object:  Table [dbo].[VotingItem]    Script Date: 2/28/2014 5:17:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VotingItem](
	[VotingId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[VotingOptionId] [int] NOT NULL,
	[VotingTime] [smalldatetime] NOT NULL,
	[MatchId] [int] NOT NULL,
	[HomeScore] [int] NOT NULL,
	[AwayScore] [int] NOT NULL,
 CONSTRAINT [PK_VotingItem] PRIMARY KEY CLUSTERED 
(
	[VotingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


