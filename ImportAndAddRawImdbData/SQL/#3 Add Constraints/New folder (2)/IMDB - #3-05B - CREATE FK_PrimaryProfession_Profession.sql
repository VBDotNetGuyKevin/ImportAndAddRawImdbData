--**************************************************************************************
--  IMDB - #3-05B - CREATE FK_PrimaryProfession_Profession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    CHECK CONSTRAINT    [FK_PrimaryProfession_Profession];
