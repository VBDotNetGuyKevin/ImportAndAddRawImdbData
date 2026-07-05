--**************************************************************************************
--  IMDB - #3-37 - CHECK CONSTRAINT FK_TitleCharacters_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Title];
