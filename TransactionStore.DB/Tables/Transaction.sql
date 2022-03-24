CREATE TABLE [dbo].[Transaction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](9, 2) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Date] [datetime2](2) NOT NULL,
	[Currency] [tinyint] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)) ON [PRIMARY]
