--**************************************************************************************
--  IMDB - #3-19 - CREATE FK_TitleNames_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNames] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNames_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );
