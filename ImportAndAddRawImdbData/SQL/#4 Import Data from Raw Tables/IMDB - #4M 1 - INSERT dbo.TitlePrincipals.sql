
-- ======================================================================================================
-- == #4M 1: Import data into: [IMDB].[dbo].[TitlePrincipals]
-- ======================================================================================================
INSERT  INTO
        [IMDB].[dbo].[TitlePrincipals]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[Ordinal]
       ,[PrincipalId]
       ,[ProfessionId]
    )
SELECT  CAST(SUBSTRING(tp.[TitleId], 3, 10) AS INT)   AS [TitleId]
       ,tp.[Ordering]                                 AS [Ordinal]
       ,CAST(SUBSTRING(tp.[NameId], 3, 10) AS INT)    AS [PrincipalId]
       ,ABS(CHECKSUM(tp.[Category]))%10000            AS [ProfessionId]
FROM    [IMDB].[Raw].[title.principals.tsv.gz] AS tp
GO

-- 99,764,730 Rows

/*

SELECT COUNT(*) FROM [IMDB].[dbo].[TitlePrincipals]
GO

*/
