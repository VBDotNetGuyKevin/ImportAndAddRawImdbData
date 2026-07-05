--**************************************************************************************
--  IMDB - #1-06 - DROP FK_TitleCharacters_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    DROP CONSTRAINT     [FK_TitleCharacters_Principal];
