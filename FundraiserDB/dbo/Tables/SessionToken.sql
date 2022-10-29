﻿CREATE TABLE [dbo].[SessionToken] (
    [Id] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [UserId] INT FOREIGN KEY REFERENCES Login(Id),
    [SessionId] NVARCHAR(128) NOT NULL,
    [ExpiresOn] DATETIME NOT NULL,
);