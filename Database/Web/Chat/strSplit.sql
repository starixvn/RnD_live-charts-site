USE [bodb_LCC]
GO
/****** Object:  UserDefinedFunction [dbo].[strSplit]    Script Date: 05/17/2014 13:35:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION  [dbo].[strSplit] ( @string nvarchar( 4000), @splitter CHAR( 1) )
RETURNS @res TABLE ( id INT PRIMARY KEY, rank INT, val nvarchar( 4000) )
AS
BEGIN
     IF SUBSTRING ( @string, len ( @string), 1)<>@splitter
          SET @string= @string+@splitter
     DECLARE @start INT, @word nvarchar(4000), @charindex INT, @i INT
     SET @i=1
     SET @start=1
     SET @charindex= CHARINDEX( @splitter, @string, @start)
     WHILE (@charindex <> 0)BEGIN
          SET @word= SUBSTRING( @string, @start, @charindex - @start)
          SET @start= @charindex +1
          SET @charindex= CHARINDEX( @splitter, @string, @start)
          INSERT INTO @res  VALUES ( @start, @i, @word)
          SET @i=@i+1
     END
     RETURN
END