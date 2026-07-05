-- ======================================================================================================
-- == #4H 1:  Import data into: [IMDB].[dbo].[TitleGenres]
-- ======================================================================================================
--      TitleGenres: dbo.[TitleGenres]
---------------------------------------------------------------------------------------------------------
INSERT  INTO 
        [IMDB].[dbo].[TitleGenres]
        WITH (TABLOCKX, HOLDLOCK) 
        (
            [TitleId]
           ,[GenreId]
        )
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]
       ,ABS(CHECKSUM(p.[value]))%32000           AS [GenreId]
FROM    [IMDB].[Raw].[title.basics.tsv.gz]      AS t
    CROSS APPLY STRING_SPLIT(t.[Genres], ',')   AS p
WHERE   p.[value] != ''
GO

--  19,547,408 Rows
