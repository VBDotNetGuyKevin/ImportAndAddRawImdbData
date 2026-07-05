--**************************************************************************************
--  IMDB - #1-24 - DROP PK_Professions.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Professions] 
    DROP CONSTRAINT     [PK_Professions]
    WITH (ONLINE = OFF);
