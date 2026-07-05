--**************************************************************************************
--  IMDB - #3-31 - CHECK CONSTRAINT FK_TitlePrincipals_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Title];
