--**************************************************************************************
--  IMDB - #1-19 - DROP PK_Attributes.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Attributes] 
    DROP CONSTRAINT     [PK_Attributes]
    WITH (ONLINE = OFF);
