CREATE TABLE [dbo].[Order]
(
	[Id]		     UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [RefrenceNumber] NVARCHAR(50) NOT NULL, 
    [CustomerId]     UNIQUEIDENTIFIER NOT NULL, 
    [IssueDate]      DATETIME NOT NULL, 
    [Total] MONEY NOT NULL,
    CONSTRAINT [FK_Order_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CustomerInformation] ([Id])
	
)
