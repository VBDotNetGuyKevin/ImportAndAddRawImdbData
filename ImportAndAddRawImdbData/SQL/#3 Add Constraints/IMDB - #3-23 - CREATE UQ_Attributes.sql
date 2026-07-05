--**************************************************************************************
--  IMDB - #3-23 - CREATE UQ_Attributes.sql
--**************************************************************************************
USE [IMDB];

SET ANSI_PADDING ON

ALTER TABLE             [IMDB].[dbo].[Attributes]
    ADD  CONSTRAINT     [UQ_Attributes]
    UNIQUE 
    NONCLUSTERED    (   [Class]     ASC
                       ,[Attribute] ASC     )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
     )
ON [PRIMARY];
