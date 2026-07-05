--**************************************************************************************
--  IMDB - #3-29A - CREATE FK_TitleCharacters_Parent.sql
--**************************************************************************************
USE [IMDB]
GO

ALTER TABLE             [IMDB].[dbo].[Episodes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Parent]
    FOREIGN KEY     (   [ParentId]      )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]       );
