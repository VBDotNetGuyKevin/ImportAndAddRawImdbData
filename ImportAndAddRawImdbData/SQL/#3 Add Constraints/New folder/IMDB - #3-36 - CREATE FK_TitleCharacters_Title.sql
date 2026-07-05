--**************************************************************************************
--  IMDB - #3-36 - CREATE FK_TitleCharacters_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleCharacters] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );
