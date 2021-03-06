USE [master]
GO
/****** Object:  Database [StockControl]    Script Date: 5/12/2014 4:34:01 PM ******/
CREATE DATABASE [StockControl]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StockControl', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\StockControl.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'StockControl_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\StockControl_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [StockControl] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StockControl].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StockControl] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StockControl] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StockControl] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StockControl] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StockControl] SET ARITHABORT OFF 
GO
ALTER DATABASE [StockControl] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StockControl] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [StockControl] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StockControl] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StockControl] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StockControl] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StockControl] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StockControl] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StockControl] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StockControl] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StockControl] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StockControl] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StockControl] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StockControl] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StockControl] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StockControl] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StockControl] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StockControl] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StockControl] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [StockControl] SET  MULTI_USER 
GO
ALTER DATABASE [StockControl] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StockControl] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StockControl] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StockControl] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [StockControl]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryOrderDetails]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryOrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryOrderId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[SalesOrderDetailId] [int] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_DeliveryOrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryOrders]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[DeliveryDate] [date] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_DeliveryOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Items]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sku] [varchar](30) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Ready] [int] NOT NULL,
	[PendingDelivery] [int] NOT NULL,
	[PendingReceival] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseOrderDetails]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchaseOrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseOrderId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[Quantity] [int] NOT NULL,
	[PendingReceival] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsFulfilled] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_PurchaseOrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseOrders]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchaseOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[PurchaseDate] [date] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseReceivalDetails]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchaseReceivalDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseReceivalId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[PurchaseOrderDetailId] [int] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_PurchaseReceivalDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseReceivals]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchaseReceivals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[ReceivalDate] [date] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_PurchaseReceivals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesOrderDetails]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalesOrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SalesOrderId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[PendingDelivery] [int] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsFulfilled] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_SalesOrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalesOrders]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalesOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[SalesDate] [datetime] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_SalesOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StockMutations]    Script Date: 5/12/2014 4:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StockMutations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [varchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[MutationCase] [int] NOT NULL,
	[ItemCase] [int] NOT NULL,
	[SourceDocumentId] [int] NOT NULL,
	[SourceDocument] [varchar](50) NOT NULL,
	[SourceDocumentDetailId] [int] NOT NULL,
	[SourceDocumentDetail] [varchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_StockMutations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Unique_ItemId]    Script Date: 5/12/2014 4:34:01 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_ItemId] ON [dbo].[StockMutations]
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [StockControl] SET  READ_WRITE 
GO
