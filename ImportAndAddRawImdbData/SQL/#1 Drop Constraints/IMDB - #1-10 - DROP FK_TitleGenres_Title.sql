--**************************************************************************************
--  IMDB - #1-10 - DROP FK_TitleGenres_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    DROP CONSTRAINT     [FK_TitleGenres_Title];
