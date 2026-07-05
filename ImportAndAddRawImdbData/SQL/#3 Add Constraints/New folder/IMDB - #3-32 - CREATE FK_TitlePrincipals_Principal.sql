--**************************************************************************************
--  IMDB - #3-32 - CREATE FK_TitlePrincipals_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   );
