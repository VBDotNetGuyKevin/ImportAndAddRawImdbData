
-- ======================================================================================================
-- == #4Q 1: Import data into: [IMDB].[dbo].[Titles]
-- ==                        : [IMDB].[dbo].[Principals]
-- ======================================================================================================
---------------------------------------------------------------------------------------------------------
--      Titles    : dbo.[Titles]
--      Principals: dbo.[Principals]
---------------------------------------------------------------------------------------------------------
--  Data inconsistency:
-- 
--  Some of these titles and principals
--  are not in their proper respective
--  datasets.
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
        [TitleId] AS [TitleId]
       ,0         AS [TitleTypeId]
       ,0         AS [IsAdult]
FROM    #writers_directors
WHERE   [TitleId] NOT IN (SELECT [TitleId] FROM [IMDB].[dbo].[Titles])
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
        [PrincipalId] AS [PrincipalId]
       ,N'Unknown'    AS [PrimaryName]
FROM    #writers_directors
WHERE   [PrincipalId] NOT IN (SELECT [PrincipalId] FROM [IMDB].[dbo].[Principals])
GO

--  19 Rows
