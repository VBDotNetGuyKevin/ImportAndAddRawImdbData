USE [IMDB];

-------------------------------------------------------------------------------
--  #4  Principals: dbo.[Principals]
-------------------------------------------------------------------------------
/*
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
*/
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
WHERE   nb.[PrimaryName] IS NOT NULL;                                  

-- 15,380,979 Row(s) affected
-- 20 seconds

-------------------------------------------------------------------------------
--  #5  Professions : dbo.[Professions]
-------------------------------------------------------------------------------
/*
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
*/
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
FROM    [IMDB].[Raw].[name.basics.tsv.gz]  AS n                                    
    CROSS APPLY                                                                    
        STRING_SPLIT                                                               
            (                                                                      
                n.[PrimaryProfession]                                              
               ,N','                                                               
            )                              AS p                                    
WHERE   p.[value] != ''                                                            
UNION                                                                              
SELECT  DISTINCT                                                                   
        ABS(CHECKSUM(tp.[Category]))%10000                                                  AS [ProfessionId]  
       ,UPPER(LEFT(tp.[Category], 1))+SUBSTRING(REPLACE(tp.[Category], N'_', N' '), 2, 100) AS [Profession]    
FROM    [IMDB].[Raw].[title.principals.tsv.gz] tp                                  
WHERE   tp.[Category] != N'';                                                      

-- 47 Row(s) affected
-- 23 seconds

/*
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
*/

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
       ,'Writer'                        AS [Profession];   

-- 2 Row(s) affected
-- 0 seconds

/*
    SELECT  *
    FROM    [IMDB].[dbo].[Professions]
*/

--  49 Row(s) affected
-- ? seconds

-------------------------------------------------------------------------------
--  #6  Primary professions: dbo.[PrimaryProfessions]
-------------------------------------------------------------------------------
/*
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
*/
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
FROM    [IMDB].[Raw].[name.basics.tsv.gz]  AS nb                           
    CROSS APPLY                                
        STRING_SPLIT                           
            (                                  
                nb.primaryProfession           
               ,N','                           
               ,1                              
            )                              AS p
WHERE   p.[value] != ''                        
    AND nb.[PrimaryName] IS NOT NULL;          

-- 17,114,040 Row(s) affected
-- 53 seconds

/*
        PrincipalId	ProfessionId	Ordinal
        -----------	------------	-------
        3870229 	4961        	1
        3870229 	4120        	2
        5156121 	4961        	1
        6305655 	6352        	1
*/
--and p.[value] IN ('director','writer')


--  SELECT  COUNT(*)
--  FROM    [IMDB].[dbo].[PrimaryProfessions]


-------------------------------------------------------------------------------
--  #7  Genres: dbo.[Genres]
-------------------------------------------------------------------------------
/*
INSERT  INTO 
        [IMDB].[dbo].[Genres]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [GenreId]
       ,[Genre]
    )
SELECT  DISTINCT 
        (ABS(CHECKSUM(p.[value]))%32000)                                            AS [GenreId]
       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], '_', ' '), 2, 100)) AS [Genre]
FROM    [IMDB].[Raw].[title.basics.tsv.gz] AS t
    CROSS APPLY STRING_SPLIT(t.[Genres], ',') AS p
WHERE   p.[value] != ''
GO
*/

INSERT  INTO                                                           
        [IMDB].[dbo].[Genres]                                          
        WITH (TABLOCKX, HOLDLOCK)                                      
    (                                                                  
        [GenreId]                                                      
       ,[Genre]                                                        
    )                                                                  
SELECT  DISTINCT                                                       
        (ABS(CHECKSUM(p.[value]))%32000)                                            AS [GenreId]   
       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], '_', ' '), 2, 100)) AS [Genre]     
FROM    [IMDB].[Raw].[title.basics.tsv.gz] AS t                        
    CROSS APPLY                                                        
        STRING_SPLIT                                                   
            (                                                          
                t.[Genres]                                             
               ,','                                                    
            )                              AS p                        
WHERE   p.[value] != '';                                               

-- 28 Row(s) affected
-- 5 seconds

/*
    SELECT  *
    FROM    [IMDB].[dbo].[Genres]
    ORDER   BY  [Genre]
*/

-------------------------------------------------------------------------------
--  #8  Title types:    dbo.[TitleTypes]
-------------------------------------------------------------------------------
/*
INSERT  INTO 
        [IMDB].[dbo].[TitleTypes]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleTypeId]
       ,[TitleType]
    )
SELECT  DISTINCT 
        ABS(CHECKSUM([TitleType]))%100 AS [TitleTypeId]
       ,[TitleType]
FROM    [IMDB].[Raw].[title.basics.tsv.gz]
GO
*/

INSERT  INTO                   
        [IMDB].[dbo].[TitleTypes]                          
        WITH (TABLOCKX, HOLDLOCK)                          
    (   
        [TitleTypeId]
       ,[TitleType]
    )                        
SELECT  DISTINCT               
        ABS(CHECKSUM([TitleType]))%100 AS [TitleTypeId]    
       ,[TitleType]            
FROM    [IMDB].[Raw].[title.basics.tsv.gz];

-- 11 Row(s) affected
-- 1 seconds

/*
    SELECT  *
    FROM    [IMDB].[dbo].[TitleTypes]
    ORDER   BY [TitleType]
*/

-------------------------------------------------------------------------------
--  #9  Titles: dbo.[Titles]
-------------------------------------------------------------------------------
/*
INSERT  INTO 
        [IMDB].[dbo].[Titles]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
       ,[StartYear]
       ,[EndYear]
       ,[Runtime]
    )
SELECT  CAST(SUBSTRING(tb.[TitleId], 3, 10) AS INT)                    AS [TitleId]
       ,ABS(CHECKSUM(tb.[TitleType]))%100                              AS [TitleTypeId]
       ,tb.[IsAdult]                                                   AS [IsAdult]
       ,DATEFROMPARTS(tb.[StartYear], 1, 1)                            AS [StartYear]
       ,DATEFROMPARTS(tb.[EndYear], 12, 31)                            AS [EndYear]
       ,DATEADD(MINUTE, tb.[RuntimeMinutes], CAST('00:00' AS TIME(0))) AS [Runtime]
FROM    [IMDB].[Raw].[title.basics.tsv.gz] tb
GO
*/
INSERT  INTO                                                   
        [IMDB].[dbo].[Titles]                                  
        WITH (TABLOCKX, HOLDLOCK)                              
    (                                                          
        [TitleId]                                              
       ,[TitleTypeId]                                          
       ,[IsAdult]                                              
       ,[StartYear]                                            
       ,[EndYear]                                              
       ,[Runtime]                                              
    )                                                          
SELECT  CAST(SUBSTRING(tb.[TitleId], 3, 10) AS INT)                    AS [TitleId]        
       ,ABS(CHECKSUM(tb.[TitleType]))%100                              AS [TitleTypeId]    
       ,tb.[IsAdult]                                                   AS [IsAdult]        
       ,DATEFROMPARTS(tb.[StartYear], 1, 1)                            AS [StartYear]      
       ,DATEFROMPARTS(tb.[EndYear], 12, 31)                            AS [EndYear]        
       ,DATEADD(MINUTE, tb.[RuntimeMinutes], CAST('00:00' AS TIME(0))) AS [Runtime]        
FROM    [IMDB].[Raw].[title.basics.tsv.gz] tb;

-- 12,541,389 Row(s) affected
-- 37 seconds

/*
    SELECT COUNT(*)    FROM [IMDB].[dbo].[Titles]
    SELECT TOP(1000) * FROM [IMDB].[dbo].[Titles]
*/

-------------------------------------------------------------------------------
--  #10 Data inconsistency
--          TitleTypes: dbo.[TitleTypes]
--          Titles    : dbo.[Titles]
-------------------------------------------------------------------------------
--  Data inconsistency:
--
--  Some titles only exist in the "aka"
--  table.
-------------------------------------------------------------------------------
/*
INSERT  INTO 
        [IMDB].[dbo].[TitleTypes]
    (
        [TitleTypeId]
       ,[TitleType]
    )
VALUES 
    (
        0
       ,'Unknown'
    )
GO
*/
INSERT  INTO                       
        [IMDB].[dbo].[TitleTypes]  
    (  
        [TitleTypeId]              
       ,[TitleType]                
    )  
        VALUES                     
    (  
        0                          
       ,'Unknown'                  
    );
-- 1 Row(s) affected
-- 0 seconds
/*
INSERT  INTO 
        [IMDB].[dbo].[Titles]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
    )
SELECT  TOP (1) WITH TIES
        CAST(SUBSTRING(ta.[TitleId], 3, 10) AS INT) AS [TitleId]
       ,0                                           AS [TitleTypeId]
       ,0                                           AS [IsAdult]
FROM    [IMDB].[Raw].[title.akas.tsv.gz] ta
WHERE   ta.[TitleId] NOT IN 
        (   
            SELECT  [TitleId] 
            FROM    [IMDB].[Raw].[title.basics.tsv.gz]
        )
ORDER   BY ROW_NUMBER() OVER (PARTITION BY ta.[TitleId] ORDER BY ta.[IsOriginalTitle] DESC, ta.[Ordering])
GO
*/

INSERT  INTO                               
        [IMDB].[dbo].[Titles]              
        WITH (TABLOCKX, HOLDLOCK)          
    (                                      
        [TitleId]                          
       ,[TitleTypeId]                      
       ,[IsAdult]                          
    )                                      
SELECT  TOP (1) WITH TIES                  
        CAST(SUBSTRING(ta.[TitleId], 3, 10) AS INT) AS [TitleId]       
       ,0                                           AS [TitleTypeId]   
       ,0                                           AS [IsAdult]       
FROM    [IMDB].[Raw].[title.akas.tsv.gz] ta
WHERE   ta.[TitleId] NOT IN                
        (                                  
            SELECT  [TitleId]              
            FROM    [IMDB].[Raw].[title.basics.tsv.gz]                 
        )                                  
ORDER   BY ROW_NUMBER()                    
        OVER                               
        (                                  
            PARTITION  BY  ta.[TitleId]    
            ORDER      BY  ta.[IsOriginalTitle] DESC                   
           ,ta.[Ordering]                  
        );

-- 35 Row(s) affected
-- 56 seconds

/*
    SELECT COUNT(*) FROM [IMDB].[dbo].[Titles]
*/

--  12,541,424

-------------------------------------------------------------------------------
--  #11 TitleGenres:    dbo.[TitleGenres]
-------------------------------------------------------------------------------
/*
INSERT  INTO 
        [IMDB].[dbo].[TitleGenres]
        WITH (TABLOCKX, HOLDLOCK) 
        (
            [TitleId]
           ,[GenreId]
        )
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]
       ,ABS(CHECKSUM(p.[value]))%32000           AS [GenreId]
FROM    [IMDB].[Raw].[title.basics.tsv.gz]      AS t
    CROSS APPLY STRING_SPLIT(t.[Genres], ',')   AS p
WHERE   p.[value] != ''
GO
*/

INSERT  INTO                       
        [IMDB].[dbo].[TitleGenres] 
        WITH (TABLOCKX, HOLDLOCK)  
    (                              
        [TitleId]                  
       ,[GenreId]                  
    )                              
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]  
       ,ABS(CHECKSUM(p.[value]))%32000           AS [GenreId]  
FROM    [IMDB].[Raw].[title.basics.tsv.gz]      AS t           
    CROSS APPLY                    
        STRING_SPLIT               
            (                      
                t.[Genres]         
               ,','                
            )                                   AS p           
WHERE   p.[value] != '';

-- 19,547,408 Row(s) affected
-- 48 seconds

-------------------------------------------------------------------------------
--  #12 TitleNames: dbo.[TitleNames]
-------------------------------------------------------------------------------
/*
INSERT  INTO                                                               
        [IMDB].[dbo].[TitleNames]                                          
        WITH (TABLOCKX, HOLDLOCK)                                          
    (                                                                      
        [TitleId]                                                          
       ,[Ordinal]                                                          
       ,[Region]                                                           
       ,[Language]                                                         
       ,[IsOriginal]                                                       
       ,[Title]                                                            
    )                                                                      
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT)  AS [TitleId]
       ,[Ordering]                                AS [Ordinal]
       ,[Region]                                  AS [Region]
       ,[Language]                                AS [Language]
       ,(
            CASE 
                WHEN [Ordering] =                                          
                     MIN(                                                  
                         (   CASE                                          
                                 WHEN ISNULL([IsOriginalTitle], 1) = 1     
                                 THEN [Ordering]                           
                             END                                           
                         )                                                 
                        )                                                  
                     OVER                                                  
                         (   PARTITION  BY  [TitleId] )                    
                THEN 1                                                     
                ELSE 0                                                     
            END                                                            
        )                                         AS [IsOriginal]          
       ,[Title]                                   AS [Title]
FROM    [IMDB].[Raw].[title.akas.tsv.gz]
GO
*/

INSERT  INTO                                   
        [IMDB].[dbo].[TitleNames]              
        WITH (TABLOCKX, HOLDLOCK)              
    (                                          
        [TitleId]                              
       ,[Ordinal]                              
       ,[Region]                               
       ,[Language]                             
       ,[IsOriginal]                           
       ,[Title]                                
    )                                          
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT)  AS [TitleId]             
       ,[Ordering]                                AS [Ordinal]             
       ,[Region]                                  AS [Region]              
       ,[Language]                                AS [Language]            
       ,(                                      
            CASE                               
                WHEN [Ordering] =              
                     MIN(                      
                         (   CASE              
                                 WHEN ISNULL([IsOriginalTitle], 1) = 1     
                                 THEN [Ordering]                           
                             END               
                         )                     
                        )                      
                     OVER                      
                         (   PARTITION  BY  [TitleId] )                    
                THEN 1                         
                ELSE 0                         
            END                                
        )                                         AS [IsOriginal]          
       ,[Title]                                   AS [Title]               
FROM    [IMDB].[Raw].[title.akas.tsv.gz];

-- 57,452,364 Row(s) affected
-- 407 seconds

/*
    SELECT COUNT(*) FROM [IMDB].[dbo].[TitleNames]
    SELECT TOP(1000) * FROM [IMDB].[dbo].[TitleNames]
*/

-------------------------------------------------------------------------------
--  #13 Attributes: title attribute
--      Attributes           :  dbo.[Attributes]
--      Title Name Attributes:  dbo.[TitleNameAttributes]
-------------------------------------------------------------------------------
/*
INSERT  INTO                                   
        [IMDB].[dbo].[Attributes]              
        WITH (TABLOCKX, HOLDLOCK)              
    (                                          
        [AttributeId]                          
       ,[Class]                                
       ,[Attribute]                            
    )                                          
SELECT  DISTINCT                               
        DENSE_RANK() OVER                      
    (                                          
        ORDER   BY                             
        (                                      
            SELECT a.[value]                   
        )                                      
    )                                                  AS [AttributeId],   
       'Title attribute'                               AS [Class],         
       a.[value]                                       AS [Attribute]      
FROM    [IMDB].[Raw].[title.akas.tsv.gz]               AS aka              
    CROSS APPLY                                                            
        STRING_SPLIT                                                       
            (                                                              
                aka.[Attributes]                                           
               ,CHAR(2)                                                    
            )                                          AS a                
WHERE   a.[value] != '';                       
*/

INSERT  INTO                                   
        [IMDB].[dbo].[Attributes]              
        WITH (TABLOCKX, HOLDLOCK)              
    (                                          
        [AttributeId]                          
       ,[Class]                                
       ,[Attribute]                            
    )                                          
SELECT  DISTINCT                               
        DENSE_RANK() OVER                      
    (                                          
        ORDER   BY                             
        (                                      
            SELECT a.[value]                   
        )                                      
    )                                                  AS [AttributeId],   
       'Title attribute'                               AS [Class],         
       a.[value]                                       AS [Attribute]      
FROM    [IMDB].[Raw].[title.akas.tsv.gz]               AS aka              
    CROSS APPLY                                
        STRING_SPLIT                           
            (                                  
                aka.[Attributes]               
               ,CHAR(2)                        
            )                                          AS a                
WHERE   a.[value] != '';

-- 163 Row(s) affected
-- 2 seconds

/*
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
*/

INSERT  INTO                                       
        [IMDB].[dbo].[TitleNameAttributes]         
        WITH (TABLOCKX, HOLDLOCK)                  
    (                                              
        [TitleId]                                  
       ,[Ordinal]                                  
       ,[AttributeId]                              
    )                                              
SELECT  DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT)  AS [TitleId],       
       aka.[Ordering]                                      AS [Ordinal],       
       attr.[AttributeId]                                  AS [AttributeId]    
FROM    [IMDB].[Raw].[title.akas.tsv.gz]           AS aka                      
    CROSS APPLY                                    
        STRING_SPLIT                               
            (                                      
                aka.[Attributes]                   
               ,CHAR(2)                            
            )                                      AS a                        
    INNER JOIN [IMDB].[dbo].[Attributes]           AS attr                     
        ON  attr.[Class]     = 'Title attribute'   
        AND attr.[Attribute] = a.[value];          

-- 311,606 Row(s) affected
-- 23 seconds

-------------------------------------------------------------------------------
--  #14 Attributes: Title types
--      Attributes           :  dbo.[Attributes]
--      Title Name Attributes:  dbo.[TitleNameAttributes]
-------------------------------------------------------------------------------
/*
INSERT  INTO
        [IMDB].[dbo].[Attributes]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [AttributeId]
       ,[Class]
       ,[Attribute]
    )
SELECT  DISTINCT 
        (SELECT MAX([AttributeId]) FROM [IMDB].[dbo].[Attributes])+DENSE_RANK() OVER (ORDER BY (SELECT a.[value]))  AS [AttributeId]
       ,'Title types'                                                                                               AS [Class]
       ,a.[value]                                                                                                   AS [Attribute]
FROM    [IMDB].[Raw].[title.akas.tsv.gz]            AS aka
    CROSS APPLY STRING_SPLIT(aka.[Types], CHAR(2))  AS a
WHERE   a.[value] NOT IN ('imdbDisplay', 'original')
GO
*/

INSERT  INTO [IMDB].[dbo].[Attributes] 
        WITH (TABLOCKX, HOLDLOCK)      
    (   
        [AttributeId]                  
       ,[Class]                        
       ,[Attribute]                    
    )                                  
SELECT  DISTINCT                       
    (                                  
        SELECT MAX([AttributeId])      
        FROM   [IMDB].[dbo].[Attributes]                           
    )   +                              
        DENSE_RANK()                   
        OVER                           
    (                                  
        ORDER BY                       
        (                              
            SELECT  a.[value]          
        )                              
    )                              AS [AttributeId]                
       ,'Title types'              AS [Class]                      
       ,a.[value]                  AS [Attribute]                  
FROM    [IMDB].[Raw].[title.akas.tsv.gz]            AS aka         
    CROSS APPLY                        
        STRING_SPLIT                   
            (                          
                aka.[Types]            
               ,CHAR(2)                
            )                                       AS a           
WHERE   a.[value] NOT IN               
    (                                  
        'imdbDisplay'                  
       ,'original'                     
    );                                 

-- 6 Row(s) affected
-- 4 seconds

/*
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
    CROSS APPLY STRING_SPLIT(aka.[Types], CHAR(2))  AS a
    INNER JOIN [IMDB].[dbo].[Attributes]            AS attr ON  attr.[Class]     = 'Title types' 
                                                            AND attr.[Attribute] = a.[value]
GO
*/
INSERT  INTO                               
        [IMDB].[dbo].[TitleNameAttributes] 
        WITH (TABLOCKX, HOLDLOCK)          
    (                                      
        [TitleId]                          
       ,[Ordinal]                          
       ,[AttributeId]                      
    )                                      
SELECT  DISTINCT                           
        CAST(SUBSTRING([TitleId], 3, 10) AS INT)   AS [TitleId]        
       ,aka.[Ordering]                             AS [Ordinal]        
       ,attr.[AttributeId]                         AS [AttributeId]    
FROM    [IMDB].[Raw].[title.akas.tsv.gz]           AS aka              
    CROSS APPLY                            
        STRING_SPLIT                       
        (                                  
            aka.[Types]                    
           ,CHAR(2)                        
        )                                          AS a                
    INNER JOIN [IMDB].[dbo].[Attributes]           AS attr             
        ON  attr.[Class]     = 'Title types'                           
        AND attr.[Attribute] = a.[value];  

-- 314,710 Row(s) affected
-- 23 seconds

-------------------------------------------------------------------------------
--  #15 Data inconsistency
--      Titles    : dbo.[Titles]
--      Principals: dbo.[Principals]
-------------------------------------------------------------------------------
--  Data inconsistency:
-- 
--  Some titles and principals only
--  exist in the "title.principals"
--  dataset.
-------------------------------------------------------------------------------
/*
INSERT  INTO
        [IMDB].[dbo].[Titles]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
    )
SELECT  DISTINCT 
        CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]
       ,0                                        AS [TitleTypeId]
       ,0                                        AS [IsAdult]
FROM    [IMDB].[Raw].[title.principals.tsv.gz]
WHERE   CAST(SUBSTRING([TitleId], 3, 10) AS INT) NOT IN 
            (
                SELECT  [TitleId] 
                FROM    [IMDB].[dbo].[Titles]
            )
GO
*/
INSERT  INTO 
        [IMDB].[dbo].[Titles] 
        WITH (TABLOCKX, HOLDLOCK)            
    (   
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
    )                           
SELECT  DISTINCT 
        CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]
       ,0                                        AS [TitleTypeId]
       ,0                                        AS [IsAdult]           
FROM    [IMDB].[Raw].[title.principals.tsv.gz]                         
WHERE   CAST(SUBSTRING([TitleId], 3, 10) AS INT) NOT IN                
    (                                      
        SELECT  [TitleId]                  
        FROM    [IMDB].[dbo].[Titles]      
    );

-- 0 Row(s) affected
-- 113 seconds

/*
INSERT  INTO
        [IMDB].[dbo].[Principals]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [PrincipalId]
       ,[PrimaryName]
    )
SELECT  DISTINCT 
        CAST(SUBSTRING([NameId], 3, 10) AS INT) AS [PrincipalId]
       ,N'Unknown'                              AS [PrimaryName]
FROM    [IMDB].[Raw].[title.principals.tsv.gz]
WHERE   CAST(SUBSTRING([NameId], 3, 10) AS INT) 
            NOT IN 
            (
                SELECT  [PrincipalId] 
                FROM    [IMDB].[dbo].[Principals]
            )
GO
*/

INSERT  INTO                                   
        [IMDB].[dbo].[Principals]              
        WITH (TABLOCKX, HOLDLOCK)              
    (                                          
        [PrincipalId]                          
       ,[PrimaryName]                          
    )                                          
SELECT  DISTINCT                               
        CAST(SUBSTRING([NameId], 3, 10) AS INT) AS [PrincipalId]           
       ,N'Unknown'                              AS [PrimaryName]           
FROM    [IMDB].[Raw].[title.principals.tsv.gz] 
WHERE   CAST(SUBSTRING([NameId], 3, 10) AS INT)
            NOT IN                             
            (                                  
                SELECT  [PrincipalId]          
                FROM    [IMDB].[dbo].[Principals]                          
            );                                 

-- 1,666 Row(s) affected
-- 89 seconds

-------------------------------------------------------------------------------
--  #16 Title principals:   dbo.[TitlePrincipals]
-------------------------------------------------------------------------------
/*
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
*/

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
FROM    [IMDB].[Raw].[title.principals.tsv.gz] tp;

-- 99,764,730 Row(s) affected
-- 418 seconds

/*
    SELECT COUNT(*) FROM [IMDB].[dbo].[TitlePrincipals]
*/

-------------------------------------------------------------------------------
--  #17 Principals "known for" titles
--      Title Principals:   dbo.[TitlePrincipals]
-------------------------------------------------------------------------------
/*
UPDATE  tp
    SET tp.[KnownForOrdinal] = k.[Ordinal]
FROM    [IMDB].[Raw].[name.basics.tsv.gz]                   AS n
    CROSS APPLY STRING_SPLIT(n.[KnownForTitles], ',', 1)    AS k
    INNER JOIN [IMDB].[dbo].[TitlePrincipals]               AS tp WITH (TABLOCKX, HOLDLOCK) ON
                CAST(SUBSTRING(n.[NameId], 3, 10) AS INT) = tp.[PrincipalId] 
                AND
                CAST(SUBSTRING(k.[value], 3, 10) AS INT)  = tp.[TitleId]
WHERE   k.[value] != ''
GO
*/

UPDATE  tp                                         
    SET tp.[KnownForOrdinal] = k.[Ordinal]         
FROM    [IMDB].[Raw].[name.basics.tsv.gz]                   AS n               
    CROSS APPLY                                    
        STRING_SPLIT(   n.[KnownForTitles]         
                       ,','                        
                       ,1                   )               AS k               
    INNER JOIN [IMDB].[dbo].[TitlePrincipals]               AS tp              
        WITH    (TABLOCKX, HOLDLOCK) 
          ON    CAST(SUBSTRING(n.[NameId], 3, 10) AS INT) = tp.[PrincipalId]   
                AND 
                CAST(SUBSTRING(k.[value], 3, 10) AS INT)  = tp.[TitleId]       
WHERE   k.[value] != '';                           

-- 10,642,136 Row(s) affected
-- 106 seconds

-------------------------------------------------------------------------------
--  #18 Title characters:   dbo.[TitleCharacters]
-------------------------------------------------------------------------------
/*
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
    CROSS APPLY STRING_SPLIT(REPLACE(REPLACE(SUBSTRING(tp.[Characters], 3, LEN(tp.[Characters])-4), N'","', NCHAR(9)), N'\"', N'"'), NCHAR(9)) AS ch
GO
*/

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
FROM    [IMDB].[Raw].[title.principals.tsv.gz]      AS tp                  
    CROSS APPLY                                
    STRING_SPLIT                               
        (   REPLACE                            
            (   REPLACE                        
                (                              
                    SUBSTRING(tp.[Characters], 3, LEN(tp.[Characters])-4)  
                   ,N'","'                   
                   ,NCHAR(9)                   
                )                              
               ,N'\"'                         
               ,N'"'                          
            )                                  
           ,NCHAR(9)                           
        ) AS ch;                               

-- 48,687,272 Row(s) affected
-- 587 seconds

-------------------------------------------------------------------------------
--  #19 Directors and Writers
-------------------------------------------------------------------------------
--  Directors and writers
--  (there's a slight overlap with the
--  title principals here)
-------------------------------------------------------------------------------
/*
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
*/

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
        FROM    STRING_SPLIT(tc.[Directors], ',')        AS p                       
        WHERE   tc.[Directors] != ''                    
        UNION                                           
        SELECT  CAST(SUBSTRING(w.[value], 3, 10) AS INT) AS [PrincipalId]           
               ,ABS(CHECKSUM('writer'))%10000            AS [ProfessionId]          
        FROM    STRING_SPLIT(tc.[Writers], ',')          AS w                       
        WHERE   tc.[Writers] != ''                      
    ) AS x                                              
    LEFT JOIN [IMDB].[dbo].[TitlePrincipals]            AS tp                       
                ON  tp.[TitleId]     = CAST(SUBSTRING(tc.[TitleId], 3, 10) AS INT)  
                AND tp.[PrincipalId] = x.[PrincipalId]  
WHERE   tp.[TitleId] IS NULL;                           

-- 3,401,367 Row(s) affected
-- 38 seconds

-------------------------------------------------------------------------------
--  #20 Data inconsistency
--      Titles    : dbo.[Titles]
--      Principals: dbo.[Principals]
-------------------------------------------------------------------------------
--  Data inconsistency:
-- 
--  Some of these titles and principals
--  are not in their proper respective
--  datasets.
-------------------------------------------------------------------------------
/*
INSERT  INTO
        [IMDB].[dbo].[Titles]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[TitleTypeId]
       ,[IsAdult]
    )
SELECT  DISTINCT 
        [TitleId] AS [TitleId]
       ,0         AS [TitleTypeId]
       ,0         AS [IsAdult]
FROM    #writers_directors
WHERE   [TitleId] NOT IN (SELECT [TitleId] FROM [IMDB].[dbo].[Titles])
GO
*/
INSERT  INTO           
        [IMDB].[dbo].[Titles]                      
        WITH (TABLOCKX, HOLDLOCK)                  
    (   
        [TitleId]      
       ,[TitleTypeId]  
       ,[IsAdult]      
    )                  
SELECT  DISTINCT       
        [TitleId]   AS [TitleId]                   
       ,0           AS [TitleTypeId]               
       ,0           AS [IsAdult]                   
FROM    #writers_directors                         
WHERE   [TitleId] NOT IN                           
            (          
                SELECT  [TitleId]                  
                FROM    [IMDB].[dbo].[Titles]      
            );         

-- 0 Row(s) affected
-- 3 seconds
/*
INSERT  INTO
        [IMDB].[dbo].[Principals]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [PrincipalId]
       ,[PrimaryName]
    )
SELECT  DISTINCT 
        [PrincipalId] AS [PrincipalId]
       ,N'Unknown'    AS [PrimaryName]
FROM    #writers_directors
WHERE   [PrincipalId] NOT IN (SELECT [PrincipalId] FROM [IMDB].[dbo].[Principals])
GO
*/

INSERT  INTO           
        [IMDB].[dbo].[Principals]                  
        WITH (TABLOCKX, HOLDLOCK)                  
    (                  
        [PrincipalId]  
       ,[PrimaryName]  
    )                  
SELECT  DISTINCT       
        [PrincipalId]  AS [PrincipalId]            
       ,N'Unknown'     AS [PrimaryName]            
FROM    #writers_directors                         
WHERE   [PrincipalId] NOT IN                       
            (          
                SELECT  [PrincipalId]              
                FROM    [IMDB].[dbo].[Principals]  
            );         

-- 19 Row(s) affected
-- 3 seconds

-------------------------------------------------------------------------------
--  #21 Now (finally) the actual title principals
--      Title Principals:   dbo.[TitlePrincipals]
-------------------------------------------------------------------------------
/*
INSERT  INTO
        [IMDB].[dbo].[TitlePrincipals]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[Ordinal]
       ,[PrincipalId]
       ,[ProfessionId]
    )
SELECT  x.[TitleId]                                                                                                    AS [TitleId]
       ,ISNULL(o.[Ordinal], 0)+ROW_NUMBER() OVER (PARTITION BY x.[TitleId] ORDER BY x.[ProfessionId], x.[PrincipalId]) AS [Ordinal]
       ,x.[PrincipalId]                                                                                                AS [PrincipalId]
       ,x.[ProfessionId]                                                                                               AS [ProfessionId]
FROM    #writers_directors AS x
    LEFT JOIN 
        (
            SELECT  [TitleId]
                   ,MAX([Ordinal]) AS [Ordinal]
            FROM    [IMDB].[dbo].[TitlePrincipals]
            GROUP   BY [TitleId]
        ) AS o ON x.[TitleId] = o.[TitleId];
DROP TABLE #writers_directors
*/

INSERT  INTO                                
        [IMDB].[dbo].[TitlePrincipals]      
        WITH (TABLOCKX, HOLDLOCK)           
    (                                       
        [TitleId]                           
       ,[Ordinal]                           
       ,[PrincipalId]                       
       ,[ProfessionId]                      
    )                                       
SELECT  x.[TitleId]                                  AS [TitleId]       
       ,ISNULL(o.[Ordinal], 0) + ROW_NUMBER()                           
            OVER (                          
                    PARTITION BY    x.[TitleId]                         
                    ORDER     BY    x.[ProfessionId]                    
                                   ,x.[PrincipalId]                     
                 )                                   AS [Ordinal]       
       ,x.[PrincipalId]                              AS [PrincipalId]   
       ,x.[ProfessionId]                             AS [ProfessionId]  
FROM    #writers_directors          AS x             
    LEFT JOIN                               
        (                                   
            SELECT  [TitleId]       AS [TitleId]                         
                   ,MAX([Ordinal])  AS [Ordinal]                         
            FROM    [IMDB].[dbo].[TitlePrincipals]                      
            GROUP   BY [TitleId]            
        )                           AS o                            
            ON x.[TitleId] = o.[TitleId]    
DROP TABLE #writers_directors;                                          

-- 3,401,367 Row(s) affected
-- 43 seconds

-------------------------------------------------------------------------------
--  #22 Epidodes:   dbo.[Episodes]
-------------------------------------------------------------------------------
/*
INSERT  INTO
        [IMDB].[dbo].[Episodes]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [ParentId]
       ,[EpisodeId]
       ,[Season]
       ,[Episode]
    )
SELECT  DISTINCT
        CAST(SUBSTRING(te.[ParentTitleId], 3, 10) AS INT) AS [ParentId]
       ,CAST(SUBSTRING(te.[TitleId], 3, 10) AS INT)       AS [EpisodeId]
       ,te.[SeasonNumber]                                 AS [Season]
       ,te.[EpisodeNumber]                                AS [Episode]
FROM    [IMDB].[Raw].[title.episode.tsv.gz] te
GO
*/
INSERT  INTO                                   
        [IMDB].[dbo].[Episodes]                
        WITH (TABLOCKX, HOLDLOCK)              
    (   
        [ParentId]                             
       ,[EpisodeId]                            
       ,[Season]                               
       ,[Episode]                              
    )                                          
SELECT  DISTINCT                               
        CAST(SUBSTRING(te.[ParentTitleId], 3, 10) AS INT) AS [ParentId]    
       ,CAST(SUBSTRING(te.[TitleId], 3, 10) AS INT)       AS [EpisodeId]   
       ,te.[SeasonNumber]                                 AS [Season]      
       ,te.[EpisodeNumber]                                AS [Episode]     
FROM    [IMDB].[Raw].[title.episode.tsv.gz] te;                            

-- 9,687,178 Row(s) affected
-- 58 seconds

-------------------------------------------------------------------------------
--  #23 Votes and average ratings on titles
--      Titles: dbo.[Titles]
-------------------------------------------------------------------------------
--  Votes and average ratings on
--  titles.
-------------------------------------------------------------------------------
/*
UPDATE  t
    SET t.[VoteCount]     = r.[NumVotes]
       ,t.[AverageRating] = r.[AverageRating]
FROM    [IMDB].[dbo].[Titles] AS t 
        WITH (TABLOCKX, HOLDLOCK)
    INNER JOIN [IMDB].[Raw].[title.ratings.tsv.gz] AS r ON t.[TitleId] = CAST(SUBSTRING(r.[TitleId], 3, 10) AS INT)
GO
*/

UPDATE  t                                      
    SET t.[VoteCount]     = r.[NumVotes]       
       ,t.[AverageRating] = r.[AverageRating]  
FROM    [IMDB].[dbo].[Titles] AS t             
        WITH (TABLOCKX, HOLDLOCK)              
    INNER JOIN [IMDB].[Raw].[title.ratings.tsv.gz] AS r                 
            ON t.[TitleId] = CAST(SUBSTRING(r.[TitleId], 3, 10) AS INT);

-- 1,676,404 Row(s) affected
-- 10 seconds
