-- ======================================================================================================
-- == #4D 1:  Import data into: [IMDB].[dbo].[Genres]
-- ======================================================================================================
--      Genres: dbo.[Genres]

INSERT  INTO 
        [IMDB].[dbo].[Genres]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [GenreId]
       ,[Genre]
    )
SELECT  DISTINCT 
        (ABS(CHECKSUM(p.[value]))%32000)                                            AS [GenreId]
       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], '_', ' '), 2, 100)) AS [Genre]
FROM    [IMDB].[Raw].[title.basics.tsv.gz] AS t
    CROSS APPLY STRING_SPLIT(t.[Genres], ',') AS p
WHERE   p.[value] != ''
GO

--  28 Rows

/*

SELECT  *
FROM    [IMDB].[dbo].[Genres]
ORDER   BY  [Genre]

GO
*/
