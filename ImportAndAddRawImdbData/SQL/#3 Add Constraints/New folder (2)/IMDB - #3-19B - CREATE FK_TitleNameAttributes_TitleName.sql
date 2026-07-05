--**************************************************************************************
--  IMDB - #3-19B - CREATE FK_TitleNameAttributes_TitleName.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_TitleName];
