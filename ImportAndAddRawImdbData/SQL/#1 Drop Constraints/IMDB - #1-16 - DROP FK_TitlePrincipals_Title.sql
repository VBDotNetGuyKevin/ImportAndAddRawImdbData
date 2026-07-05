--**************************************************************************************
--  IMDB - #1-16 - DROP FK_TitlePrincipals_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Title];
