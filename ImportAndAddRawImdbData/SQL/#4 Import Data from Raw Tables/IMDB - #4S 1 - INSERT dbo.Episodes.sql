
-- ======================================================================================================
-- == #4S 1: Import data into: [IMDB].[dbo].[Episodes]
-- ======================================================================================================

---------------------------------------------------------------------------------------------------------
--  #22 Epidodes:   dbo.[Episodes]
-------------------------------------------------------------------------*----------------------------------------
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
