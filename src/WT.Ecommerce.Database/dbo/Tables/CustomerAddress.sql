CREATE TABLE [dbo].[CustomerAddress]
(
	[Id]		 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [Address]    NVARCHAR(MAX) NULL, 
    [City]       NVARCHAR(50) NULL, 
    [Country]    NVARCHAR(50) NULL, 
    [PostalCode] NVARCHAR(50) NULL, 
    [CustomerId] UNIQUEIDENTIFIER NULL,
    [IsDeleted]  BIT NOT NULL DEFAULT 0, 
    [IsActive]   BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_CustomerAddress_CustomerInfo_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CustomerInformation] ([Id])
)
