Imports AH25 = ImportAndAddRawImdbData.Constants.AdHoc2_5TableNameEnum

''' <summary>
''' This class contains all the constants used in the ImportAndAddRawImdbData 
''' project. It includes connection strings, file names, SQL commands, and 
''' other configuration values that are used throughout the application.
''' </summary>
Public Class Constants

    Public Const DEFAULT_COMMIT_COUNT As Integer = 10000

    Public Const IMDB_CONNECTION_STRING As String =
                 "Data Source=DESKTOP-GKQ9F83\SQL2K22;" &
                 "Initial Catalog=IMDB;" &
                 "Integrated Security=True;" &
                 "Trust Server Certificate=True"

    Public Const COMMA_MASK As String =
                 "###,###,###,###,##0"

    Public Const CompressedFileExtension As String =
                 ".tsv.gz"
    Public Const UnCompressedFileExtension As String =
                 ".tsv"

    Public Const NameBasicsType As String =
                 "name.basics"
    Public Const TitleAkasType As String =
                 "title.akas"
    Public Const TitleBasicsType As String =
                 "title.basics"
    Public Const TitleCrewType As String =
                 "title.crew"
    Public Const TitleEpisodeType As String =
                 "title.episode"
    Public Const TitlePrincipalsType As String =
                 "title.principals"
    Public Const TitleRatingsType As String =
                 "title.ratings"

    Public Const NameBasicsCompressedFileName As String =
                 "name.basics.tsv.gz"
    Public Const TitleAkasCompressedFileName As String =
                 "title.akas.tsv.gz"
    Public Const TitleBasicsCompressedFileName As String =
                 "title.basics.tsv.gz"
    Public Const TitleCrewCompressedFileName As String =
                 "title.crew.tsv.gz"
    Public Const TitleEpisodeCompressedFileName As String =
                 "title.episode.tsv.gz"
    Public Const TitlePrincipalsCompressedFileName As String =
                 "title.principals.tsv.gz"
    Public Const TitleRatingsCompressedFileName As String =
                 "title.ratings.tsv.gz"

    Public Const NameBasicsDecompFileName As String =
                 "name.basics.tsv"
    Public Const TitleAkasDecompFileName As String =
                 "title.akas.tsv"
    Public Const TitleBasicsDecompFileName As String =
                 "title.basics.tsv"
    Public Const TitleCrewDecompFileName As String =
                 "title.crew.tsv"
    Public Const TitleEpisodeDecompFileName As String =
                 "title.episode.tsv"
    Public Const TitlePrincipalsDecompFileName As String =
                 "title.principals.tsv"
    Public Const TitleRatingsDecompFileName As String =
                 "title.ratings.tsv"

    Public Const DASHES As String =
                 "------------------------------------------------------------------------------------------"
    Public Const EQUALSIGNS As String =
                 "=========================================================================================="

    Public Const DEFAULT_TIMEOUT As Integer = 30

    '===================================================================================
    '  STEP #1
    '===================================================================================

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-01 - DROP PK_Episodes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_101 As String =
        "ALTER TABLE [IMDB].[dbo].[Episodes] " & vbCrLf &
        "    DROP CONSTRAINT [PK_Episodes] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-02 - DROP FK_PrimaryProfession_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_102 As String =
        "ALTER TABLE [IMDB].[dbo].[PrimaryProfessions] " & vbCrLf &
        "    DROP CONSTRAINT [FK_PrimaryProfession_Principal];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-03 - DROP FK_PrimaryProfession_Profession.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_103 As String =
        "ALTER TABLE [IMDB].[dbo].[PrimaryProfessions] " & vbCrLf &
        "    DROP CONSTRAINT [FK_PrimaryProfession_Profession];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-04 - DROP FK_TitleCharacters_Episode.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_104 As String =
        "ALTER TABLE [IMDB].[dbo].[Episodes] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleCharacters_Episode];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-05 - DROP FK_TitleCharacters_Parent.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_105 As String =
        "ALTER TABLE [IMDB].[dbo].[Episodes] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleCharacters_Parent];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-06 - DROP FK_TitleCharacters_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_106 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleCharacters] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleCharacters_Principal];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-07 - DROP FK_TitleCharacters_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_107 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleCharacters] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleCharacters_Title];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-08 - DROP IX_TitleCharacters.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_108 As String =
        "DROP INDEX [IX_TitleCharacters] " & vbCrLf &
        "    ON [IMDB].[dbo].[TitleCharacters] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-09 - DROP FK_TitleGenres_Genre.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_109 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleGenres] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleGenres_Genre];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-10 - DROP FK_TitleGenres_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_110 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleGenres] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleGenres_Title];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-11 - DROP FK_TitleNameAttributes_Attribute.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_111 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNameAttributes] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleNameAttributes_Attribute];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-12 - DROP FK_TitleNameAttributes_TitleName.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_112 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNameAttributes] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleNameAttributes_TitleName];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-13 - DROP FK_TitleNames_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_113 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNames] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitleNames_Title];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-14 - DROP FK_TitlePrincipals_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_114 As String =
        "ALTER TABLE [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitlePrincipals_Principal];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-15 - DROP FK_TitlePrincipals_Profession.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_115 As String =
        "ALTER TABLE [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitlePrincipals_Profession];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-16 - DROP FK_TitlePrincipals_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_116 As String =
        "ALTER TABLE [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "    DROP CONSTRAINT [FK_TitlePrincipals_Title];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-17 - DROP FK_Titles_TitleType.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_117 As String =
        "ALTER TABLE [IMDB].[dbo].[Titles] " & vbCrLf &
        "    DROP CONSTRAINT [FK_Titles_TitleType];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-18 - DROP PK_TitleTypes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_118 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleTypes] " & vbCrLf &
        "    DROP CONSTRAINT [PK_TitleTypes] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-19 - DROP PK_Attributes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_119 As String =
        "ALTER TABLE [IMDB].[dbo].[Attributes] " & vbCrLf &
        "    DROP CONSTRAINT [PK_Attributes] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-20 - DROP UQ_Attributes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_120 As String =
        "ALTER TABLE [IMDB].[dbo].[Attributes] " & vbCrLf &
        "    DROP CONSTRAINT [UQ_Attributes];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-21 - DROP PK_Genres.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_121 As String =
        "ALTER TABLE [IMDB].[dbo].[Genres] " & vbCrLf &
        "    DROP CONSTRAINT [PK_Genres] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-22 - DROP PK_PrimaryProfession.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_122 As String =
        "ALTER TABLE [IMDB].[dbo].[PrimaryProfessions] " & vbCrLf &
        "    DROP CONSTRAINT [PK_PrimaryProfession] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------    
    '  IMDB - #1-23 - DROP PK_Principals.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_123 As String =
        "ALTER TABLE [IMDB].[dbo].[Principals] " & vbCrLf &
        "    DROP CONSTRAINT [PK_Principals] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-24 - DROP PK_Professions.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_124 As String =
        "ALTER TABLE [IMDB].[dbo].[Professions] " & vbCrLf &
        "    DROP CONSTRAINT [PK_Professions] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-25 - DROP PK_TitleGenres.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_125 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleGenres] " & vbCrLf &
        "    DROP CONSTRAINT [PK_TitleGenres] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-26 - DROP PK_TitleNameAttributes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_126 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNameAttributes] " & vbCrLf &
        "    DROP CONSTRAINT [PK_TitleNameAttributes] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-27 - DROP IX_TitleNames_Original.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_127 As String =
        "DROP INDEX [IX_TitleNames_Original] " & vbCrLf &
        "    ON [IMDB].[dbo].[TitleNames];"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-28 - DROP PK_TitleNames.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_128 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNames] " & vbCrLf &
        "    DROP CONSTRAINT [PK_TitleNames] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-29 - DROP PK_TitlePrincipals.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_129 As String =
        "ALTER TABLE [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "    DROP CONSTRAINT [PK_TitlePrincipals] WITH (ONLINE = OFF);"

    '-----------------------------------------------------------------------------------
    '  IMDB - #1-30 - DROP PK_Titles.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_130 As String =
        "ALTER TABLE [IMDB].[dbo].[Titles] " & vbCrLf &
        "    DROP CONSTRAINT [PK_Titles] WITH (ONLINE = OFF);"

    Public Const ADHOC_COUNT_1_MAX As Integer = 30

    Public Shared ReadOnly Property AdHoc1List As New SortedList(Of Integer, String) From
        {
            {1, ADHOC_101}, {2, ADHOC_102}, {3, ADHOC_103}, {4, ADHOC_104}, {5, ADHOC_105},
            {6, ADHOC_106}, {7, ADHOC_107}, {8, ADHOC_108}, {9, ADHOC_109}, {10, ADHOC_110},
            {11, ADHOC_111}, {12, ADHOC_112}, {13, ADHOC_113}, {14, ADHOC_114}, {15, ADHOC_115},
            {16, ADHOC_116}, {17, ADHOC_117}, {18, ADHOC_118}, {19, ADHOC_119}, {20, ADHOC_120},
            {21, ADHOC_121}, {22, ADHOC_122}, {23, ADHOC_123}, {24, ADHOC_124}, {25, ADHOC_125},
            {26, ADHOC_126}, {27, ADHOC_127}, {28, ADHOC_128}, {29, ADHOC_129}, {30, ADHOC_130}
        }

    '===================================================================================
    '  STEP #2
    '===================================================================================
    Public Const BASIC_LOG_MESSAGE_2 As String =
        "#2 Truncating Existing IMDB Db Tables"

    Public Const ADHOC_201 As String = "TRUNCATE TABLE [IMDB].[dbo].[Attributes];"
    Public Const ADHOC_202 As String = "TRUNCATE TABLE [IMDB].[dbo].[Episodes];"
    Public Const ADHOC_203 As String = "TRUNCATE TABLE [IMDB].[dbo].[Genres];"
    Public Const ADHOC_204 As String = "TRUNCATE TABLE [IMDB].[dbo].[PrimaryProfessions];"
    Public Const ADHOC_205 As String = "TRUNCATE TABLE [IMDB].[dbo].[Principals];"
    Public Const ADHOC_206 As String = "TRUNCATE TABLE [IMDB].[dbo].[Professions];"
    Public Const ADHOC_207 As String = "TRUNCATE TABLE [IMDB].[dbo].[TitleCharacters];"
    Public Const ADHOC_208 As String = "TRUNCATE TABLE [IMDB].[dbo].[TitleGenres];"
    Public Const ADHOC_209 As String = "TRUNCATE TABLE [IMDB].[dbo].[TitleNameAttributes];"
    Public Const ADHOC_210 As String = "TRUNCATE TABLE [IMDB].[dbo].[TitleNames];"
    Public Const ADHOC_211 As String = "TRUNCATE TABLE [IMDB].[dbo].[TitlePrincipals];"
    Public Const ADHOC_212 As String = "TRUNCATE TABLE [IMDB].[dbo].[Titles];"
    Public Const ADHOC_213 As String = "TRUNCATE TABLE [IMDB].[dbo].[TitleTypes];"

    Public Const ADHOC_TABLE_COUNT_MIN As AH25 = AH25.Attributes
    Public Const ADHOC_TABLE_COUNT_MAX As AH25 = AH25.TitleTypes

    Public Shared ReadOnly Property AdHoc2List As New SortedList(Of AH25, String) From
        {
            {AH25.Attributes, ADHOC_201},
            {AH25.Episodes, ADHOC_202},
            {AH25.Genres, ADHOC_203},
            {AH25.PrimaryProfessions, ADHOC_204},
            {AH25.Principals, ADHOC_205},
            {AH25.Professions, ADHOC_206},
            {AH25.TitleCharacters, ADHOC_207},
            {AH25.TitleGenres, ADHOC_208},
            {AH25.TitleNameAttributes, ADHOC_209},
            {AH25.TitleNames, ADHOC_210},
            {AH25.TitlePrincipals, ADHOC_211},
            {AH25.Titles, ADHOC_212},
            {AH25.TitleTypes, ADHOC_213}
        }

    '===================================================================================
    '  STEP #3
    '===================================================================================

    '-----------------------------------------------------------------------------------
    ' ADHOC_301 : ..\IMDB - #3-01 - CREATE PK_Principals.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_301 As String =
        "ALTER TABLE             [IMDB].[dbo].[Principals]                 " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_Principals]                           " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED       (   [PrincipalId] ASC   )                     " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_302 : ..\IMDB - #3-02 - CREATE PK_Professions.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_302 As String =
        "ALTER TABLE             [IMDB].[dbo].[Professions]                 " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_Professions]                           " & vbCrLf &
        "    PRIMARY KEY                                                    " & vbCrLf &
        "    CLUSTERED       (   [ProfessionId] ASC  )                      " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,             " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,                " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                        " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF ) " & vbCrLf &
        "ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_303 : ..\IMDB - #3-03 - CREATE PK_PrimaryProfessions.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_303 As String =
        "ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]         " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_PrimaryProfession]                    " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED ( [PrincipalId] ASC, [ProfessionId] ASC )           " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_304 : ..\IMDB - #3-04 - CREATE FK_PrimaryProfession_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_304 As String =
        "ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] " & vbCrLf &
        "    WITH CHECK                                            " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_PrimaryProfession_Principal]  " & vbCrLf &
        "    FOREIGN KEY     (   [PrincipalId]   )                 " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Principals]         " & vbCrLf &
        "                    (   [PrincipalId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_305 : ..\IMDB - #3-05 - CHECK CONSTRAINT FK_PrimaryProfession_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_305 As String =
        "ALTER TABLE [IMDB].[dbo].[PrimaryProfessions] " & vbCrLf &
        "CHECK CONSTRAINT [FK_PrimaryProfession_Principal];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_306 : ..\IMDB - #3-06 - CREATE FK_PrimaryProfession_Profession.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_306 As String =
        "ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] " & vbCrLf &
        "    WITH CHECK                                            " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_PrimaryProfession_Profession] " & vbCrLf &
        "    FOREIGN KEY     (   [ProfessionId]  )                 " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Professions]        " & vbCrLf &
        "                    (   [ProfessionId]  );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_307 : ..\IMDB - #3-07 - CHECK CONSTRAINT FK_PrimaryProfession_Profession.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_307 As String =
        "ALTER TABLE [IMDB].[dbo].[PrimaryProfessions] " & vbCrLf &
        "CHECK CONSTRAINT [FK_PrimaryProfession_Profession];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_308 : ..\IMDB - #3-08 - CREATE PK_Genres.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_308 As String =
        "ALTER TABLE             [IMDB].[dbo].[Genres]                      " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_Genres]                                " & vbCrLf &
        "    PRIMARY KEY                                                    " & vbCrLf &
        "    CLUSTERED       (   [GenreId] ASC   )                          " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,             " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,                " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                        " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF ) " & vbCrLf &
        "ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_309 : ..\IMDB - #3-09 - CREATE PK_TitleTypes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_309 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleTypes]                  " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_TitleTypes]                            " & vbCrLf &
        "    PRIMARY KEY                                                    " & vbCrLf &
        "    CLUSTERED       (   [TitleTypeId] ASC   )                      " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,             " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,                " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                        " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF ) " & vbCrLf &
        "ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_310 : ..\IMDB - #3-10 - CREATE PK_Titles.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_310 As String =
        "ALTER TABLE             [IMDB].[dbo].[Titles]                     " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_Titles]                               " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED       (   [TitleId] ASC   )                         " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_311 : ..\IMDB - #3-11 - CREATE FK_Titles_TitleType.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_311 As String =
        "ALTER TABLE             [IMDB].[dbo].[Titles]     " & vbCrLf &
        "    WITH CHECK                                    " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_Titles_TitleType]     " & vbCrLf &
        "    FOREIGN KEY     (   [TitleTypeId]   )         " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[TitleTypes] " & vbCrLf &
        "                    (   [TitleTypeId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_312 : ..\IMDB - #3-12 - CHECK CONSTRAINT FK_Titles_TitleType.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_312 As String =
        "ALTER TABLE [IMDB].[dbo].[Titles] " & vbCrLf &
        "CHECK CONSTRAINT [FK_Titles_TitleType];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_313 : ..\IMDB - #3-13 - CREATE PK_TitleGenres.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_313 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleGenres]                " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_TitleGenres]                          " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED       (   [TitleId] ASC, [GenreId] ASC   )          " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_314 : ..\IMDB - #3-14 - CREATE FK_TitleGenres_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_314 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleGenres] " & vbCrLf &
        "    WITH CHECK                                     " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleGenres_Title]     " & vbCrLf &
        "    FOREIGN KEY     (   [TitleId]   )              " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Titles]      " & vbCrLf &
        "                    (   [TitleId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_315 : ..\IMDB - #3-15 - CHECK CONSTRAINT FK_TitleGenres_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_315 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleGenres] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleGenres_Title];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_316 : ..\IMDB - #3-16 - CREATE FK_TitleGenres_Genre.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_316 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleGenres] " & vbCrLf &
        "    WITH CHECK                                     " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleGenres_Genre]     " & vbCrLf &
        "    FOREIGN KEY     (   [GenreId]   )              " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Genres]      " & vbCrLf &
        "                    (   [GenreId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_317 : ..\IMDB - #3-17 - CHECK CONSTRAINT FK_TitleGenres_Genre.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_317 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleGenres] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleGenres_Genre];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_318 : ..\IMDB - #3-18 - CREATE PK_TitleNames.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_318 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleNames]                 " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_TitleNames]                           " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED       (   [TitleId] ASC, [Ordinal] ASC   )          " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_319 : ..\IMDB - #3-19 - CREATE FK_TitleNames_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_319 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleNames]      " & vbCrLf &
        "    WITH CHECK                                         " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleNames_Title]          " & vbCrLf &
        "    FOREIGN KEY     (   [TitleId]   )                  " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Titles]          " & vbCrLf &
        "                    (   [TitleId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_320 : ..\IMDB - #3-20 - CHECK CONSTRAINT FK_TitleNames_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_320 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNames] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleNames_Title];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_321 : ..\IMDB - #3-21 - CREATE IX_TitleNames_Original.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_321 As String =
        "CREATE UNIQUE                                          " & vbCrLf &
        "    NONCLUSTERED INDEX  [IX_TitleNames_Original]       " & vbCrLf &
        "                ON      [IMDB].[dbo].[TitleNames]      " & vbCrLf &
        "                    (   [TitleId] ASC   )              " & vbCrLf &
        "                INCLUDE                                " & vbCrLf &
        "                    (   [Title]         )              " & vbCrLf &
        "                WHERE                                  " & vbCrLf &
        "                    (   [IsOriginal] = (1)  )          " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,    " & vbCrLf &
        "        DROP_EXISTING = OFF, ONLINE = OFF,             " & vbCrLf &
        "        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON,   " & vbCrLf &
        "        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF,             " & vbCrLf &
        "        DATA_COMPRESSION = PAGE )                      " & vbCrLf &
        "ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_322 : ..\IMDB - #3-22 - CREATE PK_Attributes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_322 As String =
        "ALTER TABLE             [IMDB].[dbo].[Attributes]      " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_Attributes]                " & vbCrLf &
        "    PRIMARY KEY                                        " & vbCrLf &
        "    CLUSTERED       (   [AttributeId] ASC   )          " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,    " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,            " & vbCrLf &
        "        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON,   " & vbCrLf &
        "        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_323 : ..\IMDB - #3-23 - CREATE UQ_Attributes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_323 As String =
        "SET ANSI_PADDING ON                                                " & vbCrLf &
        "ALTER TABLE             [IMDB].[dbo].[Attributes]                  " & vbCrLf &
        "    ADD  CONSTRAINT     [UQ_Attributes]                            " & vbCrLf &
        "    UNIQUE                                                         " & vbCrLf &
        "    NONCLUSTERED ( [Class] ASC, [Attribute] ASC )                  " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,             " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,                " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                        " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF ) " & vbCrLf &
        "ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_324 : ..\IMDB - #3-24 - CREATE PK_TitleNameAttributes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_324 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]        " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_TitleNameAttributes]                  " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED ( [TitleId] ASC, [Ordinal] ASC, [AttributeId] ASC ) " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_325 : ..\IMDB - #3-25 - CREATE FK_TitleNameAttributes_TitleName.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_325 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] " & vbCrLf &
        "    WITH CHECK                                             " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleNameAttributes_TitleName] " & vbCrLf &
        "    FOREIGN KEY     (   [TitleId], [Ordinal]   )           " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[TitleNames]          " & vbCrLf &
        "                    (   [TitleId], [Ordinal]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_326 : ..\IMDB - #3-26 - CHECK CONSTRAINT FK_TitleNameAttributes_TitleName.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_326 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNameAttributes] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleNameAttributes_TitleName];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_327 : ..\IMDB - #3-27 - CREATE FK_TitleNameAttributes_Attribute.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_327 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] " & vbCrLf &
        "    WITH CHECK                                             " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleNameAttributes_Attribute] " & vbCrLf &
        "    FOREIGN KEY     (   [AttributeId]   )                  " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Attributes]          " & vbCrLf &
        "                    (   [AttributeId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_328 : ..\IMDB - #3-28 - CHECK CONSTRAINT FK_TitleNameAttributes_Attribute.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_328 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleNameAttributes] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleNameAttributes_Attribute];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_329 : ..\IMDB - #3-29 - CREATE PK_TitlePrincipals.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_329 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]            " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_TitlePrincipals]                      " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED       (   [TitleId] ASC, [Ordinal] ASC   )          " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_330 : ..\IMDB - #3-30 - CREATE FK_TitlePrincipals_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_330 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "    WITH CHECK                                         " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitlePrincipals_Title]     " & vbCrLf &
        "    FOREIGN KEY     (   [TitleId]   )                  " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Titles]          " & vbCrLf &
        "                    (   [TitleId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_331 : ..\IMDB - #3-31 - CHECK CONSTRAINT FK_TitlePrincipals_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_331 As String =
        "ALTER TABLE [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitlePrincipals_Title];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_332 : ..\IMDB - #3-32 - CREATE FK_TitlePrincipals_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_332 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "    WITH CHECK                                         " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitlePrincipals_Principal] " & vbCrLf &
        "    FOREIGN KEY     (   [PrincipalId]   )              " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Principals]      " & vbCrLf &
        "                    (   [PrincipalId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_333 : ..\IMDB - #3-33 - CHECK CONSTRAINT FK_TitlePrincipals_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_333 As String =
        "ALTER TABLE [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitlePrincipals_Principal];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_334 : ..\IMDB - #3-34 - CREATE FK_TitlePrincipals_Profession.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_334 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]  " & vbCrLf &
        "    WITH CHECK                                          " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitlePrincipals_Profession] " & vbCrLf &
        "    FOREIGN KEY     (   [ProfessionId]  )               " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Professions]      " & vbCrLf &
        "                    (   [ProfessionId]  );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_335 : ..\IMDB - #3-35 - CHECK CONSTRAINT FK_TitlePrincipals_Profession.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_335 As String =
        "ALTER TABLE [IMDB].[dbo].[TitlePrincipals] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitlePrincipals_Profession];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_336 : ..\IMDB - #3-36 - CREATE FK_TitleCharacters_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_336 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleCharacters] " & vbCrLf &
        "    WITH CHECK                                         " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleCharacters_Title]     " & vbCrLf &
        "    FOREIGN KEY     (   [TitleId]   )                  " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Titles]          " & vbCrLf &
        "                    (   [TitleId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_337 : ..\IMDB - #3-37 - CHECK CONSTRAINT FK_TitleCharacters_Title.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_337 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleCharacters] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleCharacters_Title];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_338 : ..\IMDB - #3-38 - CREATE FK_TitleCharacters_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_338 As String =
        "ALTER TABLE             [IMDB].[dbo].[TitleCharacters] " & vbCrLf &
        "    WITH CHECK                                         " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleCharacters_Principal] " & vbCrLf &
        "    FOREIGN KEY     (   [PrincipalId]   )              " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Principals]      " & vbCrLf &
        "                    (   [PrincipalId]   );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_339 : ..\IMDB - #3-39 - CHECK CONSTRAINT FK_TitleCharacters_Principal.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_339 As String =
        "ALTER TABLE [IMDB].[dbo].[TitleCharacters] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleCharacters_Principal];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_340 : ..\IMDB - #3-40 - CREATE IX_TitleCharacters.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_340 As String =
        "CREATE CLUSTERED INDEX  [IX_TitleCharacters]                      " & vbCrLf &
        "                    ON  [IMDB].[dbo].[TitleCharacters]            " & vbCrLf &
        "                      ( [TitleId] ASC, [PrincipalId] ASC )        " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF,                " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_341 : #41  - ..\IMDB - #3-41 - CREATE PK_Episodes.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_341 As String =
        "ALTER TABLE             [IMDB].[dbo].[Episodes]                   " & vbCrLf &
        "    ADD  CONSTRAINT     [PK_Episodes]                             " & vbCrLf &
        "    PRIMARY KEY                                                   " & vbCrLf &
        "    CLUSTERED       (   [EpisodeId] ASC   )                       " & vbCrLf &
        "WITH (  PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,            " & vbCrLf &
        "        SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF,               " & vbCrLf &
        "        ONLINE = OFF, ALLOW_ROW_LOCKS = ON,                       " & vbCrLf &
        "        ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, " & vbCrLf &
        "        DATA_COMPRESSION = PAGE  ) ON [PRIMARY];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_342 : ..\IMDB - #3-42 - CREATE FK_TitleCharacters_Parent.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_342 As String =
        "ALTER TABLE             [IMDB].[dbo].[Episodes]     " & vbCrLf &
        "    WITH CHECK                                      " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleCharacters_Parent] " & vbCrLf &
        "    FOREIGN KEY     (   [ParentId]      )           " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Titles]       " & vbCrLf &
        "                    (   [TitleId]       );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_343 : ..\IMDB - #3-43 - CHECK CONSTRAINT FK_TitleCharacters_Parent.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_343 As String =
        "ALTER TABLE [IMDB].[dbo].[Episodes] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleCharacters_Parent];"

    '-----------------------------------------------------------------------------------
    ' ADHOC_344 : ..\IMDB - #3-44 - CREATE FK_TitleCharacters_Episode.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_344 As String =
        "ALTER TABLE             [IMDB].[dbo].[Episodes]      " & vbCrLf &
        "    WITH CHECK                                       " & vbCrLf &
        "    ADD  CONSTRAINT     [FK_TitleCharacters_Episode] " & vbCrLf &
        "    FOREIGN KEY     (   [EpisodeId]     )            " & vbCrLf &
        "    REFERENCES          [IMDB].[dbo].[Titles]        " & vbCrLf &
        "                    (   [TitleId]       );"

    '-----------------------------------------------------------------------------------
    ' ADHOC_345 : ..\IMDB - #3-45 - CHECK CONSTRAINT FK_TitleCharacters_Episode.sql
    '-----------------------------------------------------------------------------------
    Public Const ADHOC_345 As String =
        "ALTER TABLE [IMDB].[dbo].[Episodes] " & vbCrLf &
        "CHECK CONSTRAINT [FK_TitleCharacters_Episode];"

    Public Const ADHOC_COUNT_3_MAX As Integer = 45

    ''' <summary>
    ''' Returns a sorted list of all the AdHoc SQL statements for Step #3, 
    ''' which is the creation of the IMDB database schema and its associated 
    ''' tables, indexes, and constraints. Each entry in the list is keyed by 
    ''' an integer representing the step number and maps to a string containing 
    ''' the corresponding SQL command.
    ''' </summary>
    ''' <returns>A sorted list of AdHoc SQL statements for Step #3.</returns>
    Public Shared ReadOnly Property AdHoc3List As New SortedList(Of Integer, String) From
        {
            {1, ADHOC_301}, {2, ADHOC_302}, {3, ADHOC_303}, {4, ADHOC_304}, {5, ADHOC_305},
            {6, ADHOC_306}, {7, ADHOC_307}, {8, ADHOC_308}, {9, ADHOC_309}, {10, ADHOC_310},
            {11, ADHOC_311}, {12, ADHOC_312}, {13, ADHOC_313}, {14, ADHOC_314}, {15, ADHOC_315},
            {16, ADHOC_316}, {17, ADHOC_317}, {18, ADHOC_318}, {19, ADHOC_319}, {20, ADHOC_320},
            {21, ADHOC_321}, {22, ADHOC_322}, {23, ADHOC_323}, {24, ADHOC_324}, {25, ADHOC_325},
            {26, ADHOC_326}, {27, ADHOC_327}, {28, ADHOC_328}, {29, ADHOC_329}, {30, ADHOC_330},
            {31, ADHOC_331}, {32, ADHOC_332}, {33, ADHOC_333}, {34, ADHOC_334}, {35, ADHOC_335},
            {36, ADHOC_336}, {37, ADHOC_337}, {38, ADHOC_338}, {39, ADHOC_339}, {40, ADHOC_340},
            {41, ADHOC_341}, {42, ADHOC_342}, {43, ADHOC_343}, {44, ADHOC_344}, {45, ADHOC_345}
        }

    '===================================================================================
    '  STEP #4
    '===================================================================================

    Public Const BASIC_LOG_MESSAGE_4 As String =
        "#4 Importing and Transforming all Data from RAW data tables to IMDB.dbo Tables"

    '======================================================================================================
    '== #4 01 - INSERT into [IMDB].[dbo].[Principals]
    '======================================================================================================
    Public Const ADHOC_401 As String =
        "INSERT  INTO [IMDB].[dbo].[Principals]                               " & vbCrLf &
        "        WITH (TABLOCKX, HOLDLOCK)                                    " & vbCrLf &
        "    ( [PrincipalId], [PrimaryName], [BirthYear], [DeathYear] )       " & vbCrLf &
        "SELECT  CAST(SUBSTRING(nb.[NameId], 3, 100) AS INT) AS [PrincipalId] " & vbCrLf &
        "       ,nb.[PrimaryName]                            AS [PrimaryName] " & vbCrLf &
        "       ,DATEFROMPARTS(nb.[BirthYear], 1, 1)         AS [BirthYear]   " & vbCrLf &
        "       ,DATEFROMPARTS(nb.[DeathYear], 12, 31)       AS [DeathYear]   " & vbCrLf &
        "FROM    [IMDB].[Raw].[name.basics.tsv.gz] nb                         " & vbCrLf &
        "WHERE   nb.[PrimaryName] IS NOT NULL;"

    '======================================================================================================
    '== #4 02 - INSERT into [IMDB].[dbo].[Professions]
    '======================================================================================================
    Public Const ADHOC_402 As String =
        "INSERT  INTO [IMDB].[dbo].[Professions]  WITH (TABLOCKX, HOLDLOCK)                                            " & vbCrLf &
        "      ( [ProfessionId], [Profession] )                                                                        " & vbCrLf &
        "SELECT  DISTINCT                                                                                              " & vbCrLf &
        "        (ABS(CHECKSUM(p.[value]))%10000)                                              AS [ProfessionId]       " & vbCrLf &
        "       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], N'_', N' '), 2, 100)) AS [Profession]         " & vbCrLf &
        "FROM    [IMDB].[Raw].[name.basics.tsv.gz]  AS n                                                               " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(n.[PrimaryProfession],N',')                              AS p                    " & vbCrLf &
        "WHERE   p.[value] != ''                                                                                       " & vbCrLf &
        "UNION                                                                                                         " & vbCrLf &
        "SELECT  DISTINCT                                                                                              " & vbCrLf &
        "        ABS(CHECKSUM(tp.[Category]))%10000                                                  AS [ProfessionId] " & vbCrLf &
        "       ,UPPER(LEFT(tp.[Category], 1))+SUBSTRING(REPLACE(tp.[Category], N'_', N' '), 2, 100) AS [Profession]   " & vbCrLf &
        "FROM    [IMDB].[Raw].[title.principals.tsv.gz] tp                                                             " & vbCrLf &
        "WHERE   tp.[Category] != N'';"

    '======================================================================================================
    '== #4 03 - INSERT into [IMDB].[dbo].[Professions]
    '======================================================================================================
    Public Const ADHOC_403 As String =
        "INSERT INTO [IMDB].[dbo].[Professions] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [ProfessionId], [Profession] )                               " & vbCrLf &
        "SELECT  ABS(CHECKSUM('director'))%10000 AS [ProfessionId]        " & vbCrLf &
        "       ,'Director'                      AS [Profession]          " & vbCrLf &
        "UNION                                                            " & vbCrLf &
        "SELECT  ABS(CHECKSUM('writer'))%10000   AS [ProfessionId]        " & vbCrLf &
        "       ,'Writer'                        AS [Profession];"

    '======================================================================================================
    '== #4 04 - INSERT into [IMDB].[dbo].[PrimaryProfessions]
    '======================================================================================================
    Public Const ADHOC_404 As String =
        "INSERT INTO [IMDB].[dbo].[PrimaryProfessions] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [PrincipalId], [ProfessionId], [Ordinal] )                          " & vbCrLf &
        "SELECT  (CAST(SUBSTRING(nb.[NameId], 3, 100) AS INT)) AS [PrincipalId]  " & vbCrLf &
        "       ,(ABS(CHECKSUM(p.[value]))%10000)              AS [ProfessionId] " & vbCrLf &
        "       ,p.[Ordinal]                                   AS [Ordinal]      " & vbCrLf &
        "FROM    [IMDB].[Raw].[name.basics.tsv.gz]                 AS nb         " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(nb.primaryProfession,N',',1) AS p          " & vbCrLf &
        "WHERE   p.[value] != ''                                                 " & vbCrLf &
        "    AND nb.[PrimaryName] IS NOT NULL;"

    '======================================================================================================
    '== #4 05 - INSERT into [IMDB].[dbo].[Genres]
    '======================================================================================================
    Public Const ADHOC_405 As String =
        "INSERT INTO [IMDB].[dbo].[Genres] WITH (TABLOCKX, HOLDLOCK)                                      " & vbCrLf &
        "  ( [GenreId], [Genre] )                                                                         " & vbCrLf &
        "SELECT  DISTINCT                                                                                 " & vbCrLf &
        "        (ABS(CHECKSUM(p.[value]))%32000)                                            AS [GenreId] " & vbCrLf &
        "       ,(UPPER(LEFT(p.[value], 1))+SUBSTRING(REPLACE(p.[value], '_', ' '), 2, 100)) AS [Genre]   " & vbCrLf &
        "FROM    [IMDB].[Raw].[title.basics.tsv.gz]   AS t                                                " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(t.[Genres],',') AS p                                                " & vbCrLf &
        "WHERE   p.[value] != '';"

    '======================================================================================================
    '== #4 06 - INSERT into [IMDB].[dbo].[TitleTypes]
    '======================================================================================================
    Public Const ADHOC_406 As String =
        "INSERT INTO [IMDB].[dbo].[TitleTypes] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [TitleTypeId], [TitleType] )                                " & vbCrLf &
        "SELECT  DISTINCT                                                " & vbCrLf &
        "        ABS(CHECKSUM([TitleType]))%100 AS [TitleTypeId]         " & vbCrLf &
        "       ,[TitleType]                                             " & vbCrLf &
        "FROM    [IMDB].[Raw].[title.basics.tsv.gz];"

    '======================================================================================================
    '== #4 07 - INSERT into [IMDB].[dbo].[Titles]
    '======================================================================================================
    Public Const ADHOC_407 As String =
        "INSERT  INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK)                            " & vbCrLf &
        "    (   [TitleId], [TitleTypeId], [IsAdult], [StartYear], [EndYear], [Runtime] )        " & vbCrLf &
        "SELECT  CAST(SUBSTRING(tb.[TitleId], 3, 10) AS INT)                    AS [TitleId]     " & vbCrLf &
        "       ,ABS(CHECKSUM(tb.[TitleType]))%100                              AS [TitleTypeId] " & vbCrLf &
        "       ,tb.[IsAdult]                                                   AS [IsAdult]     " & vbCrLf &
        "       ,DATEFROMPARTS(tb.[StartYear], 1, 1)                            AS [StartYear]   " & vbCrLf &
        "       ,DATEFROMPARTS(tb.[EndYear], 12, 31)                            AS [EndYear]     " & vbCrLf &
        "       ,DATEADD(MINUTE, tb.[RuntimeMinutes], CAST('00:00' AS TIME(0))) AS [Runtime]     " & vbCrLf &
        "FROM    [IMDB].[Raw].[title.basics.tsv.gz] tb;"

    '======================================================================================================
    '== #4 08 - INSERT into [IMDB].[dbo].[TitleTypes]
    '======================================================================================================
    Public Const ADHOC_408 As String =
        "INSERT  INTO [IMDB].[dbo].[TitleTypes] " & vbCrLf &
        "    ( [TitleTypeId],[TitleType] )      " & vbCrLf &
        "  VALUES (0,'Unknown');"

    '======================================================================================================
    '== #4 09 - INSERT into [IMDB].[dbo].[Titles]
    '======================================================================================================
    Public Const ADHOC_409 As String =
        "INSERT INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK)          " & vbCrLf &
        "  ( [TitleId], [TitleTypeId], [IsAdult] )                            " & vbCrLf &
        "SELECT  TOP (1) WITH TIES                                            " & vbCrLf &
        "        CAST(SUBSTRING(ta.[TitleId], 3, 10) AS INT) AS [TitleId]     " & vbCrLf &
        "       ,0                                           AS [TitleTypeId] " & vbCrLf &
        "       ,0                                           AS [IsAdult]     " & vbCrLf &
        "FROM    [IMDB].[Raw].[title.akas.tsv.gz] ta                          " & vbCrLf &
        "WHERE   ta.[TitleId] NOT IN                                          " & vbCrLf &
        "        ( SELECT [TitleId] FROM [IMDB].[Raw].[title.basics.tsv.gz] ) " & vbCrLf &
        "ORDER   BY ROW_NUMBER()                                              " & vbCrLf &
        "        OVER                                                         " & vbCrLf &
        "  ( PARTITION BY ta.[TitleId] ORDER BY ta.[IsOriginalTitle] DESC, ta.[Ordering] );"

    '======================================================================================================
    '== #4 10 - INSERT into [IMDB].[dbo].[TitleGenres]
    '======================================================================================================
    Public Const ADHOC_410 As String =
        "INSERT INTO [IMDB].[dbo].[TitleGenres] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [TitleId], [GenreId] )                                       " & vbCrLf &
        "SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]    " & vbCrLf &
        "       ,ABS(CHECKSUM(p.[value]))%32000           AS [GenreId]    " & vbCrLf &
        "FROM    [IMDB].[Raw].[title.basics.tsv.gz]      AS t             " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(t.[Genres], ',')   AS p             " & vbCrLf &
        "WHERE   p.[value] != '';"

    '======================================================================================================
    '== #4 11 - INSERT into [IMDB].[dbo].[TitleNames]
    '======================================================================================================
    Public Const ADHOC_411 As String =
        "INSERT INTO [IMDB].[dbo].[TitleNames] WITH (TABLOCKX, HOLDLOCK)         " & vbCrLf &
        "  ( [TitleId], [Ordinal], [Region], [Language], [IsOriginal], [Title] ) " & vbCrLf &
        "SELECT  CAST(SUBSTRING([TitleId], 3, 10) AS INT)  AS [TitleId]          " & vbCrLf &
        "       ,[Ordering]                                AS [Ordinal]          " & vbCrLf &
        "       ,[Region]                                  AS [Region]           " & vbCrLf &
        "       ,[Language]                                AS [Language]         " & vbCrLf &
        "       ,(CASE WHEN [Ordering] =                                         " & vbCrLf &
        "                   MIN((CASE WHEN ISNULL([IsOriginalTitle], 1) = 1      " & vbCrLf &
        "                             THEN [Ordering] END))                      " & vbCrLf &
        "                     OVER ( PARTITION BY [TitleId] )                    " & vbCrLf &
        "              THEN 1 ELSE 0 END)                  AS [IsOriginal]       " & vbCrLf &
        "       ,[Title]                                   AS [Title]            " & vbCrLf &
        "FROM    [IMDB].[Raw].[title.akas.tsv.gz];"

    '======================================================================================================
    '== #4 12 - INSERT into [IMDB].[dbo].[Attributes]
    '======================================================================================================
    Public Const ADHOC_412 As String =
        "INSERT INTO [IMDB].[dbo].[Attributes] WITH (TABLOCKX, HOLDLOCK)                     " & vbCrLf &
        "  ( [AttributeId], [Class], [Attribute] )                                           " & vbCrLf &
        "SELECT DISTINCT DENSE_RANK() OVER ( ORDER BY (SELECT a.[value]) ) AS [AttributeId], " & vbCrLf &
        "       'Title attribute' AS [Class],                                                " & vbCrLf &
        "       a.[value]         AS [Attribute]                                             " & vbCrLf &
        "FROM [IMDB].[Raw].[title.akas.tsv.gz]                   AS aka                      " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(aka.[Attributes], CHAR(2)) AS a                        " & vbCrLf &
        "WHERE   a.[value] != '';"

    '======================================================================================================
    '== #4 13 - INSERT into [IMDB].[dbo].[TitleNameAttributes]
    '======================================================================================================
    Public Const ADHOC_413 As String =
        "INSERT INTO [IMDB].[dbo].[TitleNameAttributes] WITH (TABLOCKX, HOLDLOCK)  " & vbCrLf &
        "  ( [TitleId], [Ordinal], [AttributeId] )                                 " & vbCrLf &
        "SELECT DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId],    " & vbCrLf &
        "       aka.[Ordering]                                    AS [Ordinal],    " & vbCrLf &
        "       attr.[AttributeId]                                AS [AttributeId] " & vbCrLf &
        "FROM [IMDB].[Raw].[title.akas.tsv.gz]                   AS aka            " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(aka.[Attributes], CHAR(2)) AS a              " & vbCrLf &
        "    INNER JOIN [IMDB].[dbo].[Attributes]                AS attr           " & vbCrLf &
        "        ON  attr.[Class]     = 'Title attribute'                          " & vbCrLf &
        "        AND attr.[Attribute] = a.[value];"

    '======================================================================================================
    '== #4 14 - INSERT into [IMDB].[dbo].[Attributes]
    '======================================================================================================
    Public Const ADHOC_414 As String =
        "INSERT INTO [IMDB].[dbo].[Attributes] WITH (TABLOCKX, HOLDLOCK)      " & vbCrLf &
        "  ( [AttributeId], [Class], [Attribute] )                            " & vbCrLf &
        "SELECT DISTINCT                                                      " & vbCrLf &
        "  ( SELECT MAX([AttributeId]) FROM [IMDB].[dbo].[Attributes] ) +     " & vbCrLf &
        "    DENSE_RANK() OVER (ORDER BY(SELECT a.[value]) ) AS [AttributeId] " & vbCrLf &
        "   ,'Title types'                                   AS [Class]       " & vbCrLf &
        "   ,a.[value]                                       AS [Attribute]   " & vbCrLf &
        "FROM [IMDB].[Raw].[title.akas.tsv.gz]              AS aka            " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(aka.[Types], CHAR(2)) AS a              " & vbCrLf &
        "WHERE a.[value] NOT IN ('imdbDisplay', 'original');"

    '======================================================================================================
    '== #4 15 - INSERT into [IMDB].[dbo].[TitleNameAttributes]
    '======================================================================================================
    Public Const ADHOC_415 As String =
        "INSERT INTO [IMDB].[dbo].[TitleNameAttributes] WITH (TABLOCKX, HOLDLOCK)  " & vbCrLf &
        "  ( [TitleId], [Ordinal], [AttributeId] )                                 " & vbCrLf &
        "SELECT DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]     " & vbCrLf &
        "               ,aka.[Ordering]                           AS [Ordinal]     " & vbCrLf &
        "               ,attr.[AttributeId]                       AS [AttributeId] " & vbCrLf &
        "FROM [IMDB].[Raw].[title.akas.tsv.gz] AS aka                              " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(aka.[Types], CHAR(2)) AS a                   " & vbCrLf &
        "    INNER JOIN [IMDB].[dbo].[Attributes]           AS attr                " & vbCrLf &
        "        ON  attr.[Class]     = 'Title types'                              " & vbCrLf &
        "        AND attr.[Attribute] = a.[value];"

    '======================================================================================================
    '== #4 16 - INSERT into [IMDB].[dbo].[Titles]
    '======================================================================================================
    Public Const ADHOC_416 As String =
        "INSERT INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK)               " & vbCrLf &
        "  ( [TitleId], [TitleTypeId], [IsAdult] )                                 " & vbCrLf &
        "SELECT DISTINCT CAST(SUBSTRING([TitleId], 3, 10) AS INT) AS [TitleId]     " & vbCrLf &
        "               ,0                                        AS [TitleTypeId] " & vbCrLf &
        "               ,0                                        AS [IsAdult]     " & vbCrLf &
        "FROM [IMDB].[Raw].[title.principals.tsv.gz]                               " & vbCrLf &
        "WHERE CAST(SUBSTRING([TitleId], 3, 10) AS INT) NOT IN                     " & vbCrLf &
        "      (SELECT [TitleId] FROM [IMDB].[dbo].[Titles]);"

    '======================================================================================================
    '== #4 17 - INSERT into [IMDB].[dbo].[Principals]
    '======================================================================================================
    Public Const ADHOC_417 As String =
        "INSERT INTO [IMDB].[dbo].[Principals] WITH (TABLOCKX, HOLDLOCK)          " & vbCrLf &
        "  ( [PrincipalId], [PrimaryName] )                                       " & vbCrLf &
        "SELECT DISTINCT CAST(SUBSTRING([NameId], 3, 10) AS INT) AS [PrincipalId] " & vbCrLf &
        "               ,N'Unknown'                              AS [PrimaryName] " & vbCrLf &
        "FROM [IMDB].[Raw].[title.principals.tsv.gz]                              " & vbCrLf &
        "WHERE CAST(SUBSTRING([NameId], 3, 10) AS INT) NOT IN                     " & vbCrLf &
        "      (SELECT [PrincipalId] FROM [IMDB].[dbo].[Principals]);"

    '======================================================================================================
    '== #4 18 - INSERT into [IMDB].[dbo].[TitlePrincipals]
    '======================================================================================================
    Public Const ADHOC_418 As String =
        "INSERT INTO [IMDB].[dbo].[TitlePrincipals] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [TitleId], [Ordinal], [PrincipalId], [ProfessionId] )            " & vbCrLf &
        "SELECT CAST(SUBSTRING(tp.[TitleId], 3, 10) AS INT) AS [TitleId]      " & vbCrLf &
        "      ,tp.[Ordering]                               AS [Ordinal]      " & vbCrLf &
        "      ,CAST(SUBSTRING(tp.[NameId], 3, 10) AS INT)  AS [PrincipalId]  " & vbCrLf &
        "      ,ABS(CHECKSUM(tp.[Category]))%10000          AS [ProfessionId] " & vbCrLf &
        "FROM [IMDB].[Raw].[title.principals.tsv.gz] tp;"

    '======================================================================================================
    '== #4 19 - UPDATE [IMDB].[dbo].[TitlePrincipals]
    '======================================================================================================
    Public Const ADHOC_419 As String =
        "UPDATE tp                                                                    " & vbCrLf &
        "    SET tp.[KnownForOrdinal] = k.[Ordinal]                                   " & vbCrLf &
        "FROM [IMDB].[Raw].[name.basics.tsv.gz]                   AS n                " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(n.[KnownForTitles], ',', 1) AS k                " & vbCrLf &
        "    INNER JOIN [IMDB].[dbo].[TitlePrincipals]            AS tp               " & vbCrLf &
        "        WITH    (TABLOCKX, HOLDLOCK)                                         " & vbCrLf &
        "          ON    CAST(SUBSTRING(n.[NameId], 3, 10) AS INT) = tp.[PrincipalId] " & vbCrLf &
        "                AND                                                          " & vbCrLf &
        "                CAST(SUBSTRING(k.[value], 3, 10) AS INT)  = tp.[TitleId]     " & vbCrLf &
        "WHERE k.[value] != '';"

    '======================================================================================================
    '== #4 20 - INSERT into [IMDB].[dbo].[TitleCharacters]
    '======================================================================================================
    Public Const ADHOC_420 As String =
        "INSERT INTO [IMDB].[dbo].[TitleCharacters] WITH (TABLOCKX, HOLDLOCK)    " & vbCrLf &
        "  ( [TitleId], [PrincipalId], [Character] )                             " & vbCrLf &
        "SELECT CAST(SUBSTRING(tp.[TitleId], 3, 10) AS INT) AS [TitleId]         " & vbCrLf &
        "      ,CAST(SUBSTRING(tp.[NameId], 3, 10) AS INT)  AS [PrincipalId]     " & vbCrLf &
        "      ,ch.[value]                                  AS [Character]       " & vbCrLf &
        "FROM [IMDB].[Raw].[title.principals.tsv.gz] AS tp                       " & vbCrLf &
        "    CROSS APPLY STRING_SPLIT(REPLACE(REPLACE(                           " & vbCrLf &
        "                SUBSTRING(tp.[Characters], 3, LEN(tp.[Characters])-4)   " & vbCrLf &
        "                , N'"", ""',NCHAR(9)), N'\""', N'""'), NCHAR(9)) AS ch; "

    '======================================================================================================
    '== #4 21 - INSERT into #writers_directors
    '======================================================================================================
    Public Const ADHOC_421 As String =
        "SELECT t.[TitleId], x.[PrincipalId], x.[ProfessionId]                                  " & vbCrLf &
        "INTO #writers_directors                                                                " & vbCrLf &
        "FROM [IMDB].[Raw].[title.crew.tsv.gz] AS tc                                            " & vbCrLf &
        "    CROSS APPLY (VALUES (CAST(SUBSTRING(tc.[TitleId], 3, 10) AS INT))) AS t([TitleId]) " & vbCrLf &
        "    CROSS APPLY (SELECT CAST(SUBSTRING(p.[value], 3, 10) AS INT) AS [PrincipalId]      " & vbCrLf &
        "                       ,ABS(CHECKSUM('director'))%10000          AS [ProfessionId]     " & vbCrLf &
        "                 FROM STRING_SPLIT(tc.[Directors], ',')        AS p                    " & vbCrLf &
        "                 WHERE tc.[Directors] != ''                                            " & vbCrLf &
        "                 UNION                                                                 " & vbCrLf &
        "                 SELECT CAST(SUBSTRING(w.[value], 3, 10) AS INT) AS [PrincipalId]      " & vbCrLf &
        "                       ,ABS(CHECKSUM('writer'))%10000            AS [ProfessionId]     " & vbCrLf &
        "                 FROM STRING_SPLIT(tc.[Writers], ',')          AS w                    " & vbCrLf &
        "                 WHERE tc.[Writers] != '')              AS x                           " & vbCrLf &
        "    LEFT JOIN [IMDB].[dbo].[TitlePrincipals]            AS tp                          " & vbCrLf &
        "                ON  tp.[TitleId]     = CAST(SUBSTRING(tc.[TitleId], 3, 10) AS INT)     " & vbCrLf &
        "                AND tp.[PrincipalId] = x.[PrincipalId]                                 " & vbCrLf &
        "WHERE tp.[TitleId] IS NULL;"

    '======================================================================================================
    '== #4 22 - INSERT into [IMDB].[dbo].[Titles] from #writers_directors
    '======================================================================================================
    Public Const ADHOC_422 As String =
        "INSERT INTO [IMDB].[dbo].[Titles] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [TitleId], [TitleTypeId], [IsAdult] )                   " & vbCrLf &
        "SELECT DISTINCT [TitleId]   AS [TitleId]                    " & vbCrLf &
        "               ,0           AS [TitleTypeId]                " & vbCrLf &
        "               ,0           AS [IsAdult]                    " & vbCrLf &
        "FROM #writers_directors                                     " & vbCrLf &
        "WHERE [TitleId] NOT IN                                      " & vbCrLf &
        "      (SELECT [TitleId] FROM [IMDB].[dbo].[Titles] );"

    '======================================================================================================
    '== #4 23 - INSERT into [IMDB].[dbo].[Principals] from #writers_directors
    '======================================================================================================
    Public Const ADHOC_423 As String =
        "INSERT INTO [IMDB].[dbo].[Principals] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [PrincipalId], [PrimaryName] )                              " & vbCrLf &
        "SELECT DISTINCT [PrincipalId]  AS [PrincipalId]                 " & vbCrLf &
        "               ,N'Unknown'     AS [PrimaryName]                 " & vbCrLf &
        "FROM #writers_directors                                         " & vbCrLf &
        "WHERE [PrincipalId] NOT IN                                      " & vbCrLf &
        "      (SELECT [PrincipalId] FROM [IMDB].[dbo].[Principals]);"

    '======================================================================================================
    '== #4 24 - INSERT into [IMDB].[dbo].[TitlePrincipals] from #writers_directors
    '======================================================================================================
    Public Const ADHOC_424 As String =
        "INSERT INTO [IMDB].[dbo].[TitlePrincipals] WITH (TABLOCKX, HOLDLOCK) " & vbCrLf &
        "  ( [TitleId], [Ordinal], [PrincipalId], [ProfessionId] )            " & vbCrLf &
        "SELECT x.[TitleId]                                AS [TitleId]       " & vbCrLf &
        "      ,ISNULL(o.[Ordinal], 0) + ROW_NUMBER()                         " & vbCrLf &
        "         OVER (PARTITION BY x.[TitleId]                              " & vbCrLf &
        "               ORDER     BY x.[ProfessionId], x.[PrincipalId]        " & vbCrLf &
        "              )                                   AS [Ordinal]       " & vbCrLf &
        "      ,x.[PrincipalId]                            AS [PrincipalId]   " & vbCrLf &
        "      ,x.[ProfessionId]                           AS [ProfessionId]  " & vbCrLf &
        "FROM #writers_directors AS x                                         " & vbCrLf &
        "    LEFT JOIN (SELECT [TitleId]       AS [TitleId]                   " & vbCrLf &
        "                     ,MAX([Ordinal])  AS [Ordinal]                   " & vbCrLf &
        "               FROM   [IMDB].[dbo].[TitlePrincipals]                 " & vbCrLf &
        "               GROUP  BY [TitleId] )  AS o                           " & vbCrLf &
        "              ON x.[TitleId] = o.[TitleId]                           " & vbCrLf &
        "DROP TABLE #writers_directors;"

    '======================================================================================================
    '== #4 25 - INSERT into [IMDB].[dbo].[Episodes]
    '======================================================================================================
    Public Const ADHOC_425 As String =
        "INSERT INTO [IMDB].[dbo].[Episodes] WITH (TABLOCKX, HOLDLOCK)           " & vbCrLf &
        "  ( [ParentId], [EpisodeId], [Season], [Episode] )                      " & vbCrLf &
        "SELECT DISTINCT                                                         " & vbCrLf &
        "       CAST(SUBSTRING(te.[ParentTitleId], 3, 10) AS INT) AS [ParentId]  " & vbCrLf &
        "      ,CAST(SUBSTRING(te.[TitleId], 3, 10) AS INT)       AS [EpisodeId] " & vbCrLf &
        "      ,te.[SeasonNumber]                                 AS [Season]    " & vbCrLf &
        "      ,te.[EpisodeNumber]                                AS [Episode]   " & vbCrLf &
        "FROM [IMDB].[Raw].[title.episode.tsv.gz] te                             " & vbCrLf &
        "    INNER JOIN [IMDB].[dbo].[Titles] t1 ON t1.[TitleId] =               " & vbCrLf &
        "                      CAST(SUBSTRING(te.[ParentTitleId], 3, 10) AS INT) " & vbCrLf &
        "    INNER JOIN [IMDB].[dbo].[Titles] t2 ON t2.[TitleId] =               " & vbCrLf &
        "                      CAST(SUBSTRING(te.[TitleId], 3, 10) AS INT);"

    '======================================================================================================
    '== #4 26 - UPDATE data in [IMDB].[dbo].[Titles] for Votes and Average Ratings
    '======================================================================================================
    Public Const ADHOC_426 As String =
        "UPDATE t                                                   " & vbCrLf &
        "  SET t.[VoteCount]     = r.[NumVotes]                     " & vbCrLf &
        "     ,t.[AverageRating] = r.[AverageRating]                " & vbCrLf &
        "FROM [IMDB].[dbo].[Titles] AS t WITH (TABLOCKX, HOLDLOCK)  " & vbCrLf &
        "    INNER JOIN [IMDB].[Raw].[title.ratings.tsv.gz] AS r    " & vbCrLf &
        "            ON t.[TitleId] =                               " & vbCrLf &
        "               CAST(SUBSTRING(r.[TitleId], 3, 10) AS INT);"



    Public Const ADHOC_COUNT_4_MIN As Integer = 1
    Public Const ADHOC_COUNT_4_MAX As Integer = 26

    ''' <summary>
    ''' Returns a sorted list of the AdHoc SQL statements for Step #4, keyed by their index number.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property AdHoc4List As New SortedList(Of Integer, String) From
        {
            {1, ADHOC_401}, {2, ADHOC_402}, {3, ADHOC_403}, {4, ADHOC_404}, {5, ADHOC_405},
            {6, ADHOC_406}, {7, ADHOC_407}, {8, ADHOC_408}, {9, ADHOC_409}, {10, ADHOC_410},
            {11, ADHOC_411}, {12, ADHOC_412}, {13, ADHOC_413}, {14, ADHOC_414}, {15, ADHOC_415},
            {16, ADHOC_416}, {17, ADHOC_417}, {18, ADHOC_418}, {19, ADHOC_419}, {20, ADHOC_420},
            {21, ADHOC_421}, {22, ADHOC_422}, {23, ADHOC_423}, {24, ADHOC_424}, {25, ADHOC_425},
            {26, ADHOC_426}
        }

    ''' <summary>
    ''' Returns a sorted list of the timeout values for each 
    ''' AdHoc SQL statement for Step #4, keyed by their index number.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property TimeOut4List As New SortedList(Of Integer, Integer) From
        {
            {1, 0},
            {2, 0},
            {3, 0},
            {4, 0},
            {5, 0},
            {6, 0},
            {7, 0},
            {8, 0},
            {9, 0},
            {10, 0},
            {11, 0},
            {12, 0},
            {13, 0},
            {14, 0},
            {15, 0},
            {16, 0},
            {17, 0},
            {18, 0},
            {19, 0},
            {20, 0},
            {21, 0},
            {22, 0},
            {23, 0},
            {24, 0},
            {25, 0},
            {26, 0}
        }

    ''' <summary>
    ''' Returns a sorted list of the approximate row counts for each 
    ''' AdHoc SQL statement for Step #4, keyed by their index number.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property ApproxRows4 As New SortedList(Of Integer, Long) From
        {
            {1, 0},
            {2, 0},
            {3, 0},
            {4, 0},
            {5, 0},
            {6, 0},
            {7, 0},
            {8, 0},
            {9, 0},
            {10, 0},
            {11, 0},
            {12, 0},
            {13, 0},
            {14, 0},
            {15, 0},
            {16, 0},
            {17, 0},
            {18, 0},
            {19, 0},
            {20, 0},
            {21, 0},
            {22, 0},
            {23, 0},
            {24, 0},
            {25, 0},
            {26, 0}
        }

    '===================================================================================
    '  STEP #5
    '===================================================================================

    Public Const BASIC_LOG_MESSAGE_5 As String =
        "#5 Getting Final Table Counts for the IMDB.dbo Db Tables"

    Public Const ADHOC_501 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[Attributes];"
    Public Const ADHOC_502 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[Episodes];"
    Public Const ADHOC_503 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[Genres];"
    Public Const ADHOC_504 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[PrimaryProfessions];"
    Public Const ADHOC_505 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[Principals];"
    Public Const ADHOC_506 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[Professions];"
    Public Const ADHOC_507 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[TitleCharacters];"
    Public Const ADHOC_508 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[TitleGenres];"
    Public Const ADHOC_509 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[TitleNameAttributes];"
    Public Const ADHOC_510 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[TitleNames];"
    Public Const ADHOC_511 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[TitlePrincipals];"
    Public Const ADHOC_512 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[Titles];"
    Public Const ADHOC_513 As String =
                 "SELECT COUNT(*) FROM [IMDB].[dbo].[TitleTypes];"

    Public Const ADHOC_COUNT_5_MIN As AH25 = AH25.Attributes
    Public Const ADHOC_COUNT_5_MAX As AH25 = AH25.TitleTypes

    ''' <summary>
    ''' Returns a sorted list of the AdHoc SQL statements for Step #5, keyed by their index number.
    ''' </summary>
    Public Enum AdHoc2_5TableNameEnum As Integer
        Attributes = 1
        Episodes = 2
        Genres = 3
        PrimaryProfessions = 4
        Principals = 5
        Professions = 6
        TitleCharacters = 7
        TitleGenres = 8
        TitleNameAttributes = 9
        TitleNames = 10
        TitlePrincipals = 11
        Titles = 12
        TitleTypes = 13
    End Enum

    ''' <summary>
    ''' Returns a sorted list of the AdHoc SQL statements for Step #5, keyed by their index number.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property AdHoc5List As New SortedList(Of AH25, String) From
        {
            {AH25.Attributes, ADHOC_501},
            {AH25.Episodes, ADHOC_502},
            {AH25.Genres, ADHOC_503},
            {AH25.PrimaryProfessions, ADHOC_504},
            {AH25.Principals, ADHOC_505},
            {AH25.Professions, ADHOC_506},
            {AH25.TitleCharacters, ADHOC_507},
            {AH25.TitleGenres, ADHOC_508},
            {AH25.TitleNameAttributes, ADHOC_509},
            {AH25.TitleNames, ADHOC_510},
            {AH25.TitlePrincipals, ADHOC_511},
            {AH25.Titles, ADHOC_512},
            {AH25.TitleTypes, ADHOC_513}
        }

    ''' <summary>
    ''' Returns a sorted list of the approximate row counts for each 
    ''' AdHoc SQL statement for Step #5, keyed by their index number.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property ApproxRows5 As New SortedList(Of AH25, Long) From
        {
            {AH25.Attributes, 169},
            {AH25.Episodes, 9743260},
            {AH25.Genres, 28},
            {AH25.PrimaryProfessions, 17185592},
            {AH25.Principals, 15454479},
            {AH25.Professions, 49},
            {AH25.TitleCharacters, 48942647},
            {AH25.TitleGenres, 19644165},
            {AH25.TitleNameAttributes, 630492},
            {AH25.TitleNames, 58185048},
            {AH25.TitlePrincipals, 103668658},
            {AH25.Titles, 12611790},
            {AH25.TitleTypes, 12}
        }

End Class