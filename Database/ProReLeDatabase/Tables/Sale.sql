CREATE TABLE [PRORELE].[Sale]
(
  [Id]            INT IDENTITY(1, 1)  NOT NULL,
  [ProductId]      INT                NOT NULL,
  [ClientId]      INT                 NOT NULL,
  [Amount]        INT                 NOT NULL,
  [InitialPrice]  DECIMAL(8,2)        NOT NULL,
  [Discount]      DECIMAL(8,2)        NOT NULL,
  [FinalPrice]    DECIMAL(8,2)        NOT NULL,
  [Date]          DATETIMEOFFSET      NOT NULL,
  CONSTRAINT PK_SALE PRIMARY KEY (Id),
  CONSTRAINT FK_SALEPRODUCT FOREIGN KEY (ProductId) REFERENCES [PRORELE].[Product](Id),
  CONSTRAINT FK_SALECLIENT FOREIGN KEY (ClientId) REFERENCES [PRORELE].[Client](Id)
)