USE [PROG260FA23]
GO
/****** Object:  Table [dbo].[Character]    Script Date: 10/25/2023 10:24:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Character](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Character] [nchar](20) NOT NULL,
	[StatsID] [int] NOT NULL,
 CONSTRAINT [PK_Character] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CharacterStat]    Script Date: 10/25/2023 10:24:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CharacterStat](
	[StatsID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nchar](20) NOT NULL,
	[MapLocation] [nchar](20) NULL,
	[Original_Character] [bit] NOT NULL,
	[Sword_Fighter] [bit] NULL,
	[Magic_User] [bit] NULL,
 CONSTRAINT [PK_CharacterStat] PRIMARY KEY CLUSTERED 
(
	[StatsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Character]  WITH CHECK ADD  CONSTRAINT [FK_Character_CharacterStat] FOREIGN KEY([ID])
REFERENCES [dbo].[CharacterStat] ([StatsID])
GO
ALTER TABLE [dbo].[Character] CHECK CONSTRAINT [FK_Character_CharacterStat]
GO
