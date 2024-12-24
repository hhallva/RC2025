USE [Profiki]
GO
/****** Object:  Table [dbo].[AbsenceEvent]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbsenceEvent](
	[AbsenceEventId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[AbsenceType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AbsenceEvent] PRIMARY KEY CLUSTERED 
(
	[AbsenceEventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [text] NULL,
	[ParentDepartmentId] [int] NULL,
	[HeadEmployeeId] [int] NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Birthday] [date] NULL,
	[MobilePhone] [varchar](20) NULL,
	[WorkPhone] [varchar](20) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[DepartamentId] [int] NOT NULL,
	[Position] [nvarchar](100) NOT NULL,
	[DirectManagerId] [int] NULL,
	[Cabinet] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[EventId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Status] [nvarchar](100) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[ResponsibleEmployeeId] [int] NULL,
	[Description] [nchar](10) NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event_Has_Material]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event_Has_Material](
	[EventId] [int] NOT NULL,
	[MaterialId] [int] NOT NULL,
 CONSTRAINT [PK_Event_Has_Material] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[MaterialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventAttendee]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventAttendee](
	[EventAttendeeId] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
 CONSTRAINT [PK_EventAttendee] PRIMARY KEY CLUSTERED 
(
	[EventAttendeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Material]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Material](
	[MaterialId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreateDate] [date] NOT NULL,
	[ConfirmDate] [date] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Category] [nvarchar](100) NULL,
	[Author] [nvarchar](100) NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[MaterialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingEvent]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingEvent](
	[TrainingEventId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](10) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_TrainingEvent] PRIMARY KEY CLUSTERED 
(
	[TrainingEventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkingCalendar]    Script Date: 24.12.2024 15:52:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkingCalendar](
	[Id] [bigint] NOT NULL,
	[ExceptionDate] [date] NOT NULL,
	[IsWorkingDay] [bit] NOT NULL,
 CONSTRAINT [WorkingCalendar_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (1, CAST(N'2024-01-01' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (2, CAST(N'2024-01-02' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (3, CAST(N'2024-01-03' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (4, CAST(N'2024-01-04' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (5, CAST(N'2024-01-05' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (6, CAST(N'2024-01-08' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (7, CAST(N'2024-02-23' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (8, CAST(N'2024-03-08' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (9, CAST(N'2024-04-27' AS Date), 1)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (10, CAST(N'2024-04-29' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (11, CAST(N'2024-04-30' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (12, CAST(N'2024-05-01' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (13, CAST(N'2024-05-09' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (14, CAST(N'2024-05-10' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (15, CAST(N'2024-06-12' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (16, CAST(N'2024-11-02' AS Date), 1)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (17, CAST(N'2024-11-04' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (18, CAST(N'2024-12-28' AS Date), 1)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (19, CAST(N'2024-12-30' AS Date), 0)
GO
INSERT [dbo].[WorkingCalendar] ([Id], [ExceptionDate], [IsWorkingDay]) VALUES (20, CAST(N'2024-12-31' AS Date), 0)
GO
ALTER TABLE [dbo].[AbsenceEvent]  WITH CHECK ADD  CONSTRAINT [FK_AbsenceEvent_Employee] FOREIGN KEY([AbsenceEventId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[AbsenceEvent] CHECK CONSTRAINT [FK_AbsenceEvent_Employee]
GO
ALTER TABLE [dbo].[Department]  WITH NOCHECK ADD  CONSTRAINT [FK_Department_Department] FOREIGN KEY([ParentDepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Department]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Department] FOREIGN KEY([DepartamentId])
REFERENCES [dbo].[Department] ([DepartmentId])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Department]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Employee] FOREIGN KEY([DirectManagerId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Employee]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Employee] FOREIGN KEY([ResponsibleEmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Employee]
GO
ALTER TABLE [dbo].[Event_Has_Material]  WITH CHECK ADD  CONSTRAINT [FK_Event_Has_Material_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventId])
GO
ALTER TABLE [dbo].[Event_Has_Material] CHECK CONSTRAINT [FK_Event_Has_Material_Event]
GO
ALTER TABLE [dbo].[Event_Has_Material]  WITH CHECK ADD  CONSTRAINT [FK_Event_Has_Material_Material] FOREIGN KEY([MaterialId])
REFERENCES [dbo].[Material] ([MaterialId])
GO
ALTER TABLE [dbo].[Event_Has_Material] CHECK CONSTRAINT [FK_Event_Has_Material_Material]
GO
ALTER TABLE [dbo].[EventAttendee]  WITH CHECK ADD  CONSTRAINT [FK_EventAttendee_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[EventAttendee] CHECK CONSTRAINT [FK_EventAttendee_Department]
GO
ALTER TABLE [dbo].[EventAttendee]  WITH CHECK ADD  CONSTRAINT [FK_EventAttendee_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[EventAttendee] CHECK CONSTRAINT [FK_EventAttendee_Employee]
GO
ALTER TABLE [dbo].[EventAttendee]  WITH CHECK ADD  CONSTRAINT [FK_EventAttendee_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventId])
GO
ALTER TABLE [dbo].[EventAttendee] CHECK CONSTRAINT [FK_EventAttendee_Event]
GO
ALTER TABLE [dbo].[TrainingEvent]  WITH CHECK ADD  CONSTRAINT [FK_TrainingEvent_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[TrainingEvent] CHECK CONSTRAINT [FK_TrainingEvent_Employee]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Идентификатор строки' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkingCalendar', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'День-исключение' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkingCalendar', @level2type=N'COLUMN',@level2name=N'ExceptionDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - будний день, но законодательно принят выходным' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkingCalendar', @level2type=N'COLUMN',@level2name=N'IsWorkingDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Список дней исключений в производственном календаре' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkingCalendar'
GO
