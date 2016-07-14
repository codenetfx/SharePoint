CREATE TABLE [dbo].[tEngagements]
(
	[id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [path] NVARCHAR(255) NOT NULL, 
    [sitecolid] BIGINT NOT NULL, 
    [sitecolguid] UNIQUEIDENTIFIER NOT NULL, 
    [wbnummer] BIGINT NOT NULL, 
    [mandantenname] NVARCHAR(255) NULL, 
    [opportunitynr] BIGINT NULL, 
    [account] NVARCHAR(255) NULL, 
    [concurringpartner] NVARCHAR(255) NULL, 
    [partner] NVARCHAR(255) NULL, 
    [manager] NVARCHAR(255) NULL, 
    [niederlassung] NVARCHAR(255) NULL, 
    [bezeichnung] NVARCHAR(255) NULL, 
    [wbauftragstatus] NVARCHAR(255) NULL, 
    [wbauftragstatusdatum] DATETIME NULL, 
    [status] NVARCHAR(255) NULL, 
    [statusdate] DATETIME NULL,
	[created] DATETIME NULL
)
