--**************************************************************************************
--  IMDB - #3C 1 - CREATE PK_PrimaryProfession.sql
--**************************************************************************************
USE [IMDB];

ALTER TABLE             [IMDB].[dbo].[PrimaryProfessions]
    ADD  CONSTRAINT     [PK_PrimaryProfession]
    PRIMARY KEY 
    CLUSTERED       (   [PrincipalId]  ASC
                       ,[ProfessionId] ASC      )
WITH (  
		PAD_INDEX = OFF
	   ,STATISTICS_NORECOMPUTE = OFF
	   ,SORT_IN_TEMPDB = OFF
	   ,IGNORE_DUP_KEY = OFF
	   ,ONLINE = OFF
	   ,ALLOW_ROW_LOCKS = ON
	   ,ALLOW_PAGE_LOCKS = ON
	   ,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
	   ,DATA_COMPRESSION = PAGE
     ) 
ON [PRIMARY];
