--**************************************************************************************
--  IMDB - #1-07 - DROP FK_TitleCharacters_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    DROP CONSTRAINT     [FK_TitleCharacters_Title];
