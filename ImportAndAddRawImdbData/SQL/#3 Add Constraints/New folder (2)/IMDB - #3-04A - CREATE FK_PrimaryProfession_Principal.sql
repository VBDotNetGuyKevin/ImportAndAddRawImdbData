--**************************************************************************************
--  IMDB - #3-04A - CREATE FK_PrimaryProfession_Principal.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_PrimaryProfession_Principal]
    FOREIGN KEY     (   [PrincipalId]   )
    REFERENCES          [IMDB].[dbo].[Principals]
                    (   [PrincipalId]   );
