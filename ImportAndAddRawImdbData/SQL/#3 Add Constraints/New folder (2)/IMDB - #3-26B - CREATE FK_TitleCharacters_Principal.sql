--**************************************************************************************
--  IMDB - #3-26B - CREATE FK_TitleCharacters_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters]
    CHECK CONSTRAINT    [FK_TitleCharacters_Principal];
