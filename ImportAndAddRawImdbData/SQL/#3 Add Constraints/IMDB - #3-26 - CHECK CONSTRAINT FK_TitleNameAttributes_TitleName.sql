--**************************************************************************************
--  IMDB - #3-26 - CHECK CONSTRAINT FK_TitleNameAttributes_TitleName.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_TitleName];
