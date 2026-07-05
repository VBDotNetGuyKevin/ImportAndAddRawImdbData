--**************************************************************************************
--  IMDB - #3-39 - CHECK CONSTRAINT FK_TitleCharacters_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Principal];
