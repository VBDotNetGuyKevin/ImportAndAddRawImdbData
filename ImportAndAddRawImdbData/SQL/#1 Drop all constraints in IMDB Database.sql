---------------------------------------------------------------------------------
-- #1 Drop all constraints in IMDB Database
---------------------------------------------------------------------------------

-- ADHOC_101 
ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [PK_Episodes]
    WITH (ONLINE = OFF);

-- ADHOC_102 
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [FK_PrimaryProfession_Principal];

-- ADHOC_103 
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [FK_PrimaryProfession_Profession];

-- ADHOC_104 
ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [FK_TitleCharacters_Episode];

-- ADHOC_105 
ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [FK_TitleCharacters_Parent];

-- ADHOC_106 
ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    DROP CONSTRAINT     [FK_TitleCharacters_Principal];

-- ADHOC_107 
ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    DROP CONSTRAINT     [FK_TitleCharacters_Title];

-- ADHOC_108 
DROP INDEX              [IX_TitleCharacters]
    ON                  [IMDB].[dbo].[TitleCharacters]
    WITH (ONLINE = OFF);

-- ADHOC_109 
ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [FK_TitleGenres_Genre];

-- ADHOC_110 
ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [FK_TitleGenres_Title];

-- ADHOC_111 
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [FK_TitleNameAttributes_Attribute];

-- ADHOC_112 
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [FK_TitleNameAttributes_TitleName];

-- ADHOC_113
ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    DROP CONSTRAINT     [FK_TitleNames_Title];

-- ADHOC_114
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Principal];

-- ADHOC_115
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Profession];

-- ADHOC_116
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Title];

-- ADHOC_117
ALTER TABLE             [IMDB].[dbo].[Titles] 
    DROP CONSTRAINT     [FK_Titles_TitleType];

-- ADHOC_118
ALTER TABLE             [IMDB].[dbo].[TitleTypes] 
    DROP CONSTRAINT     [PK_TitleTypes]
    WITH (ONLINE = OFF);

-- ADHOC_119
ALTER TABLE             [IMDB].[dbo].[Attributes] 
    DROP CONSTRAINT     [PK_Attributes]
    WITH (ONLINE = OFF);

-- ADHOC_120
ALTER TABLE             [IMDB].[dbo].[Attributes] 
    DROP CONSTRAINT     [UQ_Attributes];

-- ADHOC_121
ALTER TABLE             [IMDB].[dbo].[Genres] 
    DROP CONSTRAINT     [PK_Genres]
    WITH (ONLINE = OFF);

-- ADHOC_122
ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [PK_PrimaryProfession]
    WITH (ONLINE = OFF);

-- ADHOC_123
ALTER TABLE             [IMDB].[dbo].[Principals] 
    DROP CONSTRAINT     [PK_Principals]
    WITH (ONLINE = OFF);

-- ADHOC_124
ALTER TABLE             [IMDB].[dbo].[Professions] 
    DROP CONSTRAINT     [PK_Professions]
    WITH (ONLINE = OFF);

-- ADHOC_125
ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [PK_TitleGenres]
    WITH (ONLINE = OFF);

-- ADHOC_126
ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [PK_TitleNameAttributes]
    WITH (ONLINE = OFF);

-- ADHOC_127
DROP INDEX              [IX_TitleNames_Original]
    ON                  [IMDB].[dbo].[TitleNames];

-- ADHOC_128
ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    DROP CONSTRAINT     [PK_TitleNames]
    WITH (ONLINE = OFF);

-- ADHOC_129
ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [PK_TitlePrincipals]
    WITH (ONLINE = OFF);

-- ADHOC_130
ALTER TABLE             [IMDB].[dbo].[Titles] 
    DROP CONSTRAINT     [PK_Titles]
    WITH (ONLINE = OFF);
