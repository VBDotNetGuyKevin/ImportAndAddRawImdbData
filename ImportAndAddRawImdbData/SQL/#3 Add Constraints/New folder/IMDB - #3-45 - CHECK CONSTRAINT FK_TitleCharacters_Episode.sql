--**************************************************************************************
--  IMDB - #3-45 - CHECK CONSTRAINT FK_TitleCharacters_Episode.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Episodes]
    CHECK CONSTRAINT    [FK_TitleCharacters_Episode];
