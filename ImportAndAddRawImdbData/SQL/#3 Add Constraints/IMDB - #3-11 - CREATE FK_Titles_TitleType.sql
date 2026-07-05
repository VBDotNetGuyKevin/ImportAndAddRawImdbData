--**************************************************************************************
--  IMDB - #3-11 - CREATE FK_Titles_TitleType.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[Titles] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_Titles_TitleType]
    FOREIGN KEY     (   [TitleTypeId]   )
    REFERENCES          [IMDB].[dbo].[TitleTypes]
                    (   [TitleTypeId]   );
