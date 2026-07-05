-- ======================================================================================================
-- == #4L 1:  Data Inconsistencies
-- ======================================================================================================
--     Import data into: [IMDB].[dbo].[Titles]
--                     : [IMDB].[dbo].[Principals]
---------------------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------------------
--  #15 Data inconsistency
--      Titles    : dbo.[Titles]
--      Principals: dbo.[Principals]
---------------------------------------------------------------------------------------------------------
--  Data inconsistency:
-- 
--  Some titles and principals only
--  exist in the "title.principals"
--  dataset.
---------------------------------------------------------------------------------------------------------
INSERT  INTO
        [IMDB].[dbo].[Titles]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
    )
SELECT  DISTINCT 
        CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]
       ,0                                        AS [TitleTypeId]
       ,0                                        AS [IsAdult]
FROM    [IMDB].[Raw].[title.principals.tsv.gz]
WHERE   CAST(SUBSTRING([TitleId], 3, 10) AS INT) NOT IN 
            (
                SELECT  [TitleId] 
                FROM    [IMDB].[dbo].[Titles]
            )
GO

--  0 Rows

INSERT  INTO
        [IMDB].[dbo].[Principals]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [PrincipalId]
       ,[PrimaryName]
    )
SELECT  DISTINCT 
        CAST(SUBSTRING([NameId], 3, 10) AS INT) AS [PrincipalId]
       ,N'Unknown'                              AS [PrimaryName]
FROM    [IMDB].[Raw].[title.principals.tsv.gz]
WHERE   CAST(SUBSTRING([NameId], 3, 10) AS INT) NOT IN 
            (
                SELECT  [PrincipalId] 
                FROM    [IMDB].[dbo].[Principals]
            )
GO

--  1,666 Rows
