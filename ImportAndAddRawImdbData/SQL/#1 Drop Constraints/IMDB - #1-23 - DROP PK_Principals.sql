--**************************************************************************************
--  IMDB - #1-23 - DROP PK_Principals.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Principals] 
    DROP CONSTRAINT     [PK_Principals]
    WITH (ONLINE = OFF);
