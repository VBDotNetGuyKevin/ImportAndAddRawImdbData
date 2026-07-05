
-- ======================================================================================================
-- == #4C 1:  Import data into: [IMDB].[dbo].[PrimaryProfessions]
-- ======================================================================================================
INSERT  INTO 
        [IMDB].[dbo].[PrimaryProfessions]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [PrincipalId]
       ,[ProfessionId]
       ,[Ordinal]
    )
SELECT  (CAST(SUBSTRING(nb.[NameId], 3, 100) AS INT)) AS [PrincipalId]
       ,(ABS(CHECKSUM(p.[value]))%10000)              AS [ProfessionId]
       ,p.[Ordinal]                                   AS [Ordinal]
FROM    [IMDB].[Raw].[name.basics.tsv.gz] AS nb
    CROSS APPLY STRING_SPLIT(nb.primaryProfession, N',', 1) AS p
WHERE   p.[value] != ''
AND     nb.[PrimaryName]    IS NOT NULL
GO
