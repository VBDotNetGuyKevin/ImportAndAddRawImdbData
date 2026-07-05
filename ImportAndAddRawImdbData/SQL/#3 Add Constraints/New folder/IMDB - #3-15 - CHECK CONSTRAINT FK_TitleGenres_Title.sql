--**************************************************************************************
--  IMDB - #3-15 - CHECK CONSTRAINT FK_TitleGenres_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    CHECK CONSTRAINT    [FK_TitleGenres_Title];
