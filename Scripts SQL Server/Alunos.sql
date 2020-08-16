USE [Escola]
GO

/****** Object:  Table [dbo].[Alunos]    Script Date: 16/08/2020 17:41:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Alunos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nchar](50) NOT NULL,
	[ValorMensalidade] [money] NOT NULL,
	[DataVencimento] [date] NOT NULL,
	[IdProfessor] [int] NULL,
 CONSTRAINT [PK_Alunos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

