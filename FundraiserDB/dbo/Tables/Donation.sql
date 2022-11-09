CREATE TABLE [dbo].[Donation] (
    [Id]       INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [UserId] INT,
    [Amount] MONEY NOT NULL,
    [FirstName] NVARCHAR (50) NOT NULL,
    [LastName] NVARCHAR (50) NOT NULL,
    
    [PaymentType] NVARCHAR(50) NOT NULL,
    [CreditCardNumber] NVARCHAR(50),
    [CVV] NVARCHAR(3),
    [BankAccountNumber] NVARCHAR(50),

    [AddressCountry] NVARCHAR(50) NOT NULL,
    [AddressState] NVARCHAR(50) NOT NULL,
    [AddressCity] NVARCHAR(50) NOT NULL,
    [AddressStreet1] NVARCHAR(50) NOT NULL,
    [AddressStreet2] NVARCHAR(50),
    [AddressZip] NVARCHAR(50) NOT NULL,

    [FundraiserId] INT NOT NULL,
    [Note] NVARCHAR(MAX) NOT NULL,
    [Date] DATETIME NOT NULL
);
