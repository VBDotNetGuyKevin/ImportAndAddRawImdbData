--**************************************************************************************
--  IMDB - #3-30 - CREATE FK_TitlePrincipals_Title.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitlePrincipals] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitlePrincipals_Title]
    FOREIGN KEY     (   [TitleId]   )
    REFERENCES          [IMDB].[dbo].[Titles]
                    (   [TitleId]   );
