CREATE TABLE [dbo].[Login] (
    [Id]       INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (50) NOT NULL, 
    [SecurityQuestion] NVARCHAR(50) NULL, 
    [SecurityQuestionAnswer] NVARCHAR(50) NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL
);

