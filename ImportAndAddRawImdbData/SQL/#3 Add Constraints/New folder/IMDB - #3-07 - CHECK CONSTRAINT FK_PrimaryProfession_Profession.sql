--**************************************************************************************
--  IMDB - #3-07 - CHECK CONSTRAINT FK_PrimaryProfession_Profession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    CHECK CONSTRAINT    [FK_PrimaryProfession_Profession];
