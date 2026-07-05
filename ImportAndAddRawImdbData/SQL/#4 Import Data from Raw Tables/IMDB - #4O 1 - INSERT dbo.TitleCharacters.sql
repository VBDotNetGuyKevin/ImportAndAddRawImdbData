-- ======================================================================================================
-- == #4O 1: Import data into: [IMDB].[dbo].[TitleCharacters]
-- ======================================================================================================

INSERT  INTO
        [IMDB].[dbo].[TitleCharacters]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[PrincipalId]
       ,[Character]
    )
SELECT  CAST(SUBSTRING(tp.[TitleId], 3, 10) AS INT) AS [TitleId]
       ,CAST(SUBSTRING(tp.[NameId], 3, 10) AS INT)  AS [PrincipalId]
       ,ch.[value]                                  AS [Character]
FROM    [IMDB].[Raw].[title.principals.tsv.gz] AS tp
    CROSS APPLY 
        STRING_SPLIT
            (   REPLACE
                (   REPLACE
                    (   
                        SUBSTRING(tp.[Characters], 3, LEN(tp.[Characters])-4)
                       ,N'"",""'
                       ,NCHAR(9)
                    )
                   ,N'\""'
                   ,N'""'
                )
               ,NCHAR(9)
            ) AS ch
GO

--  10,642,136  Rows
