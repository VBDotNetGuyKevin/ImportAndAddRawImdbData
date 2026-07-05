--**************************************************************************************
--  IMDB - #3-20 - CHECK CONSTRAINT FK_TitleNames_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNames]
    CHECK CONSTRAINT    [FK_TitleNames_Title];
