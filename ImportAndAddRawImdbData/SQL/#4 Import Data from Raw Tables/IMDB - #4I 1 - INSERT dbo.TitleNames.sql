-- ======================================================================================================
-- == #4I 1: Import data into: [IMDB].[dbo].[TitleNames]
-- ======================================================================================================
--      TitleNames: dbo.[TitleNames]
---------------------------------------------------------------------------------------------------------
INSERT  INTO
        [IMDB].[dbo].[TitleNames]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[Ordinal]
       ,[Region]
       ,[language]
       ,[IsOriginal]
       ,[Title]
    )
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT)  AS [TitleId]
       ,[Ordering]                                AS [Ordinal]
       ,[Region]                                  AS [Region]
       ,[Language]                                AS [Language]
       ,(
            CASE 
                WHEN [Ordering] = MIN((CASE WHEN ISNULL([IsOriginalTitle], 1)=1 THEN [Ordering] END)) OVER (PARTITION BY [TitleId]) 
                THEN 1 
            ELSE 0 
            END
        )                                         AS [IsOriginal]
       ,[Title]                                   AS [Title]
FROM    [IMDB].[Raw].[title.akas.tsv.gz]
GO

--  57,452,364 Rows

/*

SELECT COUNT(*) FROM [IMDB].[dbo].[TitleNames]
SELECT TOP(1000) * FROM [IMDB].[dbo].[TitleNames]
GO

*/
