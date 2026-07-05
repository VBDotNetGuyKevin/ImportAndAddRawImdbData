---------------------------------------------------------------------------
-- ADHOC_401
---------------------------------------------------------------------------
INSERT  INTO [IMDB].[dbo].[Principals]                               
        WITH (TABLOCKX, HOLDLOCK)                                    
    ( [PrincipalId], [PrimaryName], [BirthYear], [DeathYear] )       
SELECT  CAST(SUBSTRING(nb.[NameId], 3, 100) AS INT) AS [PrincipalId] 
       ,nb.[PrimaryName]                            AS [PrimaryName] 
       ,DATEFROMPARTS(nb.[BirthYear], 1, 1)         AS [BirthYear]   
       ,DATEFROMPARTS(nb.[DeathYear], 12, 31)       AS [DeathYear]   
FROM    [IMDB].[Raw].[name.basics.tsv.gz] nb                         
WHERE   nb.[PrimaryName] IS NOT NULL;
-- 15451179

---------------------------------------------------------------------------
-- ADHOC_402
---------------------------------------------------------------------------
INSERT  INTO [IMDB].[dbo].[Professions]  WITH (TABLOCKX, HOLDLOCK)                                            
      ( [ProfessionId], [Profession] )                                                                        
SELECT  DISTINCT                                                                                              
        (ABS(CHECKSUM(p.[value]))%10000)                                              AS [ProfessionId]       
       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], N'_', N' '), 2, 100)) AS [Profession]         
FROM    [IMDB].[Raw].[name.basics.tsv.gz]  AS n                                                               
    CROSS APPLY STRING_SPLIT(n.[PrimaryProfession],N',')                              AS p                    
WHERE   p.[value] != ''                                                                                       
UNION                                                                                                         
SELECT  DISTINCT                                                                                              
        ABS(CHECKSUM(tp.[Category]))%10000                                                  AS [ProfessionId] 
       ,UPPER(LEFT(tp.[Category], 1))+SUBSTRING(REPLACE(tp.[Category], N'_', N' '), 2, 100) AS [Profession]   
FROM    [IMDB].[Raw].[title.principals.tsv.gz] tp                                                             
WHERE   tp.[Category] != N'';
-- 47

---------------------------------------------------------------------------
-- ADHOC_403
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Professions] WITH (TABLOCKX, HOLDLOCK) 
  ( [ProfessionId], [Profession] )                               
SELECT  ABS(CHECKSUM('director'))%10000 AS [ProfessionId]        
       ,'Director'                      AS [Profession]          
UNION                                                            
SELECT  ABS(CHECKSUM('writer'))%10000   AS [ProfessionId]        
       ,'Writer'                        AS [Profession];
-- 2

---------------------------------------------------------------------------
-- ADHOC_404
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[PrimaryProfessions] WITH (TABLOCKX, HOLDLOCK) 
  ( [PrincipalId], [ProfessionId], [Ordinal] )                          
SELECT  (CAST(SUBSTRING(nb.[NameId], 3, 100) AS INT)) AS [PrincipalId]  
       ,(ABS(CHECKSUM(p.[value]))%10000)              AS [ProfessionId] 
       ,p.[Ordinal]                                   AS [Ordinal]      
FROM    [IMDB].[Raw].[name.basics.tsv.gz]                 AS nb         
    CROSS APPLY STRING_SPLIT(nb.primaryProfession,N',',1) AS p          
WHERE   p.[value] != ''                                                 
    AND nb.[PrimaryName] IS NOT NULL;
-- 17,185,592

---------------------------------------------------------------------------
-- ADHOC_405
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Genres] WITH (TABLOCKX, HOLDLOCK)                                      
  ( [GenreId], [Genre] )                                                                         
SELECT  DISTINCT                                                                                 
        (ABS(CHECKSUM(p.[value]))%32000)                                            AS [GenreId] 
       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], '_', ' '), 2, 100)) AS [Genre]   
FROM    [IMDB].[Raw].[title.basics.tsv.gz]   AS t                                                
    CROSS APPLY STRING_SPLIT(t.[Genres],',') AS p                                                
WHERE   p.[value] != '';
-- 28

---------------------------------------------------------------------------
-- ADHOC_406
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitleTypes] WITH (TABLOCKX, HOLDLOCK) 
  ( [TitleTypeId], [TitleType] )                                
SELECT  DISTINCT                                                
        ABS(CHECKSUM([TitleType]))%100 AS [TitleTypeId]         
       ,[TitleType]                                             
FROM    [IMDB].[Raw].[title.basics.tsv.gz];
-- 11

---------------------------------------------------------------------------
-- ADHOC_407
---------------------------------------------------------------------------
INSERT  INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK)                            
    (   [TitleId], [TitleTypeId], [IsAdult], [StartYear], [EndYear], [Runtime] )        
SELECT  CAST(SUBSTRING(tb.[TitleId], 3, 10) AS INT)                    AS [TitleId]     
       ,ABS(CHECKSUM(tb.[TitleType]))%100                              AS [TitleTypeId] 
       ,tb.[IsAdult]                                                   AS [IsAdult]     
       ,DATEFROMPARTS(tb.[StartYear], 1, 1)                            AS [StartYear]   
       ,DATEFROMPARTS(tb.[EndYear], 12, 31)                            AS [EndYear]     
       ,DATEADD(MINUTE, tb.[RuntimeMinutes], CAST('00:00' AS TIME(0))) AS [Runtime]     
FROM    [IMDB].[Raw].[title.basics.tsv.gz] tb;
-- 12,609,928

---------------------------------------------------------------------------
-- ADHOC_408
---------------------------------------------------------------------------
INSERT  INTO [IMDB].[dbo].[TitleTypes] 
    ( [TitleTypeId],[TitleType] )      
  VALUES (0,'Unknown');
-- 1

---------------------------------------------------------------------------
-- ADHOC_409
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK)          
  ( [TitleId], [TitleTypeId], [IsAdult] )                            
SELECT  TOP (1) WITH TIES                                            
        CAST(SUBSTRING(ta.[TitleId], 3, 10) AS INT) AS [TitleId]     
       ,0                                           AS [TitleTypeId] 
       ,0                                           AS [IsAdult]     
FROM    [IMDB].[Raw].[title.akas.tsv.gz] ta                          
WHERE   ta.[TitleId] NOT IN                                          
        ( SELECT [TitleId] FROM [IMDB].[Raw].[title.basics.tsv.gz] ) 
ORDER   BY ROW_NUMBER()                                              
        OVER                                                         
  ( PARTITION BY ta.[TitleId] ORDER BY ta.[IsOriginalTitle] DESC, ta.[Ordering] );
-- 1,862

---------------------------------------------------------------------------
-- ADHOC_410
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitleGenres] WITH (TABLOCKX, HOLDLOCK) 
  ( [TitleId], [GenreId] )                                       
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]    
       ,ABS(CHECKSUM(p.[value]))%32000           AS [GenreId]    
FROM    [IMDB].[Raw].[title.basics.tsv.gz]      AS t             
    CROSS APPLY STRING_SPLIT(t.[Genres], ',')   AS p             
WHERE   p.[value] != '';
-- 19,644,165

---------------------------------------------------------------------------
-- ADHOC_411
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitleNames] WITH (TABLOCKX, HOLDLOCK)         
  ( [TitleId], [Ordinal], [Region], [Language], [IsOriginal], [Title] ) 
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT)  AS [TitleId]          
       ,[Ordering]                                AS [Ordinal]          
       ,[Region]                                  AS [Region]           
       ,[Language]                                AS [Language]         
       ,(CASE WHEN [Ordering] =                                         
                   MIN((CASE WHEN ISNULL([IsOriginalTitle], 1) = 1      
                             THEN [Ordering] END))                      
                     OVER ( PARTITION BY [TitleId] )                    
              THEN 1 ELSE 0 END)                  AS [IsOriginal]       
       ,[Title]                                   AS [Title]            
FROM    [IMDB].[Raw].[title.akas.tsv.gz];
-- APPROXIMATELY: 57,452,364 Rows
-- 58,185,048

---------------------------------------------------------------------------
-- ADHOC_412
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Attributes] WITH (TABLOCKX, HOLDLOCK)                     
  ( [AttributeId], [Class], [Attribute] )                                           
SELECT DISTINCT DENSE_RANK() OVER ( ORDER BY (SELECT a.[value]) ) AS [AttributeId], 
       'Title attribute' AS [Class],                                                
       a.[value]         AS [Attribute]                                             
FROM [IMDB].[Raw].[title.akas.tsv.gz]                   AS aka                      
    CROSS APPLY STRING_SPLIT(aka.[Attributes], CHAR(2)) AS a                        
WHERE   a.[value] != '';
-- APPROXIMATELY: 163
-- 163

---------------------------------------------------------------------------
-- ADHOC_413
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitleNameAttributes] WITH (TABLOCKX, HOLDLOCK)  
  ( [TitleId], [Ordinal], [AttributeId] )                                 
SELECT DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId],    
       aka.[Ordering]                                    AS [Ordinal],    
       attr.[AttributeId]                                AS [AttributeId] 
FROM [IMDB].[Raw].[title.akas.tsv.gz]                   AS aka            
    CROSS APPLY STRING_SPLIT(aka.[Attributes], CHAR(2)) AS a              
    INNER JOIN [IMDB].[dbo].[Attributes]                AS attr           
        ON  attr.[Class]     = 'Title attribute'                          
        AND attr.[Attribute] = a.[value];
-- APPROXIMATELY: 311606
-- 312,258

---------------------------------------------------------------------------
-- ADHOC_414
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Attributes] WITH (TABLOCKX, HOLDLOCK)      
  ( [AttributeId], [Class], [Attribute] )                            
SELECT DISTINCT                                                      
  ( SELECT MAX([AttributeId]) FROM [IMDB].[dbo].[Attributes] ) +     
    DENSE_RANK() OVER (ORDER BY(SELECT a.[value]) ) AS [AttributeId] 
   ,'Title types'                                   AS [Class]       
   ,a.[value]                                       AS [Attribute]   
FROM [IMDB].[Raw].[title.akas.tsv.gz]              AS aka            
    CROSS APPLY STRING_SPLIT(aka.[Types], CHAR(2)) AS a              
WHERE a.[value] NOT IN ('imdbDisplay', 'original');
-- APPROXIMATELY: 6
-- 6

---------------------------------------------------------------------------
-- ADHOC_415
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitleNameAttributes] WITH (TABLOCKX, HOLDLOCK)  
  ( [TitleId], [Ordinal], [AttributeId] )                                 
SELECT DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]     
               ,aka.[Ordering]                           AS [Ordinal]     
               ,attr.[AttributeId]                       AS [AttributeId] 
FROM [IMDB].[Raw].[title.akas.tsv.gz] AS aka                              
    CROSS APPLY STRING_SPLIT(aka.[Types], CHAR(2)) AS a                   
    INNER JOIN [IMDB].[dbo].[Attributes]           AS attr                
        ON  attr.[Class]     = 'Title types'                              
        AND attr.[Attribute] = a.[value];
-- APPROXIMATELY: 314,710
-- 318,234

---------------------------------------------------------------------------
-- ADHOC_416
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK)               
  ( [TitleId], [TitleTypeId], [IsAdult] )                                 
SELECT DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]     
               ,0                                        AS [TitleTypeId] 
               ,0                                        AS [IsAdult]     
FROM [IMDB].[Raw].[title.principals.tsv.gz]                               
WHERE CAST(SUBSTRING([TitleId], 3, 10) AS INT) NOT IN                     
      (SELECT [TitleId] FROM [IMDB].[dbo].[Titles]);
-- APPROXIMATELY: 0
-- 0

---------------------------------------------------------------------------
-- ADHOC_417
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Principals] WITH (TABLOCKX, HOLDLOCK)          
  ( [PrincipalId], [PrimaryName] )                                       
SELECT DISTINCT CAST(SUBSTRING([NameId], 3, 10) AS INT) AS [PrincipalId] 
               ,N'Unknown'                              AS [PrimaryName] 
FROM [IMDB].[Raw].[title.principals.tsv.gz]                              
WHERE CAST(SUBSTRING([NameId], 3, 10) AS INT) NOT IN                     
      (SELECT [PrincipalId] FROM [IMDB].[dbo].[Principals]);
-- APPROXIMATELY: 1,666
-- 3,244

---------------------------------------------------------------------------
-- ADHOC_418
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitlePrincipals] WITH (TABLOCKX, HOLDLOCK) 
  ( [TitleId], [Ordinal], [PrincipalId], [ProfessionId] )            
SELECT CAST(SUBSTRING(tp.[TitleId], 3, 10) AS INT) AS [TitleId]      
      ,tp.[Ordering]                               AS [Ordinal]      
      ,CAST(SUBSTRING(tp.[NameId], 3, 10) AS INT)  AS [PrincipalId]  
      ,ABS(CHECKSUM(tp.[Category]))%10000          AS [ProfessionId] 
FROM [IMDB].[Raw].[title.principals.tsv.gz] tp;
-- APPROXIMATELY: 99,764,730
-- 100,255,269

---------------------------------------------------------------------------
-- ADHOC_419
---------------------------------------------------------------------------
UPDATE tp                                                                    
    SET tp.[KnownForOrdinal] = k.[Ordinal]                                   
FROM [IMDB].[Raw].[name.basics.tsv.gz]                   AS n                
    CROSS APPLY STRING_SPLIT(n.[KnownForTitles], ',', 1) AS k                
    INNER JOIN [IMDB].[dbo].[TitlePrincipals]            AS tp               
        WITH    (TABLOCKX, HOLDLOCK)                                         
          ON    CAST(SUBSTRING(n.[NameId], 3, 10) AS INT) = tp.[PrincipalId] 
                AND                                                          
                CAST(SUBSTRING(k.[value], 3, 10) AS INT)  = tp.[TitleId]     
WHERE k.[value] != '';
-- APPROXIMATELY: 10,642,136
-- 10,679,061

---------------------------------------------------------------------------
-- ADHOC_420
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitleCharacters] WITH (TABLOCKX, HOLDLOCK)  
  ( [TitleId], [PrincipalId], [Character] )                           
SELECT CAST(SUBSTRING(tp.[TitleId], 3, 10) AS INT) AS [TitleId]       
      ,CAST(SUBSTRING(tp.[NameId], 3, 10) AS INT)  AS [PrincipalId]   
      ,ch.[value]                                  AS [Character]     
FROM [IMDB].[Raw].[title.principals.tsv.gz] AS tp                     
    CROSS APPLY STRING_SPLIT(REPLACE(REPLACE(                         
                SUBSTRING(tp.[Characters], 3, LEN(tp.[Characters])-4) 
                , N'", "',NCHAR(9)), N'\"', N'"'), NCHAR(9)) AS ch;
-- APPROXIMATELY: 48,942,272
-- 48,942,647

---------------------------------------------------------------------------
-- ADHOC_421
---------------------------------------------------------------------------
SELECT t.[TitleId], x.[PrincipalId], x.[ProfessionId]                                  
INTO #writers_directors                                                                
FROM [IMDB].[Raw].[title.crew.tsv.gz] AS tc                                            
    CROSS APPLY (VALUES (CAST(SUBSTRING(tc.[TitleId], 3, 10) AS INT))) AS t([TitleId]) 
    CROSS APPLY (SELECT CAST(SUBSTRING(p.[value], 3, 10) AS INT) AS [PrincipalId]      
                       ,ABS(CHECKSUM('director'))%10000          AS [ProfessionId]     
                 FROM STRING_SPLIT(tc.[Directors], ',')        AS p                    
                 WHERE tc.[Directors] != ''                                            
                 UNION                                                                 
                 SELECT CAST(SUBSTRING(w.[value], 3, 10) AS INT) AS [PrincipalId]      
                       ,ABS(CHECKSUM('writer'))%10000            AS [ProfessionId]     
                 FROM STRING_SPLIT(tc.[Writers], ',')          AS w                    
                 WHERE tc.[Writers] != '')              AS x                           
    LEFT JOIN [IMDB].[dbo].[TitlePrincipals]            AS tp                          
                ON  tp.[TitleId]     = CAST(SUBSTRING(tc.[TitleId], 3, 10) AS INT)     
                AND tp.[PrincipalId] = x.[PrincipalId]                                 
WHERE tp.[TitleId] IS NULL;
-- APPROXIMATELY: 3,401,367
-- 3,413,389

---------------------------------------------------------------------------
-- ADHOC_422
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK) 
  ( [TitleId], [TitleTypeId], [IsAdult] )                   
SELECT DISTINCT [TitleId]   AS [TitleId]                    
               ,0           AS [TitleTypeId]                
               ,0           AS [IsAdult]                    
FROM #writers_directors                                     
WHERE [TitleId] NOT IN                                      
      (SELECT [TitleId] FROM [IMDB].[dbo].[Titles] );
-- APPROXIMATELY: 0
-- 0

---------------------------------------------------------------------------
-- ADHOC_423
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Principals] WITH (TABLOCKX, HOLDLOCK) 
  ( [PrincipalId], [PrimaryName] )                              
SELECT DISTINCT [PrincipalId]  AS [PrincipalId]                 
               ,N'Unknown'     AS [PrimaryName]                 
FROM #writers_directors                                         
WHERE [PrincipalId] NOT IN                                      
      (SELECT [PrincipalId] FROM [IMDB].[dbo].[Principals]);
-- APPROXIMATELY: 19
-- 56

---------------------------------------------------------------------------
-- ADHOC_424
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[TitlePrincipals] WITH (TABLOCKX, HOLDLOCK) 
  ( [TitleId], [Ordinal], [PrincipalId], [ProfessionId] )            
SELECT x.[TitleId]                                AS [TitleId]       
      ,ISNULL(o.[Ordinal], 0) + ROW_NUMBER()                         
         OVER (PARTITION BY x.[TitleId]                              
               ORDER     BY x.[ProfessionId], x.[PrincipalId]        
              )                                   AS [Ordinal]       
      ,x.[PrincipalId]                            AS [PrincipalId]   
      ,x.[ProfessionId]                           AS [ProfessionId]  
FROM #writers_directors AS x                                         
    LEFT JOIN (SELECT [TitleId]       AS [TitleId]                   
                     ,MAX([Ordinal])  AS [Ordinal]                   
               FROM   [IMDB].[dbo].[TitlePrincipals]                 
               GROUP  BY [TitleId] )  AS o                           
              ON x.[TitleId] = o.[TitleId]                           
DROP TABLE #writers_directors;
-- APPROXIMATELY: 3,401,367
-- 3,413,389

---------------------------------------------------------------------------
-- ADHOC_425
---------------------------------------------------------------------------
INSERT INTO [IMDB].[dbo].[Episodes] WITH (TABLOCKX, HOLDLOCK)           
  ( [ParentId], [EpisodeId], [Season], [Episode] )                      
SELECT DISTINCT                                                         
       CAST(SUBSTRING(te.[ParentTitleId], 3, 10) AS INT) AS [ParentId]  
      ,CAST(SUBSTRING(te.[TitleId], 3, 10) AS INT)       AS [EpisodeId] 
      ,te.[SeasonNumber]                                 AS [Season]    
      ,te.[EpisodeNumber]                                AS [Episode]   
FROM [IMDB].[Raw].[title.episode.tsv.gz] te
    INNER JOIN [IMDB].[dbo].[Titles] t1 ON t1.[TitleId] = CAST(SUBSTRING(te.[ParentTitleId], 3, 10) AS INT)
    INNER JOIN [IMDB].[dbo].[Titles] t2 ON t2.[TitleId] = CAST(SUBSTRING(te.[TitleId], 3, 10) AS INT);

-- APPROXIMATELY: 9,687,178     9687178
-- 9,743,260

---------------------------------------------------------------------------
-- ADHOC_426
---------------------------------------------------------------------------
UPDATE t                                                  
  SET t.[VoteCount]     = r.[NumVotes]                    
     ,t.[AverageRating] = r.[AverageRating]               
FROM [IMDB].[dbo].[Titles] AS t WITH (TABLOCKX, HOLDLOCK) 
    INNER JOIN [IMDB].[Raw].[title.ratings.tsv.gz] AS r   
            ON t.[TitleId] = CAST(SUBSTRING(r.[TitleId], 3, 10) AS INT);
-- APPROXIMATELY: 1,676,404     1676404
-- 1,689,394
