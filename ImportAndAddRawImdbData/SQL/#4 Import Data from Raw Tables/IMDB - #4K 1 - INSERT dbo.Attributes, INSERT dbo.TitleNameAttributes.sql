-- ======================================================================================================
-- == #4K 1: Import data into: [IMDB].[dbo].[Attributes]
-- ==                        : [IMDB].[dbo].[TitleNameAttributes]
-- ======================================================================================================

---------------------------------------------------------------------------------------------------------
--      Attributes: Title types
--      Attributes           :  dbo.[Attributes]
--      Title Name Attributes:  dbo.[TitleNameAttributes]
---------------------------------------------------------------------------------------------------------
INSERT  INTO
        [IMDB].[dbo].[Attributes]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [AttributeId]
       ,[Class]
       ,[Attribute]
    )
SELECT  DISTINCT 
    (   SELECT  MAX([AttributeId]) 
        FROM    [IMDB].[dbo].[Attributes]
    )   +
        DENSE_RANK() 
        OVER 
    (   ORDER   BY 
        (
            SELECT a.[value])
        )                                           AS [AttributeId]
       ,'Title types'                               AS [Class]
       ,a.[value]                                   AS [Attribute]
FROM    [IMDB].[Raw].[title.akas.tsv.gz]            AS aka
    CROSS APPLY 
        STRING_SPLIT
            (   aka.[Types]
               ,CHAR(2)         )                   AS a
WHERE   a.[value] NOT IN 
        (
            'imdbDisplay'
           ,'original'
        )
GO

--  6 Rows

INSERT  INTO
        [IMDB].[dbo].[TitleNameAttributes]
        WITH (TABLOCKX, HOLDLOCK)
    (
        [TitleId]
       ,[Ordinal]
       ,[AttributeId]
    )
SELECT  DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]
       ,aka.[Ordering]                                    AS [Ordinal]
       ,attr.[AttributeId]                                AS [AttributeId]
FROM    [IMDB].[Raw].[title.akas.tsv.gz]            AS aka
    CROSS APPLY 
        STRING_SPLIT
            (   aka.[Types]
               ,CHAR(2)         )                   AS a
    INNER JOIN [IMDB].[dbo].[Attributes]            AS attr 
        ON      attr.[Class]     = 'Title types' 
        AND     attr.[Attribute] = a.[value]
GO

--  314,710 Rows
