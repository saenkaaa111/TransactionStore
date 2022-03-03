CREATE TABLE [dbo].[Transaction](
	[Id] [int] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Type] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)) ON [PRIMARY]
