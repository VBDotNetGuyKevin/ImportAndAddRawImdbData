-- ======================================================================================================
-- == #4E 1:  Import data into: [IMDB].[dbo].[TitleTypes]
-- ======================================================================================================
--      Title types:    dbo.[TitleTypes]
-------------------------------------------------------------------------
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
GO

*/
