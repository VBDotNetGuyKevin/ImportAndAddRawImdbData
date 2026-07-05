USE [IMDB]
GO

ALTER TABLE             [IMDB].[dbo].[Episodes] 
    DROP CONSTRAINT     [PK_Episodes]
    WITH (ONLINE = OFF)
GO
