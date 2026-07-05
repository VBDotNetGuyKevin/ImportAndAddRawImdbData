-- ======================================================================================================
-- == #4J 1: Import data into: [IMDB].[dbo].[Attributes]
-- ==                        : [IMDB].[dbo].[TitleNameAttributes]
-- ======================================================================================================
---------------------------------------------------------------------------------------------------------
--      Attributes: title attribute
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
        DENSE_RANK() OVER (ORDER BY (SELECT a.[value])) AS [AttributeId]
       ,'Title attribute'                               AS [Class]
       ,a.[value]                                       AS [Attribute]
FROM    [IMDB].[Raw].[title.akas.tsv.gz]                AS aka
    CROSS APPLY STRING_SPLIT(aka.[Attributes], CHAR(2)) AS a
WHERE   a.[value] != ''
GO

--  163 Rows

INSERT  INTO
        [IMDB].[dbo].[TitleNameAttributes]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[Ordinal]
       ,[AttributeId]
    )
SELECT  DISTINCT 
        CAST(SUBSTRING([TitleId], 3, 10) AS INT)    AS [TitleId]
       ,aka.[Ordering]                              AS [Ordinal]
       ,attr.[AttributeId]                          AS [AttributeId]
FROM    [IMDB].[Raw].[title.akas.tsv.gz] AS aka
    CROSS APPLY STRING_SPLIT(aka.[Attributes], CHAR(2)) AS a
    INNER JOIN [IMDB].[dbo].[Attributes]            AS attr ON  attr.[Class]     = 'Title attribute' 
                                                            AND attr.[Attribute] = a.[value]
GO

--  311,606 Rows
