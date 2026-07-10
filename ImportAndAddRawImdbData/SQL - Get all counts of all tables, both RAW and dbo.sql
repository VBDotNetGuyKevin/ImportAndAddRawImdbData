--SELECT FORMAT(COUNT(*), 'N0') FROM imdb.dbo.attributes




select 'SELECT FORMAT(COUNT(*), ''N0'') FROM [' + TABLE_CATALOG + '].[' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']' FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
order by TABLE_NAME



----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/*

SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Attributes] FROM [IMDB].[dbo].[Attributes]
-----------
--	Attributes	169


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Episodes] FROM [IMDB].[dbo].[Episodes]
-----------
--	[Episodes]	9743260


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Genres] FROM [IMDB].[dbo].[Genres]
-----------
--	[Genres]	28


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: PrimaryProfessions] FROM [IMDB].[dbo].[PrimaryProfessions]
-----------
--	[PrimaryProfessions]	17185592


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Principals] FROM [IMDB].[dbo].[Principals]
-----------
--	[Principals]	15454479


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Professions] FROM [IMDB].[dbo].[Professions]
-----------
--	[Professions]	49


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleCharacters] FROM [IMDB].[dbo].[TitleCharacters]
-----------
--	[TitleCharacters]	48942647


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleGenres] FROM [IMDB].[dbo].[TitleGenres]
-----------
--	[TitleGenres]	19644165


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleNameAttributes] FROM [IMDB].[dbo].[TitleNameAttributes]
-----------
--	[TitleNameAttributes]	630492


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleNames] FROM [IMDB].[dbo].[TitleNames]
-----------
--	[TitleNames]	58185048


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitlePrincipals] FROM [IMDB].[dbo].[TitlePrincipals]
-----------
--	[TitlePrincipals]	103668658


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Titles] FROM [IMDB].[dbo].[Titles]
-----------
--	[Titles]	12611790


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleTypes] FROM [IMDB].[dbo].[TitleTypes]
-----------
--	[TitleTypes]	12


*/

SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: name.basics.tsv.gz]		FROM [imdb].[Raw].[name.basics.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: title.akas.tsv.gz]		FROM [IMDB].[Raw].[title.akas.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: title.basics.tsv.gz]		FROM [IMDB].[Raw].[title.basics.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: title.crew.tsv.gz]		FROM [IMDB].[Raw].[title.crew.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: title.episode.tsv.gz]	FROM [IMDB].[Raw].[title.episode.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: title.principals.tsv.gz]	FROM [IMDB].[Raw].[title.principals.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: title.ratings.tsv.gz]	FROM [IMDB].[Raw].[title.ratings.tsv.gz]



SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Attributes]				FROM [IMDB].[dbo].[Attributes]
-----------
--	Attributes	169


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Episodes]					FROM [IMDB].[dbo].[Episodes]
-----------
--	[Episodes]	9743260


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Genres]					FROM [IMDB].[dbo].[Genres]
-----------
--	[Genres]	28


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: PrimaryProfessions]		FROM [IMDB].[dbo].[PrimaryProfessions]
-----------
--	[PrimaryProfessions]	17185592


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Principals]				FROM [IMDB].[dbo].[Principals]
-----------
--	[Principals]	15454479


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Professions]				FROM [IMDB].[dbo].[Professions]
-----------
--	[Professions]	49


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleCharacters]			FROM [IMDB].[dbo].[TitleCharacters]
-----------
--	[TitleCharacters]	48942647


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleGenres]				FROM [IMDB].[dbo].[TitleGenres]
-----------
--	[TitleGenres]	19644165


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleNameAttributes]		FROM [IMDB].[dbo].[TitleNameAttributes]
-----------
--	[TitleNameAttributes]	630492


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleNames]				FROM [IMDB].[dbo].[TitleNames]
-----------
--	[TitleNames]	58185048


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitlePrincipals]			FROM [IMDB].[dbo].[TitlePrincipals]
-----------
--	[TitlePrincipals]	103668658


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: Titles]					FROM [IMDB].[dbo].[Titles]
-----------
--	[Titles]	12611790


SELECT FORMAT(COUNT(*), 'N0') AS [Total Row Count: TitleTypes]				FROM [IMDB].[dbo].[TitleTypes]
-----------
--	[TitleTypes]	12

/*
Total Row Count: name.basics.tsv.gz
-----------------------------------


Total Row Count: title.akas.tsv.gz
----------------------------------
58,273,183

Total Row Count: title.basics.tsv.gz
------------------------------------
12,621,168

Total Row Count: title.crew.tsv.gz
----------------------------------
12,620,239

Total Row Count: title.episode.tsv.gz
-------------------------------------
9,751,486

Total Row Count: title.principals.tsv.gz
----------------------------------------
100,321,296

Total Row Count: title.ratings.tsv.gz
-------------------------------------
1,691,399

Total Row Count: Attributes
---------------------------
169

Total Row Count: Episodes
-------------------------
9,751,469

Total Row Count: Genres
-----------------------
28

Total Row Count: PrimaryProfessions
-----------------------------------
17,197,527

Total Row Count: Principals
---------------------------
15,467,201

Total Row Count: Professions
----------------------------
49

Total Row Count: TitleCharacters
--------------------------------
48,972,350

Total Row Count: TitleGenres
----------------------------
19,664,712

Total Row Count: TitleNameAttributes
------------------------------------
631,035

Total Row Count: TitleNames
---------------------------
58,273,183

Total Row Count: TitlePrincipals
--------------------------------
103,737,908

Total Row Count: Titles
-----------------------
12,621,534

Total Row Count: TitleTypes
---------------------------
12

*/