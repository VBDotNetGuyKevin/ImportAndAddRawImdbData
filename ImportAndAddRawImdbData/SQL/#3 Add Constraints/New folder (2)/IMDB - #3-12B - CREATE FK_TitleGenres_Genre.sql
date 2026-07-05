--**************************************************************************************
--  IMDB - #3-12B - CREATE FK_TitleGenres_Genre.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres]
    CHECK CONSTRAINT    [FK_TitleGenres_Genre];
