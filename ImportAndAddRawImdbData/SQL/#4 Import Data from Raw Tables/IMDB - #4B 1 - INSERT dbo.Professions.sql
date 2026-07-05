
-- ======================================================================================================
-- == #4B 1:  Import data into: [IMDB].[dbo].[Professions]
-- ======================================================================================================
INSERT  INTO 
        [IMDB].[dbo].[Professions]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [ProfessionId]
       ,[Profession]
    )
SELECT  DISTINCT 
        (ABS(CHECKSUM(p.[value]))%10000)                                              AS [ProfessionId]
       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], N'_', N' '), 2, 100)) AS [Profession]
FROM    [IMDB].[Raw].[name.basics.tsv.gz] AS n
    CROSS APPLY STRING_SPLIT(n.[PrimaryProfession], N',') AS p
WHERE   p.[value] != ''
UNION
SELECT  DISTINCT 
        ABS(CHECKSUM(tp.[Category]))%10000                                                  AS [ProfessionId]
       ,UPPER(LEFT(tp.[Category], 1))+SUBSTRING(REPLACE(tp.[Category], N'_', N' '), 2, 100) AS [Profession]
FROM    [IMDB].[Raw].[title.principals.tsv.gz] tp
WHERE   tp.[Category] != N''
GO

INSERT  INTO 
        [IMDB].[dbo].[Professions]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [ProfessionId]
       ,[Profession]
    )
SELECT  ABS(CHECKSUM('director'))%10000 AS [ProfessionId]
       ,'Director'                      AS [Profession]
UNION
SELECT  ABS(CHECKSUM('writer'))%10000   AS [ProfessionId]
       ,'Writer'                        AS [Profession]
GO
