--**************************************************************************************
--  IMDB - #1-14 - DROP FK_TitlePrincipals_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    DROP CONSTRAINT     [FK_TitlePrincipals_Principal];
