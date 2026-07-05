--**************************************************************************************
--  IMDB - #3-18 - CREATE PK_TitleNames.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNames]
    ADD  CONSTRAINT     [PK_TitleNames]
    PRIMARY KEY 
    CLUSTERED       (   [TitleId] ASC
                       ,[Ordinal] ASC   )
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
