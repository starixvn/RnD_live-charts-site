USE [bodb_LCC]
GO

/****** Object:  Table [dbo].[ScorePredictionItem]    Script Date: 3/3/2014 9:49:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScorePredictionItem](
	[ScorePredictionId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [nvarchar](50) NOT NULL,
	[ScorePredictionOptionId] [int] NOT NULL,
	[ScorePredictionTime] [smalldatetime] NOT NULL,
	[MatchId] [int] NOT NULL,
	[HomeScore] [int] NOT NULL,
	[AwayScore] [int] NOT NULL,
 CONSTRAINT [PK_ScorePredictionItem] PRIMARY KEY CLUSTERED 
(
	[ScorePredictionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


