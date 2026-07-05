--**************************************************************************************
--  IMDB - #1-17 - DROP FK_Titles_TitleType.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Titles] 
    DROP CONSTRAINT     [FK_Titles_TitleType];
