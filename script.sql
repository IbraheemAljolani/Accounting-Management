USE [master]
GO
/****** Object:  Database [AccountingManagement]    Script Date: 6/22/2023 1:32:50 AM ******/
CREATE DATABASE [AccountingManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AccountingManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AccountingManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AccountingManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AccountingManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AccountingManagement] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AccountingManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AccountingManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AccountingManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AccountingManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AccountingManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AccountingManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [AccountingManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AccountingManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AccountingManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AccountingManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AccountingManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AccountingManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AccountingManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AccountingManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AccountingManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AccountingManagement] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AccountingManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AccountingManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AccountingManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AccountingManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AccountingManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AccountingManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AccountingManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AccountingManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AccountingManagement] SET  MULTI_USER 
GO
ALTER DATABASE [AccountingManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AccountingManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AccountingManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AccountingManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AccountingManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AccountingManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AccountingManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [AccountingManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [AccountingManagement]
GO
/****** Object:  Table [dbo].[Account_Table]    Script Date: 6/22/2023 1:32:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account_Table](
	[Account_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[Server_DateTime] [datetime] NOT NULL,
	[DateTime_UTC] [datetime] NOT NULL,
	[Update_DateTime_UTC] [datetime] NOT NULL,
	[Account_Number] [varchar](7) NOT NULL,
	[Balance] [money] NOT NULL,
	[Currency] [varchar](255) NOT NULL,
	[Status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Login_Table]    Script Date: 6/22/2023 1:32:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login_Table](
	[Login_ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[PasswordHash] [varbinary](max) NOT NULL,
	[PasswordSalt] [varbinary](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[User_ID] [int] NULL,
 CONSTRAINT [PK__Login_Ta__D788686737710919] PRIMARY KEY CLUSTERED 
(
	[Login_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction_Table]    Script Date: 6/22/2023 1:32:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction_Table](
	[Transaction_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[Account_ID] [int] NULL,
	[Amount] [money] NOT NULL,
	[CreditType] [varchar](10) NOT NULL,
	[TransactionStatus] [varchar](10) NOT NULL,
	[Server_DateTime] [datetime] NOT NULL,
	[DateTime_UTC] [datetime] NOT NULL,
	[Update_DateTime_UTC] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Transaction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Table]    Script Date: 6/22/2023 1:32:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Table](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[Server_DateTime] [datetime] NOT NULL,
	[DateTime_UTC] [datetime] NOT NULL,
	[Update_DateTime_UTC] [datetime] NOT NULL,
	[Username] [varchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[First_Name] [nvarchar](255) NOT NULL,
	[Last_Name] [nvarchar](255) NOT NULL,
	[Status] [int] NOT NULL,
	[Gender] [int] NOT NULL,
	[Date_Of_Birth] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account_Table]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[User_Table] ([User_ID])
GO
ALTER TABLE [dbo].[Login_Table]  WITH CHECK ADD  CONSTRAINT [FK__Login_Tab__User___403A8C7D] FOREIGN KEY([User_ID])
REFERENCES [dbo].[User_Table] ([User_ID])
GO
ALTER TABLE [dbo].[Login_Table] CHECK CONSTRAINT [FK__Login_Tab__User___403A8C7D]
GO
ALTER TABLE [dbo].[Transaction_Table]  WITH CHECK ADD FOREIGN KEY([Account_ID])
REFERENCES [dbo].[Account_Table] ([Account_ID])
GO
ALTER TABLE [dbo].[Transaction_Table]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[User_Table] ([User_ID])
GO
USE [master]
GO
ALTER DATABASE [AccountingManagement] SET  READ_WRITE 
GO
