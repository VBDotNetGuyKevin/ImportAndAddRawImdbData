--**************************************************************************************
--  IMDB - #3-11A - CREATE FK_TitleGenres_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleGenres_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );
