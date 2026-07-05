--**************************************************************************************
--  IMDB - #3-12A - CREATE FK_TitleGenres_Genre.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleGenres] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleGenres_Genre]
    FOREIGN KEY     (   [GenreId]   )
    REFERENCES          [IMDB].[dbo].[Genres]
                    (   [GenreId]   );
