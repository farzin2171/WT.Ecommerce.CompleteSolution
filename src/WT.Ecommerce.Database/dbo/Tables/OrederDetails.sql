CREATE TABLE [dbo].[OrederDetails]
(
	[Id]		 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [OrderId] UNIQUEIDENTIFIER NOT NULL, 
    [ProductId] UNIQUEIDENTIFIER NOT NULL, 
     CONSTRAINT [FK_OrederDetails_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
     CONSTRAINT [FK_OrederDetails_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id])
)
