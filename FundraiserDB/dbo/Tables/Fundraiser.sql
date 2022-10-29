CREATE TABLE [dbo].[Fundraiser] (
    [Id]       INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (50) NOT NULL, 
    [Owner] INT NOT NULL, 
    [Goal] MONEY NOT NULL, 
    [DonationTotal] MONEY NOT NULL, 
    [Picture] VARBINARY(MAX) NULL, 
);