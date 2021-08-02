CREATE TABLE [dbo].[OrderStatus]
(
	[Id]		 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [OrderId]    UNIQUEIDENTIFIER NOT NULL, 
    [Status]     NVARCHAR(50) NOT NULL, 
    [IssueDate]  DATETIME NOT NULL, 
    CONSTRAINT [FK_OrderStatus_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id])
)
