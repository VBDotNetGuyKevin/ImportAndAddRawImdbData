--**************************************************************************************
--  IMDB - #3-38 - CREATE FK_TitleCharacters_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   );
