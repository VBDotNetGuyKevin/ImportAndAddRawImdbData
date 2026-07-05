--**************************************************************************************
--  IMDB - #3-28 - CHECK CONSTRAINT FK_TitleNameAttributes_Attribute.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_Attribute];
