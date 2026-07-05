--**************************************************************************************
--  IMDB - #3-43 - CHECK CONSTRAINT FK_TitleCharacters_Parent.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Episodes]
    CHECK CONSTRAINT    [FK_TitleCharacters_Parent];
