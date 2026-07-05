-----------------------------------------------------------------------------------------------------------------------
-- #3 ADD all constraints into ..\IMDB Database.sql
-----------------------------------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_301 : ..\IMDB - #3-01 - CREATE PK_Principals.sql
-----------------------------------------------------------------------------------------------------------------------
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Principals]
    ADD  CONSTRAINT     [PK_Principals]
    PRIMARY KEY 
    CLUSTERED       (   [PrincipalId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
    )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_302 : ..\IMDB - #3-02 - CREATE PK_Professions.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Professions]
    ADD  CONSTRAINT     [PK_Professions]
    PRIMARY KEY 
    CLUSTERED       (   [ProfessionId] ASC  )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_303 : ..\IMDB - #3-03 - CREATE PK_PrimaryProfession.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    ADD  CONSTRAINT     [PK_PrimaryProfession]
    PRIMARY KEY 
    CLUSTERED       (   [PrincipalId]  ASC
                       ,[ProfessionId] ASC      )
WITH (  
		PAD_INDEX = OFF
	   ,STATISTICS_NORECOMPUTE = OFF
	   ,SORT_IN_TEMPDB = OFF
	   ,IGNORE_DUP_KEY = OFF
	   ,ONLINE = OFF
	   ,ALLOW_ROW_LOCKS = ON
	   ,ALLOW_PAGE_LOCKS = ON
	   ,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
	   ,DATA_COMPRESSION = PAGE
     ) 
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_304 : ..\IMDB - #3-04 - CREATE FK_PrimaryProfession_Principal.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_PrimaryProfession_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_305 : ..\IMDB - #3-05 - CHECK CONSTRAINT FK_PrimaryProfession_Principal.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    CHECK CONSTRAINT    [FK_PrimaryProfession_Principal];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_306 : ..\IMDB - #3-06 - CREATE FK_PrimaryProfession_Profession.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_PrimaryProfession_Profession]
    FOREIGN KEY     (   [ProfessionId]  )
    REFERENCES          [IMDB].[dbo].[Professions]
                    (   [ProfessionId]  );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_307 : ..\IMDB - #3-07 - CHECK CONSTRAINT FK_PrimaryProfession_Profession.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    CHECK CONSTRAINT    [FK_PrimaryProfession_Profession];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_308 : ..\IMDB - #3-08 - CREATE PK_Genres.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Genres]
    ADD  CONSTRAINT     [PK_Genres]
    PRIMARY KEY 
    CLUSTERED       (   [GenreId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_309 : ..\IMDB - #3-09 - CREATE PK_TitleTypes.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleTypes]
    ADD  CONSTRAINT     [PK_TitleTypes]
    PRIMARY KEY 
    CLUSTERED       (   [TitleTypeId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_310 : ..\IMDB - #3-10 - CREATE PK_Titles.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Titles]
    ADD  CONSTRAINT     [PK_Titles]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_311 : ..\IMDB - #3-11 - CREATE FK_Titles_TitleType.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Titles] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_Titles_TitleType]
    FOREIGN KEY     (   [TitleTypeId]   )
    REFERENCES          [IMDB].[dbo].[TitleTypes]
                    (   [TitleTypeId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_312 : ..\IMDB - #3-12 - CHECK CONSTRAINT FK_Titles_TitleType.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Titles]
    CHECK CONSTRAINT    [FK_Titles_TitleType];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_313 : ..\IMDB - #3-13 - CREATE PK_TitleGenres.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    ADD  CONSTRAINT     [PK_TitleGenres]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC
                       ,[GenreId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_314 : ..\IMDB - #3-14 - CREATE FK_TitleGenres_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleGenres_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_315 : ..\IMDB - #3-15 - CHECK CONSTRAINT FK_TitleGenres_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    CHECK CONSTRAINT    [FK_TitleGenres_Title];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_316 : ..\IMDB - #3-16 - CREATE FK_TitleGenres_Genre.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleGenres_Genre]
    FOREIGN KEY     (   [GenreId]   )
    REFERENCES          [IMDB].[dbo].[Genres]
                    (   [GenreId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_317 : ..\IMDB - #3-17 - CHECK CONSTRAINT FK_TitleGenres_Genre.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    CHECK CONSTRAINT    [FK_TitleGenres_Genre];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_318 : ..\IMDB - #3-18 - CREATE PK_TitleNames.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNames]
    ADD  CONSTRAINT     [PK_TitleNames]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC
                       ,[Ordinal] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_319 : ..\IMDB - #3-19 - CREATE FK_TitleNames_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNames_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_320 : ..\IMDB - #3-20 - CHECK CONSTRAINT FK_TitleNames_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNames]
    CHECK CONSTRAINT    [FK_TitleNames_Title];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_321 : ..\IMDB - #3-21 - CREATE IX_TitleNames_Original.sql
-----------------------------------------------------------------------------------------------------------------------
CREATE UNIQUE
    NONCLUSTERED INDEX  [IX_TitleNames_Original]
                ON      [IMDB].[dbo].[TitleNames]
                    (   [TitleId] ASC   )
                INCLUDE
                    (   [Title]         )
                WHERE 
                    (   [IsOriginal] = (1)  )
WITH (  
        PAD_INDEX = OFF, 
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
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_322 : ..\IMDB - #3-22 - CREATE PK_Attributes.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Attributes]
    ADD  CONSTRAINT     [PK_Attributes]
    PRIMARY KEY 
    CLUSTERED       (   [AttributeId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_323 : ..\IMDB - #3-23 - CREATE UQ_Attributes.sql
-----------------------------------------------------------------------------------------------------------------------
SET ANSI_PADDING ON

ALTER TABLE             [IMDB].[dbo].[Attributes]
    ADD  CONSTRAINT     [UQ_Attributes]
    UNIQUE 
    NONCLUSTERED    (   [Class]     ASC
                       ,[Attribute] ASC     )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_324 : ..\IMDB - #3-24 - CREATE PK_TitleNameAttributes.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    ADD  CONSTRAINT     [PK_TitleNameAttributes]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId]     ASC
                       ,[Ordinal]     ASC
                       ,[AttributeId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_325 : ..\IMDB - #3-25 - CREATE FK_TitleNameAttributes_TitleName.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNameAttributes_TitleName]
    FOREIGN KEY     (   [TitleId]
                       ,[Ordinal]   )
    REFERENCES          [IMDB].[dbo].[TitleNames]
                    (   [TitleId]
                       ,[Ordinal]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_326 : ..\IMDB - #3-26 - CHECK CONSTRAINT FK_TitleNameAttributes_TitleName.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_TitleName];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_327 : ..\IMDB - #3-27 - CREATE FK_TitleNameAttributes_Attribute.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNameAttributes_Attribute]
    FOREIGN KEY     (   [AttributeId]   )
    REFERENCES          [IMDB].[dbo].[Attributes] 
                    (   [AttributeId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_328 : ..\IMDB - #3-28 - CHECK CONSTRAINT FK_TitleNameAttributes_Attribute.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_Attribute];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_329 : ..\IMDB - #3-29 - CREATE PK_TitlePrincipals.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    ADD  CONSTRAINT     [PK_TitlePrincipals]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC
                	   ,[Ordinal] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_330 : ..\IMDB - #3-30 - CREATE FK_TitlePrincipals_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_331 : ..\IMDB - #3-31 - CHECK CONSTRAINT FK_TitlePrincipals_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Title];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_332 : ..\IMDB - #3-32 - CREATE FK_TitlePrincipals_Principal.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_333 : ..\IMDB - #3-33 - CHECK CONSTRAINT FK_TitlePrincipals_Principal.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Principal];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_334 : ..\IMDB - #3-34 - CREATE FK_TitlePrincipals_Profession.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Profession]
    FOREIGN KEY     (   [ProfessionId]  )
    REFERENCES          [IMDB].[dbo].[Professions]
                    (   [ProfessionId]  );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_335 : ..\IMDB - #3-35 - CHECK CONSTRAINT FK_TitlePrincipals_Profession.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Profession];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_336 : ..\IMDB - #3-36 - CREATE FK_TitleCharacters_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_337 : ..\IMDB - #3-37 - CHECK CONSTRAINT FK_TitleCharacters_Title.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Title];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_338 : ..\IMDB - #3-38 - CREATE FK_TitleCharacters_Principal.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_339 : ..\IMDB - #3-39 - CHECK CONSTRAINT FK_TitleCharacters_Principal.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Principal];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_340 : ..\IMDB - #3-40 - CREATE IX_TitleCharacters.sql
-----------------------------------------------------------------------------------------------------------------------
CREATE CLUSTERED INDEX  [IX_TitleCharacters]
                    ON  [IMDB].[dbo].[TitleCharacters]
                    (   [TitleId]       ASC
                       ,[PrincipalId]   ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        DROP_EXISTING = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_341 : ..\IMDB - #3-41 - CREATE PK_Episodes.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Episodes]
    ADD  CONSTRAINT     [PK_Episodes]
    PRIMARY KEY 
    CLUSTERED       (   [EpisodeId] ASC     )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     ) 
ON [PRIMARY];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_342 : ..\IMDB - #3-42 - CREATE FK_TitleCharacters_Parent.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Episodes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Parent]
    FOREIGN KEY     (   [ParentId]      )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]       );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_343 : ..\IMDB - #3-43 - CHECK CONSTRAINT FK_TitleCharacters_Parent.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Episodes]
    CHECK CONSTRAINT    [FK_TitleCharacters_Parent];

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_344 : ..\IMDB - #3-44 - CREATE FK_TitleCharacters_Episode.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Episodes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Episode]
    FOREIGN KEY     (   [EpisodeId]     )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]       );

-----------------------------------------------------------------------------------------------------------------------
-- ADHOC_345 : ..\IMDB - #3-45 - CHECK CONSTRAINT FK_TitleCharacters_Episode.sql
-----------------------------------------------------------------------------------------------------------------------
ALTER TABLE             [IMDB].[dbo].[Episodes]
    CHECK CONSTRAINT    [FK_TitleCharacters_Episode];
