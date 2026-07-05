--**************************************************************************************
--  IMDB - #3-25B - CREATE FK_TitleCharacters_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Title];
