CREATE TABLE [dbo].[Login] (
    [Id]       INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (256) NOT NULL,
    [Password] NVARCHAR (256) NOT NULL, 
    [FirstName] NVARCHAR(256) NOT NULL, 
    [LastName] NVARCHAR(256) NOT NULL,

    [AddressCountry] NVARCHAR(256) NULL,
    [AddressState] NVARCHAR(256) NULL,
    [AddressCity] NVARCHAR(256) NULL,
    [AddressStreet1] NVARCHAR(256) NULL,
    [AddressStreet2] NVARCHAR(256),
    [AddressZip] NVARCHAR(256) NULL,
);

