CREATE TABLE [PRORELE].[Client]
(
  [Id]                INT IDENTITY(1, 1)  NOT NULL,
  [Name]              NVARCHAR(50)        NOT NULL,
  [Cpf]               NVARCHAR(11)        NOT NULL,
  [LogicallyExcluded] BIT                 NOT NULL,
  CONSTRAINT PK_CLIENT PRIMARY KEY (Id),
  CONSTRAINT CK_CLIENT CHECK (LEN(CPF) = 11)
);
GO;

CREATE UNIQUE NONCLUSTERED INDEX IU_CLIENT 
ON [PRORELE].[Client](Cpf) 
WHERE LogicallyExcluded = 0;
GO;