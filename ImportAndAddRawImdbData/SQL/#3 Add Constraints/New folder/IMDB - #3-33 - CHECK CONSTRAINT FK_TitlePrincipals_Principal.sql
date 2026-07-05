--**************************************************************************************
--  IMDB - #3-33 - CHECK CONSTRAINT FK_TitlePrincipals_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals]
    CHECK CONSTRAINT    [FK_TitlePrincipals_Principal];
