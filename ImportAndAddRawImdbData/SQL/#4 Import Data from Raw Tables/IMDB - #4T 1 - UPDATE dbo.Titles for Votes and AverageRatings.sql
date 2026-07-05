
-- ======================================================================================================
-- == #4T 1: Update data for Votes and AverageRating in: [IMDB].[dbo].[Titles]
-- ======================================================================================================

---------------------------------------------------------------------------------------------------------
--  #23 Votes and average ratings on titles
--      Titles: dbo.[Titles]
---------------------------------------------------------------------------------------------------------
--  Votes and average ratings on
--  titles.
---------------------------------------------------------------------------------------------------------
UPDATE  t
    SET t.[VoteCount]     = r.[NumVotes]
       ,t.[AverageRating] = r.[AverageRating]
FROM    [IMDB].[dbo].[Titles] AS t 
        WITH (TABLOCKX, HOLDLOCK)
    INNER JOIN [IMDB].[Raw].[title.ratings.tsv.gz] AS r ON t.[TitleId] = CAST(SUBSTRING(r.[TitleId], 3, 10) AS INT)
GO

--  1,676,404 Rows
