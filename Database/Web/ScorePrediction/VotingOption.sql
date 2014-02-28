USE [LCC]
GO

/****** Object:  Table [dbo].[VotingOption]    Script Date: 2/28/2014 5:17:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VotingOption](
	[VotingOptionId] [int] IDENTITY(1,1) NOT NULL,
	[MatchId] [int] NOT NULL,
	[HomeScore] [int] NOT NULL,
	[AwayScore] [int] NOT NULL,
	[VotingCount] [int] NOT NULL,
 CONSTRAINT [PK_VotingOption] PRIMARY KEY CLUSTERED 
(
	[VotingOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


