CREATE TABLE [dbo].[Payment] (
    [Id]       INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [CardType] NVARCHAR(50) NOT NULL,
    [CardNumber] INT NOT NULL, 
    [ExpirationDate] DATE NOT NULL, 
    [SecurityCode] INT NOT NULL, 
    [Street] NVARCHAR(50) NOT NULL, 
    [City] NVARCHAR(30) NOT NULL, 
    [State] NVARCHAR(10) NOT NULL, 
    [Zipcode] NVARCHAR(50) NOT NULL, 
    [BillingName] NVARCHAR(50) NOT NULL 
);