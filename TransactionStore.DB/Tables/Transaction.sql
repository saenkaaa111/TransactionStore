CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 4) NOT NULL,
	[Type] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)) ON [PRIMARY]