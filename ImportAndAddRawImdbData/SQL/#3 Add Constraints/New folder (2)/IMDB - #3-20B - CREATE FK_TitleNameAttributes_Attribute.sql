--**************************************************************************************
--  IMDB - #3-20B - CREATE FK_TitleNameAttributes_Attribute.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes]
    CHECK CONSTRAINT    [FK_TitleNameAttributes_Attribute];
