--**************************************************************************************
--  IMDB - #3-40 - CREATE IX_TitleCharacters.sql
--**************************************************************************************
USE [IMDB];

CREATE CLUSTERED INDEX  [IX_TitleCharacters]
                    ON  [IMDB].[dbo].[TitleCharacters]
                    (   [TitleId]       ASC
                       ,[PrincipalId]   ASC   )
WITH (  
        PAD_INDEX = OFF, 
        STATISTICS_NORECOMPUTE = OFF, 
        SORT_IN_TEMPDB = OFF, 
        DROP_EXISTING = OFF, 
        ONLINE = OFF, 
        ALLOW_ROW_LOCKS = ON, 
        ALLOW_PAGE_LOCKS = ON, 
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF, 
        DATA_COMPRESSION = PAGE
     )
ON [PRIMARY];
