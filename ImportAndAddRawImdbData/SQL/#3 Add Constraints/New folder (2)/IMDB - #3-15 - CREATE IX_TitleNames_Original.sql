--**************************************************************************************
--  IMDB - #3-15 - CREATE IX_TitleNames_Original.sql
--**************************************************************************************
USE [IMDB];

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
