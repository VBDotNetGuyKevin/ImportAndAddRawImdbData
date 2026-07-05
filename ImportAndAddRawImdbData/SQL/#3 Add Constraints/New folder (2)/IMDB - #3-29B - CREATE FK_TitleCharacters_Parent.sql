--**************************************************************************************
--  IMDB - #3-29B - CREATE FK_TitleCharacters_Parent.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Episodes]
    CHECK CONSTRAINT    [FK_TitleCharacters_Parent];
