--**************************************************************************************
--  IMDB - #3-19A - CREATE FK_TitleNameAttributes_TitleName.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNameAttributes_TitleName]
    FOREIGN KEY     (   [TitleId]
                       ,[Ordinal]   )
    REFERENCES          [IMDB].[dbo].[TitleNames]
                    (   [TitleId]
                       ,[Ordinal]   );
