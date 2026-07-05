
--  #4A 1:  Principals: dbo.[Principals]

INSERT  INTO 
        [IMDB].[dbo].[Principals]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [PrincipalId]
       ,[PrimaryName]
       ,[BirthYear]
       ,[DeathYear]
    )
SELECT  CAST(SUBSTRING(nb.[NameId], 3, 100) AS INT) AS [PrincipalId]
       ,nb.[PrimaryName]                            AS [PrimaryName]
       ,DATEFROMPARTS(nb.[BirthYear], 1, 1)         AS [BirthYear]
       ,DATEFROMPARTS(nb.[DeathYear], 12, 31)       AS [DeathYear]
FROM    [IMDB].[Raw].[name.basics.tsv.gz] nb
WHERE   nb.[PrimaryName] IS NOT NULL
GO
