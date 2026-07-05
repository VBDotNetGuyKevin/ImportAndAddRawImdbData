--**************************************************************************************
--  IMDB - #1-25 - DROP PK_TitleGenres.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [PK_TitleGenres]
    WITH (ONLINE = OFF);
