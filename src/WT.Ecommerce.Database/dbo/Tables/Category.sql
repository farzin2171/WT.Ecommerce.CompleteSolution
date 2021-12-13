CREATE TABLE [dbo].[Category]
(
	[Id]		 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [Name]       NVARCHAR(50) NOT NULL, 
    [ImageName]  NVARCHAR(128) NULL, 
    [CategoryId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_Category_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id])
)
