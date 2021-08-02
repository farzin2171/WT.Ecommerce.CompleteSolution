CREATE TABLE [dbo].[Stock]
(
	[Id]		 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [ProductId] UNIQUEIDENTIFIER NOT NULL, 
    [Qty] INT NOT NULL DEFAULT 0, 
    [Description] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Stock_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
)
