CREATE DATABASE [Scheduler]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Scheduler', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Scheduler.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Scheduler_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Scheduler_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Scheduler] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Scheduler].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Scheduler] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Scheduler] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Scheduler] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Scheduler] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Scheduler] SET ARITHABORT OFF 
GO
ALTER DATABASE [Scheduler] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Scheduler] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Scheduler] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Scheduler] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Scheduler] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Scheduler] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Scheduler] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Scheduler] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Scheduler] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Scheduler] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Scheduler] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Scheduler] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Scheduler] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Scheduler] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Scheduler] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Scheduler] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Scheduler] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Scheduler] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Scheduler] SET  MULTI_USER 
GO
ALTER DATABASE [Scheduler] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Scheduler] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Scheduler] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Scheduler] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JOBDETAIL](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[JOBNAME] [nvarchar](255) NOT NULL,
	[JOBGROUP] [nvarchar](255) NOT NULL,
	[JOBDESCRIPTION] [nvarchar](max) NULL,
	[JOBSCHEDULE] [nvarchar](100) NULL,
	[JOBLASTRUNTIME] [datetime2](7) NULL,
	[JOBNEXTRUNTIME] [datetime2](7) NULL,
	[STATUS_ID] [smallint] NOT NULL,
 CONSTRAINT [PK_JOBDETAIL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JOBSTATUS](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[STATUSNAME] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_STATUS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCHEDULERINSTANCE](
	[ID] [uniqueidentifier] NOT NULL,
	[INSTANCENAME] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_SCHEDULERINSTANCE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCHEDULERINSTANCESETTING](
	[INSTANCE_ID] [uniqueidentifier] NOT NULL,
	[ISIMMEDIATEENGINESTARTENABLED] [bit] NOT NULL,
	[ISJOBSDIRECTORYTRACKINGENABLED] [bit] NOT NULL,
 CONSTRAINT [PK_SCHEDULERINSTANCESETTING] PRIMARY KEY CLUSTERED 
(
	[INSTANCE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCHEDULERSTATUS](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[STATUSNAME] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_STATUSGROUP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[JOBSTATUS] ON 

INSERT [dbo].[JOBSTATUS] ([ID], [STATUSNAME]) VALUES (1, N'Active')
INSERT [dbo].[JOBSTATUS] ([ID], [STATUSNAME]) VALUES (2, N'Inactive')
SET IDENTITY_INSERT [dbo].[JOBSTATUS] OFF
INSERT [dbo].[SCHEDULERINSTANCE] ([ID], [INSTANCENAME]) VALUES (N'5aaf9fd7-5bc6-4c49-a7bd-182966a34d4c', N'Console-DEFAULT1')
INSERT [dbo].[SCHEDULERINSTANCE] ([ID], [INSTANCENAME]) VALUES (N'98c1a030-039d-4ee3-9cca-640b5f47848a', N'WebManagementConsole-DEFAULT')
INSERT [dbo].[SCHEDULERINSTANCESETTING] ([INSTANCE_ID], [ISIMMEDIATEENGINESTARTENABLED], [ISJOBSDIRECTORYTRACKINGENABLED]) VALUES (N'5aaf9fd7-5bc6-4c49-a7bd-182966a34d4c', 0, 1)
INSERT [dbo].[SCHEDULERINSTANCESETTING] ([INSTANCE_ID], [ISIMMEDIATEENGINESTARTENABLED], [ISJOBSDIRECTORYTRACKINGENABLED]) VALUES (N'98c1a030-039d-4ee3-9cca-640b5f47848a', 1, 1)
SET IDENTITY_INSERT [dbo].[SCHEDULERSTATUS] ON 

INSERT [dbo].[SCHEDULERSTATUS] ([ID], [STATUSNAME]) VALUES (1, N'StandBy')
INSERT [dbo].[SCHEDULERSTATUS] ([ID], [STATUSNAME]) VALUES (2, N'Normal')
INSERT [dbo].[SCHEDULERSTATUS] ([ID], [STATUSNAME]) VALUES (3, N'Paused')
INSERT [dbo].[SCHEDULERSTATUS] ([ID], [STATUSNAME]) VALUES (4, N'Terminated')
SET IDENTITY_INSERT [dbo].[SCHEDULERSTATUS] OFF
ALTER TABLE [dbo].[SCHEDULERINSTANCESETTING] ADD  CONSTRAINT [DF_SCHEDULERINSTANCESETTING_ISIMMEDIATEENGINESTARTENABLED]  DEFAULT ((1)) FOR [ISIMMEDIATEENGINESTARTENABLED]
GO
ALTER TABLE [dbo].[SCHEDULERINSTANCESETTING] ADD  CONSTRAINT [DF_SCHEDULERINSTANCESETTING_ISJOBSDIRECTORYTRACKINGENABLED]  DEFAULT ((1)) FOR [ISJOBSDIRECTORYTRACKINGENABLED]
GO
ALTER TABLE [dbo].[JOBDETAIL]  WITH CHECK ADD  CONSTRAINT [FK_JOBDETAIL_STATUS] FOREIGN KEY([STATUS_ID])
REFERENCES [dbo].[JOBSTATUS] ([ID])
GO
ALTER TABLE [dbo].[JOBDETAIL] CHECK CONSTRAINT [FK_JOBDETAIL_STATUS]
GO
ALTER TABLE [dbo].[SCHEDULERINSTANCESETTING]  WITH CHECK ADD  CONSTRAINT [FK_SCHEDULERINSTANCESETTING_SCHEDULERINSTANCE] FOREIGN KEY([INSTANCE_ID])
REFERENCES [dbo].[SCHEDULERINSTANCE] ([ID])
GO
ALTER TABLE [dbo].[SCHEDULERINSTANCESETTING] CHECK CONSTRAINT [FK_SCHEDULERINSTANCESETTING_SCHEDULERINSTANCE]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GETSCHEDULERINSTANCEDETAILS]
	@ID uniqueidentifier
AS
BEGIN
	SELECT INSTANCE.ID, INSTANCE.INSTANCENAME, SETTINGS.ISIMMEDIATEENGINESTARTENABLED, SETTINGS.ISJOBSDIRECTORYTRACKINGENABLED 
	FROM dbo.SCHEDULERINSTANCE AS INSTANCE
		LEFT JOIN dbo.SCHEDULERINSTANCESETTING AS SETTINGS on SETTINGS.INSTANCE_ID = INSTANCE.ID 
	WHERE Id = @ID
END
GO
ALTER DATABASE [Scheduler] SET  READ_WRITE 
GO
