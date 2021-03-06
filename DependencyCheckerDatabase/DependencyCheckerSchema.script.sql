
USE [DependencyChecker]
GO
/****** Object:  Table [dbo].[Dependency]    Script Date: 4/28/2020 12:33:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dependency](
	[DependencyId] [bigint] IDENTITY(1,1) NOT NULL,
	[DependencyName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Dependency] PRIMARY KEY CLUSTERED 
(
	[DependencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[File]    Script Date: 4/28/2020 12:33:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[File](
	[FileId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](150) NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileDependency]    Script Date: 4/28/2020 12:33:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileDependency](
	[FileDependencyId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [bigint] NULL,
	[DependencyId] [bigint] NULL,
 CONSTRAINT [PK_FileDependency] PRIMARY KEY CLUSTERED 
(
	[FileDependencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FileDependency]  WITH CHECK ADD  CONSTRAINT [FK_FileDependency_Dependency] FOREIGN KEY([DependencyId])
REFERENCES [dbo].[Dependency] ([DependencyId])
GO
ALTER TABLE [dbo].[FileDependency] CHECK CONSTRAINT [FK_FileDependency_Dependency]
GO
ALTER TABLE [dbo].[FileDependency]  WITH CHECK ADD  CONSTRAINT [FK_FileDependency_File] FOREIGN KEY([FileId])
REFERENCES [dbo].[File] ([FileId])
GO
ALTER TABLE [dbo].[FileDependency] CHECK CONSTRAINT [FK_FileDependency_File]
GO
USE [master]
GO
ALTER DATABASE [DependencyChecker] SET  READ_WRITE 
GO
