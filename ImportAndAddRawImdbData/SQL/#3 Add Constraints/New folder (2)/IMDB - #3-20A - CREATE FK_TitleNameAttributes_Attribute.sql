--**************************************************************************************
--  IMDB - #3-20A - CREATE FK_TitleNameAttributes_Attribute.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[TitleNameAttributes] 
    WITH CHECK 
    ADD  CONSTRAINT     [FK_TitleNameAttributes_Attribute]
    FOREIGN KEY     (   [AttributeId]   )
    REFERENCES          [IMDB].[dbo].[Attributes] 
                    (   [AttributeId]   );
