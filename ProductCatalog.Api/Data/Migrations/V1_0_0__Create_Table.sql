
/*Cria�o da tabela de categoria*/
CREATE TABLE [dbo].[TB_CATEGORY]
(
	[ID] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT newid(), 
    [TITLE] VARCHAR(100) NOT NULL
);

/*Cria��o da tabela de produtos*/
CREATE TABLE [dbo].[TB_PRODUCT]
(
	[ID] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT newid(), 
    [TITLE] VARCHAR(100) NOT NULL, 
    [DESCRIPTION] TEXT NULL, 
    [PRICE] DECIMAL(18, 2) NOT NULL, 
    [QUANTITY] INT NOT NULL, 
    [IMAGE] TEXT NULL, 
    [CREATE_DATE] DATETIME NOT NULL, 
    [LAST_UPDATE_DATE] DATETIME NOT NULL, 
    [CATEGORY_ID] uniqueidentifier NOT NULL
);

/*Criando relacionamento entre as tabelas*/
ALTER TABLE dbo.TB_PRODUCT WITH NOCHECK ADD CONSTRAINT
	FK_TB_PRODUCT_TB_CATEGORY FOREIGN KEY
	(
		CATEGORY_ID
	) REFERENCES dbo.TB_CATEGORY
	(
		ID
	) 
	ON UPDATE  CASCADE 
	ON DELETE  CASCADE;