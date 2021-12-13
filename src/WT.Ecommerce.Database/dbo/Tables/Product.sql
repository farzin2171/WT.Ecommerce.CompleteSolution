CREATE TABLE [dbo].[Product]
(
	[Id]		  UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [CategoryId]  UNIQUEIDENTIFIER NOT NULL, 
    [Name]        NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Price]       MONEY NULL, 
    [IsDeleted] BIT NULL DEFAULT 0,
    CONSTRAINT [FK_Product_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id])
)
