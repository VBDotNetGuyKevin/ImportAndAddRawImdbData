--**************************************************************************************
--  IMDB - #3-34 - CREATE FK_TitlePrincipals_Profession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Profession]
    FOREIGN KEY     (   [ProfessionId]  )
    REFERENCES          [IMDB].[dbo].[Professions]
                    (   [ProfessionId]  );
