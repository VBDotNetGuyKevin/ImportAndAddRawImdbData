-- ======================================================================================================
-- == #4G 1: Import data into: [IMDB].[dbo].[TitleTypes]
-- ======================================================================================================
--                     : [IMDB].[dbo].[Titles]
-------------------------------------------------------------------------
--      Data inconsistency
--          TitleTypes: dbo.[TitleTypes]
--          Titles    : dbo.[Titles]
-------------------------------------------------------------------------
--  Data inconsistency:
--
--  Some titles only exist in the "aka" table.
-------------------------------------------------------------------------
INSERT  INTO 
        [IMDB].[dbo].[TitleTypes]
    (
        [TitleTypeId]
       ,[TitleType]
    )
VALUES 
    (
        0
       ,'Unknown'
    )
GO

--  (1 rows affected)

INSERT  INTO 
        [IMDB].[dbo].[Titles]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
    )
SELECT  TOP (1) WITH TIES
        CAST(SUBSTRING(ta.[TitleId], 3, 10) AS INT) AS [TitleId]
       ,0                                           AS [TitleTypeId]
       ,0                                           AS [IsAdult]
FROM    [IMDB].[Raw].[title.akas.tsv.gz] ta
WHERE   ta.[TitleId] NOT IN 
        (   
            SELECT  [TitleId] 
            FROM    [IMDB].[Raw].[title.basics.tsv.gz]
        )
ORDER   BY ROW_NUMBER() OVER (PARTITION BY ta.[TitleId] ORDER BY ta.[IsOriginalTitle] DESC, ta.[Ordering])
GO

--  35 Rows

/*

SELECT COUNT(*) FROM [IMDB].[dbo].[Titles]

*/
