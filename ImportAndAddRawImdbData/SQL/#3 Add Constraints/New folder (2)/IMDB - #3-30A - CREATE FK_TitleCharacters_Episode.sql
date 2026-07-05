--**************************************************************************************
--  IMDB - #3-30A - CREATE FK_TitleCharacters_Episode.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Episodes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleCharacters_Episode]
    FOREIGN KEY     (   [EpisodeId]     )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]       );
