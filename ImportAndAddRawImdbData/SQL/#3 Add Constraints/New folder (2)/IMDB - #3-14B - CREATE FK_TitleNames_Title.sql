--**************************************************************************************
--  IMDB - #3-14B - CREATE FK_TitleNames_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNames]
    CHECK CONSTRAINT    [FK_TitleNames_Title];
