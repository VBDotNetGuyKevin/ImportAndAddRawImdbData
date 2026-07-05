--**************************************************************************************
--  IMDB - #1-22 - DROP PK_PrimaryProfession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [PK_PrimaryProfession]
    WITH (ONLINE = OFF);
