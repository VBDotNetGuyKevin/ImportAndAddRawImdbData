-- ======================================================================================================
-- == #4F 1:  Import data into: [IMDB].[dbo].[Titles]
-- ======================================================================================================
--       Titles: dbo.[Titles]
-- -------------------------------------------------------------------------
INSERT  INTO 
        [IMDB].[dbo].[Titles]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
       ,[StartYear]
       ,[EndYear]
       ,[Runtime]
    )
SELECT  CAST(SUBSTRING(tb.[TitleId], 3, 10) AS INT)                    AS [TitleId]
       ,ABS(CHECKSUM(tb.[TitleType]))%100                              AS [TitleTypeId]
       ,tb.[IsAdult]                                                   AS [IsAdult]
       ,DATEFROMPARTS(tb.[StartYear], 1, 1)                            AS [StartYear]
       ,DATEFROMPARTS(tb.[EndYear], 12, 31)                            AS [EndYear]
       ,DATEADD(MINUTE, tb.[RuntimeMinutes], CAST('00:00' AS TIME(0))) AS [Runtime]
FROM    [IMDB].[Raw].[title.basics.tsv.gz] tb
GO

--  12,541,389 Rows

/*

SELECT COUNT(*)    FROM [IMDB].[dbo].[Titles]
SELECT TOP(1000) * FROM [IMDB].[dbo].[Titles]
GO

*/
