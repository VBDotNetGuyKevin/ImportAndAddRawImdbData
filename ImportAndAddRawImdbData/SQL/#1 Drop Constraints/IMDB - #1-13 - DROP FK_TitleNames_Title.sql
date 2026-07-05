--**************************************************************************************
--  IMDB - #1-13 - DROP FK_TitleNames_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    DROP CONSTRAINT     [FK_TitleNames_Title];
