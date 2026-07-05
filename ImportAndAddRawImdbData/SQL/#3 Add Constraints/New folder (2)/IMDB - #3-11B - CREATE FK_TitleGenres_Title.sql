--**************************************************************************************
--  IMDB - #3-11B - CREATE FK_TitleGenres_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    CHECK CONSTRAINT    [FK_TitleGenres_Title];
