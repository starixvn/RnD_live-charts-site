USE [bodb_LCC]
GO

/****** Object:  Table [dbo].[ScorePredictionOption]    Script Date: 3/3/2014 9:51:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScorePredictionOption](
	[ScorePredictionOptionId] [int] IDENTITY(1,1) NOT NULL,
	[MatchId] [int] NOT NULL,
	[HomeScore] [int] NOT NULL,
	[AwayScore] [int] NOT NULL,
	[ScorePredictionCount] [int] NOT NULL,
 CONSTRAINT [PK_ScorePredictionOption] PRIMARY KEY CLUSTERED 
(
	[ScorePredictionOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


