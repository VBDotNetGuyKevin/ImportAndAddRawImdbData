USE [IMDB];

/*
    I)  Drop all constraints
*/

ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [PK_Episodes]
    WITH (ONLINE = OFF)
GO

ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [FK_TitleCharacters_Episode]
GO

ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [FK_TitleCharacters_Parent]
GO

------------------------------------------------------------------------------------------------------

DROP INDEX              [IX_TitleCharacters]
    ON                  [IMDB].[dbo].[TitleCharacters]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    DROP CONSTRAINT     [FK_TitleCharacters_Principal]
GO

ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    DROP CONSTRAINT     [FK_TitleCharacters_Title]
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Principal]
GO

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Profession]
GO

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Title]
GO

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [PK_TitlePrincipals]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [FK_TitleNameAttributes_Attribute]
GO

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [FK_TitleNameAttributes_TitleName]
GO

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [PK_TitleNameAttributes]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[Attributes] 
    DROP CONSTRAINT     [UQ_Attributes]
GO

ALTER TABLE             [IMDB].[dbo].[Attributes] 
    DROP CONSTRAINT     [PK_Attributes]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

DROP INDEX              [IX_TitleNames_Original]
    ON                  [IMDB].[dbo].[TitleNames]
GO

ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    DROP CONSTRAINT     [FK_TitleNames_Title]
GO

ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    DROP CONSTRAINT     [PK_TitleNames]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [FK_TitleGenres_Genre]
GO

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [FK_TitleGenres_Title]
GO

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [PK_TitleGenres]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[Titles] 
    DROP CONSTRAINT     [FK_Titles_TitleType]
GO

ALTER TABLE             [IMDB].[dbo].[Titles] 
    DROP CONSTRAINT     [PK_Titles]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[TitleTypes] 
    DROP CONSTRAINT     [PK_TitleTypes]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[Genres] 
    DROP CONSTRAINT     [PK_Genres]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [FK_PrimaryProfession_Profession]
GO

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [FK_PrimaryProfession_Principal]
GO

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [PK_PrimaryProfession]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[Professions] 
    DROP CONSTRAINT     [PK_Professions]
    WITH (ONLINE = OFF)
GO

------------------------------------------------------------------------------------------------------

ALTER TABLE             [IMDB].[dbo].[Principals] 
    DROP CONSTRAINT     [PK_Principals]
    WITH (ONLINE = OFF)
GO

/*
    II)  Truncate data FROM Existing Tables
*/
TRUNCATE TABLE [IMDB].[dbo].[Principals]
TRUNCATE TABLE [IMDB].[dbo].[Professions]
TRUNCATE TABLE [IMDB].[dbo].[PrimaryProfessions]
TRUNCATE TABLE [IMDB].[dbo].[Genres]
TRUNCATE TABLE [IMDB].[dbo].[TitleTypes]
TRUNCATE TABLE [IMDB].[dbo].[Titles]
TRUNCATE TABLE [IMDB].[dbo].[TitleGenres]
TRUNCATE TABLE [IMDB].[dbo].[TitleNames]
TRUNCATE TABLE [IMDB].[dbo].[Attributes]
TRUNCATE TABLE [IMDB].[dbo].[TitleNameAttributes]
TRUNCATE TABLE [IMDB].[dbo].[TitlePrincipals]
TRUNCATE TABLE [IMDB].[dbo].[TitleCharacters]
TRUNCATE TABLE [IMDB].[dbo].[Episodes]
GO


/*
    III)  Add All Constraints to tables in the database
*/


/*
        1)  Principals

*/
ALTER TABLE             [IMDB].[dbo].[Principals]
    ADD  CONSTRAINT     [PK_Principals]
    PRIMARY KEY 
    CLUSTERED       (   [PrincipalId] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO

/*
        2)  Professions
*/
ALTER TABLE             [IMDB].[dbo].[Professions]
    ADD  CONSTRAINT     [PK_Professions]
    PRIMARY KEY 
    CLUSTERED       (   [ProfessionId] ASC  )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY]
GO

/*
        3)  PrimaryProfessions
*/
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    ADD  CONSTRAINT     [PK_PrimaryProfession]
    PRIMARY KEY 
    CLUSTERED       (   [PrincipalId]  ASC
                       ,[ProfessionId] ASC      )
WITH (  PAD_INDEX = OFF
	   ,STATISTICS_NORECOMPUTE = OFF
	   ,SORT_IN_TEMPDB = OFF
	   ,IGNORE_DUP_KEY = OFF
	   ,ONLINE = OFF
	   ,ALLOW_ROW_LOCKS = ON
	   ,ALLOW_PAGE_LOCKS = ON
	   ,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
	   ,DATA_COMPRESSION = PAGE
     ) 
ON [PRIMARY]
GO

/*
        3a) FK_PrimaryProfession_Principal
*/
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_PrimaryProfession_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   )
GO

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    CHECK CONSTRAINT    [FK_PrimaryProfession_Principal]
GO

/*
        3b) FK_PrimaryProfession_Profession
*/
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_PrimaryProfession_Profession]
    FOREIGN KEY     (   [ProfessionId]  )
    REFERENCES          [IMDB].[dbo].[Professions]
                    (   [ProfessionId]  )
GO

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    CHECK CONSTRAINT    [FK_PrimaryProfession_Profession]
GO

/*
        4)  Genres
*/

ALTER TABLE             [IMDB].[dbo].[Genres]
    ADD  CONSTRAINT     [PK_Genres]
    PRIMARY KEY 
    CLUSTERED       (   [GenreId] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY]
GO

/*
        5)  TitleTypes
*/
ALTER TABLE             [IMDB].[dbo].[TitleTypes]
    ADD  CONSTRAINT     [PK_TitleTypes]
    PRIMARY KEY 
    CLUSTERED       (   [TitleTypeId] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY]
GO

/*
        6)  Titles
*/
ALTER TABLE             [IMDB].[dbo].[Titles]
    ADD  CONSTRAINT     [PK_Titles]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO

/*
        6a) FK_Titles_TitleType
*/
ALTER TABLE             [IMDB].[dbo].[Titles] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_Titles_TitleType]
    FOREIGN KEY     (   [TitleTypeId]   )
    REFERENCES          [IMDB].[dbo].[TitleTypes]
                    (   [TitleTypeId]   )
GO

ALTER TABLE             [IMDB].[dbo].[Titles]
    CHECK CONSTRAINT    [FK_Titles_TitleType]
GO


/*
        7)  TitleGenres
*/
ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    ADD  CONSTRAINT     [PK_TitleGenres]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC
                       ,[GenreId] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO

/*
        7a) FK_TitleGenres_Title
*/
ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleGenres_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    CHECK CONSTRAINT    [FK_TitleGenres_Title]
GO

/*
        7b) FK_TitleGenres_Genre
*/
ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleGenres_Genre]
    FOREIGN KEY     (   [GenreId]   )
    REFERENCES          [IMDB].[dbo].[Genres]
                    (   [GenreId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    CHECK CONSTRAINT    [FK_TitleGenres_Genre]
GO

/*
        8)  TitleNames      - PK_TitleNames
*/
ALTER TABLE             [IMDB].[dbo].[TitleNames]
    ADD  CONSTRAINT     [PK_TitleNames]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC
                       ,[Ordinal] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO

/*
            8a) FK_TitleNames_Title
*/
ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNames_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitleNames]
    CHECK CONSTRAINT    [FK_TitleNames_Title]
GO

/*
            8b) create unique index     -   IX_TitleNames_Original
*/
CREATE UNIQUE 
    NONCLUSTERED INDEX  [IX_TitleNames_Original]
                ON      [IMDB].[dbo].[TitleNames]
                    (   [TitleId] ASC   )
                INCLUDE
                    (   [Title]         )
                WHERE 
                    (   [IsOriginal] = (1)  )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        DROP_EXISTING = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO

/*
        9)  Attributes
*/

ALTER TABLE             [IMDB].[dbo].[Attributes]
    ADD  CONSTRAINT     [PK_Attributes]
    PRIMARY KEY 
    CLUSTERED       (   [AttributeId] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY]
GO

/*
            9a) Unique Index        -   UQ_Attributes
*/
SET ANSI_PADDING ON
GO

ALTER TABLE             [IMDB].[dbo].[Attributes]
    ADD  CONSTRAINT     [UQ_Attributes]
    UNIQUE 
    NONCLUSTERED    (   [Class]     ASC
                       ,[Attribute] ASC     )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY]
GO

/*
        10) TitleNameAttributes     -   PK_TitleNameAttributes
*/

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    ADD  CONSTRAINT     [PK_TitleNameAttributes]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId]     ASC
                       ,[Ordinal]     ASC
                       ,[AttributeId] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO

/*
        10a)    FK_TitleNameAttributes_TitleName
*/
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNameAttributes_TitleName]
    FOREIGN KEY     (   [TitleId]
                       ,[Ordinal]   )
    REFERENCES          [IMDB].[dbo].[TitleNames]
                    (   [TitleId]
                       ,[Ordinal]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_TitleName]
GO

/*
        10b)    FK_TitleNameAttributes_Attribute
*/
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNameAttributes_Attribute]
    FOREIGN KEY     (   [AttributeId]   )
    REFERENCES          [IMDB].[dbo].[Attributes] 
                    (   [AttributeId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_Attribute]
GO

/*
        11) TitlePrincipals
*/

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    ADD  CONSTRAINT     [PK_TitlePrincipals]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC
                	   ,[Ordinal] ASC   )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO

/*
        11a)    FK_TitlePrincipals_Title
*/
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Title]
GO

/*
        11b)    FK_TitlePrincipals_Principal
*/
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Principal]
GO

/*
        11c)    FK_TitlePrincipals_Profession
*/
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Profession]
    FOREIGN KEY     (   [ProfessionId]  )
    REFERENCES          [IMDB].[dbo].[Professions]
                    (   [ProfessionId]  )
GO

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Profession]
GO

/*
        12) TitleCharacters (no primary key???)
*/

/*
        12a)    FK_TitleCharacters_Title
*/
ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Title]
GO

/*
        12b)    FK_TitleCharacters_Principal
*/
ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   )
GO

ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Principal]
GO

/*
        12c)    Index   - IX_TitleCharacters
*/
CREATE CLUSTERED INDEX  [IX_TitleCharacters]
                    ON  [IMDB].[dbo].[TitleCharacters]
                    (   [TitleId]       ASC
                       ,[PrincipalId]   ASC
                    )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        DROP_EXISTING = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY]
GO


/*
        13) Episodes    -   PK_Episodes
*/
ALTER TABLE             [IMDB].[dbo].[Episodes]
    ADD  CONSTRAINT     [PK_Episodes]
    PRIMARY KEY 
    CLUSTERED       (   [EpisodeId] ASC     )
WITH (  PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE) 
ON [PRIMARY]
GO

/*
        13a)    FK_TitleCharacters_Parent
*/
ALTER TABLE             [IMDB].[dbo].[Episodes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Parent]
    FOREIGN KEY     (   [ParentId]      )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]       )
GO

ALTER TABLE             [IMDB].[dbo].[Episodes]
    CHECK CONSTRAINT    [FK_TitleCharacters_Parent]
GO

/*
        13b)    FK_TitleCharacters_Episode
*/
ALTER TABLE             [IMDB].[dbo].[Episodes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Episode]
    FOREIGN KEY     (   [EpisodeId]     )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]       )
GO

ALTER TABLE             [IMDB].[dbo].[Episodes]
    CHECK CONSTRAINT    [FK_TitleCharacters_Episode]
GO

-------------------------------------------------
--  #4  Principals: dbo.[Principals]
-------------------------------------------------
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

--  15,380,979 Rows

-------------------------------------------------
--  #5  Professions : dbo.[Professions]
-------------------------------------------------
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

--  47 Rows

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

--  2 Rows

/*
    SELECT  *
    FROM    [IMDB].[dbo].[Professions]
*/

--  --  49 Rows total

-------------------------------------------------
--  #6  Primary professions: dbo.[PrimaryProfessions]
-------------------------------------------------
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

--  17,114,040 Rows

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


-------------------------------------------------
--  #7  Genres: dbo.[Genres]
-------------------------------------------------
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

--  28 Rows

/*
    SELECT  *
    FROM    [IMDB].[dbo].[Genres]
    ORDER   BY  [Genre]
*/

-------------------------------------------------
--  #8  Title types:    dbo.[TitleTypes]
-------------------------------------------------
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

--  11 Rows

/*
    SELECT  *
    FROM    [IMDB].[dbo].[TitleTypes]
    ORDER   BY [TitleType]
*/

-------------------------------------------------
--  #9  Titles: dbo.[Titles]
-------------------------------------------------
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

--  12,541,389 Rows

/*
    SELECT COUNT(*)    FROM [IMDB].[dbo].[Titles]
    SELECT TOP(1000) * FROM [IMDB].[dbo].[Titles]
*/

-------------------------------------------------
--  #10 Data inconsistency
--          TitleTypes: dbo.[TitleTypes]
--          Titles    : dbo.[Titles]
-------------------------------------------------
--  Data inconsistency:
--
--  Some titles only exist in the "aka"
--  table.
-------------------------------------------------
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
    --  (1 rows affected)

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


--  35 Rows

/*
    SELECT COUNT(*) FROM [IMDB].[dbo].[Titles]
*/

--  12,541,424

-------------------------------------------------
--  #11 TitleGenres:    dbo.[TitleGenres]
-------------------------------------------------
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

--  19,547,408 Rows

-------------------------------------------------
--  #12 TitleNames: dbo.[TitleNames]
-------------------------------------------------
INSERT  INTO
        [IMDB].[dbo].[TitleNames]
        WITH (TABLOCKX, HOLDLOCK) 
    (
        [TitleId]
       ,[Ordinal]
       ,[Region]
       ,[language]
       ,[IsOriginal]
       ,[Title]
    )
SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT)  AS [TitleId]
       ,[Ordering]                                AS [Ordinal]
       ,[Region]                                  AS [Region]
       ,[Language]                                AS [Language]
       ,(
            CASE 
                WHEN [Ordering] = MIN((CASE WHEN ISNULL([IsOriginalTitle], 1)=1 THEN [Ordering] END)) OVER (PARTITION BY [TitleId]) 
                THEN 1 
            ELSE 0 
            END
        )                                         AS [IsOriginal]
       ,[Title]                                   AS [Title]
FROM    [IMDB].[Raw].[title.akas.tsv.gz]
GO

--  57,452,364 Rows

/*
    SELECT COUNT(*) FROM [IMDB].[dbo].[TitleNames]
    SELECT TOP(1000) * FROM [IMDB].[dbo].[TitleNames]
*/

-------------------------------------------------
--  #13 Attributes: title attribute
--      Attributes           :  dbo.[Attributes]
--      Title Name Attributes:  dbo.[TitleNameAttributes]
-------------------------------------------------
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

-------------------------------------------------
--  #14 Attributes: Title types
--      Attributes           :  dbo.[Attributes]
--      Title Name Attributes:  dbo.[TitleNameAttributes]
-------------------------------------------------
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
    CROSS APPLY STRING_SPLIT(aka.[Types], CHAR(2))  AS a
    INNER JOIN [IMDB].[dbo].[Attributes]            AS attr ON  attr.[Class]     = 'Title types' 
                                                            AND attr.[Attribute] = a.[value]
GO

--  314,710 Rows

-------------------------------------------------
--  #15 Data inconsistency
--      Titles    : dbo.[Titles]
--      Principals: dbo.[Principals]
-------------------------------------------------
--  Data inconsistency:
-- 
--  Some titles and principals only
--  exist in the "title.principals"
--  dataset.
-------------------------------------------------
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

--  0 Rows

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
WHERE   CAST(SUBSTRING([NameId], 3, 10) AS INT) NOT IN 
            (
                SELECT  [PrincipalId] 
                FROM    [IMDB].[dbo].[Principals]
            )
GO

--  1,666 Rows

-------------------------------------------------
--  #16 Title principals:   dbo.[TitlePrincipals]
-------------------------------------------------
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

-- 99,764,730 Rows

/*
    SELECT COUNT(*) FROM [IMDB].[dbo].[TitlePrincipals]
*/

-------------------------------------------------
--  #17 Principals "known for" titles
--      Title Principals:   dbo.[TitlePrincipals]
-------------------------------------------------
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

--   Rows

-------------------------------------------------
--  #18 Title characters:   dbo.[TitleCharacters]
-------------------------------------------------
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

--  10,642,136  Rows

-------------------------------------------------
--  #19 Directors and Writers
-------------------------------------------------
--  Directors and writers
--  (there's a slight overlap with the
--  title principals here)
-------------------------------------------------
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

-------------------------------------------------
--  #20 Data inconsistency
--      Titles    : dbo.[Titles]
--      Principals: dbo.[Principals]
-------------------------------------------------
--  Data inconsistency:
-- 
--  Some of these titles and principals
--  are not in their proper respective
--  datasets.
-------------------------------------------------
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

--  0 Rows

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

--  19 Rows

-------------------------------------------------
--  #21 Now (finally) the actual title principals
--      Title Principals:   dbo.[TitlePrincipals]
-------------------------------------------------
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

--  3,401,367 Rows

DROP TABLE #writers_directors

-------------------------------------------------
--  #22 Epidodes:   dbo.[Episodes]
-------------------------------------------------
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

--  9,687,178 Rows

-------------------------------------------------
--  #23 Votes and average ratings on titles
--      Titles: dbo.[Titles]
-------------------------------------------------
--  Votes and average ratings on
--  titles.
-------------------------------------------------
UPDATE  t
    SET t.[VoteCount]     = r.[NumVotes]
       ,t.[AverageRating] = r.[AverageRating]
FROM    [IMDB].[dbo].[Titles] AS t 
        WITH (TABLOCKX, HOLDLOCK)
    INNER JOIN [IMDB].[Raw].[title.ratings.tsv.gz] AS r ON t.[TitleId] = CAST(SUBSTRING(r.[TitleId], 3, 10) AS INT)
GO

--  1,676,404 Rows
