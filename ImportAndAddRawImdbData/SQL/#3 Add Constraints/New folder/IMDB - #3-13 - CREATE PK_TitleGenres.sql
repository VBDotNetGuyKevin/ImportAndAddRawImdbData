--**************************************************************************************
--  IMDB - #3-13 - CREATE PK_TitleGenres.sql
--**************************************************************************************
USE [IMDB];

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
