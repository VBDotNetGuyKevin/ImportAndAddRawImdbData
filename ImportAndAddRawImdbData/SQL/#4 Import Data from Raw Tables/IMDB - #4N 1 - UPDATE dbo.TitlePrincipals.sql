
-- ======================================================================================================
-- == #4N 1: Import "known for" data into: [IMDB].[dbo].[TitlePrincipals]
-- ======================================================================================================
UPDATE  tp
    SET tp.[KnownForOrdinal] = k.[Ordinal]
FROM    [IMDB].[Raw].[name.basics.tsv.gz]                   AS n
    CROSS APPLY 
        STRING_SPLIT(   n.[KnownForTitles]
                       ,','
                       ,1                   )               AS k
    INNER JOIN [IMDB].[dbo].[TitlePrincipals]               AS tp 
        WITH    (TABLOCKX, HOLDLOCK) 
            ON  CAST(SUBSTRING(n.[NameId], 3, 10) AS INT) = tp.[PrincipalId] 
            AND CAST(SUBSTRING(k.[value], 3, 10) AS INT)  = tp.[TitleId]
WHERE   k.[value] != ''
GO

--   Rows
