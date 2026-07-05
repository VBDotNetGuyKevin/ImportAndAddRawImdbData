---------------------------------------------------------------------------------
-- Count all dbo tables in IMDB Database
---------------------------------------------------------------------------------
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.TitleTypes]			FROM [IMDB].[dbo].[TitleTypes]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.Titles]				FROM [IMDB].[dbo].[Titles]
SELECT FORMAT(COUNT(*), 'N0') AS  [Count of dbo.TitlePrincipals]	FROM [IMDB].[dbo].[TitlePrincipals]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.TitleNames]          FROM [IMDB].[dbo].[TitleNames]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.TitleNameAttributes] FROM [IMDB].[dbo].[TitleNameAttributes]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.TitleGenres]			FROM [IMDB].[dbo].[TitleGenres]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.TitleCharacters]		FROM [IMDB].[dbo].[TitleCharacters]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.Professions]			FROM [IMDB].[dbo].[Professions]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.Principals]			FROM [IMDB].[dbo].[Principals]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.PrimaryProfessions]	FROM [IMDB].[dbo].[PrimaryProfessions]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.Genres]				FROM [IMDB].[dbo].[Genres]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of dbo.Episodes]			FROM [IMDB].[dbo].[Episodes]
