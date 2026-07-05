---------------------------------------------------------------------------------
-- Count all RAW tables in IMDB Database
---------------------------------------------------------------------------------
SELECT FORMAT(COUNT(*), 'N0') AS [Count of Raw.name.basics.tsv.gz]      FROM [IMDB].[Raw].[name.basics.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of Raw.title.akas.tsv.gz]       FROM [IMDB].[Raw].[title.akas.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of Raw.title.basics.tsv.gz]     FROM [IMDB].[Raw].[title.basics.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of Raw.title.crew.tsv.gz]       FROM [IMDB].[Raw].[title.crew.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of Raw.title.episode.tsv.gz]    FROM [IMDB].[Raw].[title.episode.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of Raw.title.principals.tsv.gz] FROM [IMDB].[Raw].[title.principals.tsv.gz]
SELECT FORMAT(COUNT(*), 'N0') AS [Count of Raw.title.ratings.tsv.gz]    FROM [IMDB].[Raw].[title.ratings.tsv.gz]
