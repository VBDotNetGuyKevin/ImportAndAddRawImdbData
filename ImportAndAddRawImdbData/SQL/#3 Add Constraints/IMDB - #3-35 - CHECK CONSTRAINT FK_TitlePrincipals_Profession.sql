--**************************************************************************************
--  IMDB - #3-35 - CHECK CONSTRAINT FK_TitlePrincipals_Profession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Profession];
