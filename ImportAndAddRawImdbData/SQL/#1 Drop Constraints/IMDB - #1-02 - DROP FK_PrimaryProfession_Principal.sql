USE [IMDB]
GO

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions] 
    DROP CONSTRAINT     [FK_PrimaryProfession_Principal]
GO
