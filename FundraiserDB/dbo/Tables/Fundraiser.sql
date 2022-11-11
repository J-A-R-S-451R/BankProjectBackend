CREATE TABLE [dbo].[Fundraiser] (
    [Id]       INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL, 
    [Goal] MONEY NOT NULL, 
    [DonationTotal] MONEY NOT NULL, 
    [ImageUrl] NVARCHAR(MAX) NOT NULL, 
);