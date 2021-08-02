CREATE TABLE [dbo].[ProductImage]
(
	[Id]  UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [ProductId] UNIQUEIDENTIFIER NOT NULL, 
    [Name] NVARCHAR(128) NOT NULL, 
	CONSTRAINT [FK_ProductImage_Product_ProductId] FOREIGN KEY ([productId]) REFERENCES [dbo].[Product] ([Id])
)
