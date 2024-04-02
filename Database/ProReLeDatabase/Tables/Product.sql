CREATE TABLE [PRORELE].[Product]
(
  [Id]                INT IDENTITY(1, 1)  NOT NULL,
  [Description]       NVARCHAR(50)        NOT NULL,
  [Price]             DECIMAL(8,2)        NOT NULL,
  [Amount]            INT                 NOT NULL,
  [LogicallyExcluded] BIT                 NOT NULL,
  CONSTRAINT PK_PRODUCT PRIMARY KEY (Id)
)
