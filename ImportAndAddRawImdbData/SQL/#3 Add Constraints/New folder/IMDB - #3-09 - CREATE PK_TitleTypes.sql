--**************************************************************************************
--  IMDB - #3-09 - CREATE PK_TitleTypes.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleTypes]
    ADD  CONSTRAINT     [PK_TitleTypes]
    PRIMARY KEY 
    CLUSTERED       (   [TitleTypeId] ASC   )
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
