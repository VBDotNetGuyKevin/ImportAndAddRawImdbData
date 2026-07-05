--**************************************************************************************
--  IMDB - #3B 1 - CREATE PK_Professions.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Professions]
    ADD  CONSTRAINT     [PK_Professions]
    PRIMARY KEY 
    CLUSTERED       (   [ProfessionId] ASC  )
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
