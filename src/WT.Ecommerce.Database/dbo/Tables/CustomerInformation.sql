CREATE TABLE [dbo].[CustomerInformation]
(
	[Id]		  UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [FirstName]   NVARCHAR(50) NULL, 
    [LastName]    NVARCHAR(50) NULL, 
    [Email]       NVARCHAR(50) NULL, 
    [PhoneNumber] NVARCHAR(50) NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [UserCode] NVARCHAR(256) NOT NULL,

)
