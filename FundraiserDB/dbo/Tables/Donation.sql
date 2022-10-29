CREATE TABLE [dbo].[Fundraiser] (
    [Id]       INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [Amount] MONEY NOT NULL,
    [Name] NVARCHAR (50) NOT NULL, 
    [FundraiserId] INT NOT NULL, 
    [Note] NVARCHAR(MAX) NOT NULL 
);
