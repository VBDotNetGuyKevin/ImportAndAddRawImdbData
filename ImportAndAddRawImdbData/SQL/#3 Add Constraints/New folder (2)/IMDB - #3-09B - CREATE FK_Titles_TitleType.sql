--**************************************************************************************
--  IMDB - #3-09B - CREATE FK_Titles_TitleType.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Titles]
    CHECK CONSTRAINT    [FK_Titles_TitleType];
