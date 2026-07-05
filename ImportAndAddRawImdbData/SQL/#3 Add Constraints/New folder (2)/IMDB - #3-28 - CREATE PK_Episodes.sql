--**************************************************************************************
--  IMDB - #3-28 - CREATE PK_Episodes.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Episodes]
    ADD  CONSTRAINT     [PK_Episodes]
    PRIMARY KEY 
    CLUSTERED       (   [EpisodeId] ASC     )
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
