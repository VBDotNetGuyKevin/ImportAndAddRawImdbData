--**************************************************************************************
--  IMDB - #1-18 - DROP PK_TitleTypes.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleTypes] 
    DROP CONSTRAINT     [PK_TitleTypes]
    WITH (ONLINE = OFF);
