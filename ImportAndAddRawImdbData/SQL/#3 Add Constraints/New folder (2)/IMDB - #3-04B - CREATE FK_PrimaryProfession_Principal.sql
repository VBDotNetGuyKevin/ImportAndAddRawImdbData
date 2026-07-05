--**************************************************************************************
--  IMDB - #3-04B - CREATE FK_PrimaryProfession_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    CHECK CONSTRAINT    [FK_PrimaryProfession_Principal];
