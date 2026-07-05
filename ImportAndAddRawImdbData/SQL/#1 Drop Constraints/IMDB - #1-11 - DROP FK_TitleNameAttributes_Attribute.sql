--**************************************************************************************
--  IMDB - #1-11 - DROP FK_TitleNameAttributes_Attribute.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [FK_TitleNameAttributes_Attribute];
