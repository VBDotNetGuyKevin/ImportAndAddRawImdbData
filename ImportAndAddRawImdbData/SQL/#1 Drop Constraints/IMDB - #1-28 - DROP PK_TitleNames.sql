--**************************************************************************************
--  IMDB - #1-28 - DROP PK_TitleNames.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    DROP CONSTRAINT     [PK_TitleNames]
    WITH (ONLINE = OFF);
