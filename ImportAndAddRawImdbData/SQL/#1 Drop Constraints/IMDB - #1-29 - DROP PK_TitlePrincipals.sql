--**************************************************************************************
--  IMDB - #1-29 - DROP PK_TitlePrincipals.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [PK_TitlePrincipals]
    WITH (ONLINE = OFF);
