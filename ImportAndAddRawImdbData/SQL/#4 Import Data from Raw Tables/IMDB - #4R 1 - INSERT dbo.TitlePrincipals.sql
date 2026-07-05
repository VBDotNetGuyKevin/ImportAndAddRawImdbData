
-- ======================================================================================================
-- == #4R 1: Import data into: [IMDB].[dbo].[TitlePrincipals]
-- ======================================================================================================

---------------------------------------------------------------------------------------------------------
--  #21 Now (finally) the actual title principals
--      Title Principals:   dbo.[TitlePrincipals]
---------------------------------------------------------------------------------------------------------
INSERT  INTO
        [IMDB].[dbo].[TitlePrincipals]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[Ordinal]
       ,[PrincipalId]
       ,[ProfessionId]
    )
SELECT  x.[TitleId]                                     AS [TitleId]
       ,ISNULL(o.[Ordinal], 0) + ROW_NUMBER()           
            OVER (   
                    PARTITION BY    x.[TitleId]         
                    ORDER     BY    x.[ProfessionId]    
                                   ,x.[PrincipalId]     
                 )                                      AS [Ordinal]
       ,x.[PrincipalId]                                 AS [PrincipalId]
       ,x.[ProfessionId]                                AS [ProfessionId]
FROM    #writers_directors AS x
    LEFT JOIN 
        (
            SELECT  [TitleId]       AS [TitleId]
                   ,MAX([Ordinal])  AS [Ordinal]
            FROM    [IMDB].[dbo].[TitlePrincipals]
            GROUP   BY [TitleId]
        )   AS o 
            ON x.[TitleId] = o.[TitleId];

DROP TABLE #writers_directors
