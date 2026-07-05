--**************************************************************************************
--  IMDB - #3-12 - CHECK CONSTRAINT FK_Titles_TitleType.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Titles]
    CHECK CONSTRAINT    [FK_Titles_TitleType];
