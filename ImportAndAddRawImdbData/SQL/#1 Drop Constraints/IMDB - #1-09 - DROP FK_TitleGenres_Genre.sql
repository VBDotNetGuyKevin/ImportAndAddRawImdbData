--**************************************************************************************
--  IMDB - #1-09 - DROP FK_TitleGenres_Genre.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [FK_TitleGenres_Genre];
