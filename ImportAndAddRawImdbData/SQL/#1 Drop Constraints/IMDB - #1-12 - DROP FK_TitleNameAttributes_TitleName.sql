--**************************************************************************************
--  IMDB - #1-12 - DROP FK_TitleNameAttributes_TitleName.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    DROP CONSTRAINT     [FK_TitleNameAttributes_TitleName];
