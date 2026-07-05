--**************************************************************************************
--  IMDB - #1-26 - DROP PK_TitleNameAttributes.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [PK_TitleNameAttributes]
    WITH (ONLINE = OFF);
