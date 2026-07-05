
-- ======================================================================================================
-- == #4P 1: Adjusting for data inconsistencies of writers or directors
-- ======================================================================================================
--  Directors and writers
--  (there's a slight overlap with the
--  title principals here)
---------------------------------------------------------------------------------------------------------
SELECT  t.[TitleId]
       ,x.[PrincipalId]
       ,x.[ProfessionId]
INTO    #writers_directors
FROM    [IMDB].[Raw].[title.crew.tsv.gz] AS tc
    CROSS APPLY 
    (
        VALUES (CAST(SUBSTRING(tc.[TitleId], 3, 10) AS INT))
    ) AS t([TitleId])
    CROSS APPLY 
    (
        SELECT  CAST(SUBSTRING(p.[value], 3, 10) AS INT) AS [PrincipalId]
               ,ABS(CHECKSUM('director'))%10000          AS [ProfessionId]
        FROM    STRING_SPLIT(tc.[Directors], ',') AS p
        WHERE   tc.[Directors] != ''
            UNION
        SELECT  CAST(SUBSTRING(w.[value], 3, 10) AS INT) AS [PrincipalId]
               ,ABS(CHECKSUM('writer'))%10000            AS [ProfessionId]
        FROM    STRING_SPLIT(tc.[Writers], ',')   AS w
        WHERE   tc.[Writers] != ''
    ) AS x
    LEFT JOIN [IMDB].[dbo].[TitlePrincipals] AS tp ON
        tp.[TitleId]     = CAST(SUBSTRING(tc.[TitleId], 3, 10) AS INT) 
        AND
        tp.[PrincipalId] = x.[PrincipalId]
WHERE   tp.[TitleId] IS NULL
GO

--  3,401,367 Rows
