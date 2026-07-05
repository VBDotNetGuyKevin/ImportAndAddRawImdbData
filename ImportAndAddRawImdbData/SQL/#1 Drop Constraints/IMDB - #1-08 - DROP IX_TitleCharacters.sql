--**************************************************************************************
--  IMDB - #1-08 - DROP IX_TitleCharacters.sql
--**************************************************************************************
USE [IMDB];

DROP INDEX              [IX_TitleCharacters]
    ON                  [IMDB].[dbo].[TitleCharacters]
    WITH (ONLINE = OFF);
