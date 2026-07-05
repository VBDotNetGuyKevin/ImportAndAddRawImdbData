--**************************************************************************************
--  IMDB - #1-15 - DROP FK_TitlePrincipals_Profession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Profession];
