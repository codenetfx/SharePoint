CREATE TABLE [dbo].[tEngagementStaff]
(
	[id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[engagement] BIGINT NOT NULL,
    [engagementwbid] BIGINT NOT NULL, 
    [login] NVARCHAR(255) NOT NULL, 
	[role] NVARCHAR(50) NOT NULL
)
