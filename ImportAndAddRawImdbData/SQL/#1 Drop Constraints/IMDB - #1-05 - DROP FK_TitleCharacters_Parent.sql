USE [IMDB]
GO

ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [FK_TitleCharacters_Parent]
GO
