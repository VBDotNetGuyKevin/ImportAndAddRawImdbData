--**************************************************************************************
--  IMDB - #3-06 - CREATE PK_Genres.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Genres]
    ADD  CONSTRAINT     [PK_Genres]
    PRIMARY KEY 
    CLUSTERED       (   [GenreId] ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY];
