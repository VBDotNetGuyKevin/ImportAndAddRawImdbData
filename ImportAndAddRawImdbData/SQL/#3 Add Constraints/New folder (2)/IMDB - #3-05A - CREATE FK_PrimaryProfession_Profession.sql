--**************************************************************************************
--  IMDB - #3-05A - CREATE FK_PrimaryProfession_Profession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_PrimaryProfession_Profession]
    FOREIGN KEY     (   [ProfessionId]  )
    REFERENCES          [IMDB].[dbo].[Professions]
                    (   [ProfessionId]  );
