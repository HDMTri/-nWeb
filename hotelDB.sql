USE [master]
GO
/****** Object:  Database [hotelDB]    Script Date: 6/17/2022 1:51:02 PM ******/
CREATE DATABASE [hotelDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'hotelDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\hotelDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'hotelDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\hotelDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [hotelDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [hotelDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [hotelDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [hotelDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [hotelDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [hotelDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [hotelDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [hotelDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [hotelDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [hotelDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [hotelDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [hotelDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [hotelDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [hotelDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [hotelDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [hotelDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [hotelDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [hotelDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [hotelDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [hotelDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [hotelDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [hotelDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [hotelDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [hotelDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [hotelDB] SET RECOVERY FULL 
GO
ALTER DATABASE [hotelDB] SET  MULTI_USER 
GO
ALTER DATABASE [hotelDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [hotelDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [hotelDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [hotelDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [hotelDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [hotelDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'hotelDB', N'ON'
GO
ALTER DATABASE [hotelDB] SET QUERY_STORE = OFF
GO
USE [hotelDB]
GO
/****** Object:  Table [dbo].[DatPhong]    Script Date: 6/17/2022 1:51:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatPhong](
	[MaDatPhong] [int] NOT NULL,
	[TenTaiKhoan] [varchar](20) NULL,
	[MaPhong] [varchar](10) NULL,
	[NgayDat] [date] NULL,
	[NgayDen] [date] NULL,
	[NgayTra] [date] NULL,
	[DichVu] [nvarchar](100) NULL,
	[ThanhTien] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDatPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DichVu]    Script Date: 6/17/2022 1:51:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DichVu](
	[MaDichVu] [varchar](10) NOT NULL,
	[TenDichVu] [nvarchar](20) NULL,
	[GiaDichVu] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDichVu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiPhong]    Script Date: 6/17/2022 1:51:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiPhong](
	[MaLoai] [varchar](10) NOT NULL,
	[TenLoai] [nvarchar](30) NULL,
	[GhiChu] [nvarchar](250) NULL,
	[DuongDanAnh] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phong]    Script Date: 6/17/2022 1:51:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phong](
	[MaPhong] [varchar](10) NOT NULL,
	[TenPhong] [varchar](10) NULL,
	[MaLoai] [varchar](10) NULL,
	[DienTich] [int] NULL,
	[GiaThue] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 6/17/2022 1:51:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[TenTaiKhoan] [varchar](20) NOT NULL,
	[MatKhau] [varchar](20) NULL,
	[HoTen] [nvarchar](30) NULL,
	[SoDienThoai] [varchar](20) NULL,
	[Email] [varchar](35) NULL,
	[LaAdmin] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TenTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DatPhong]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[DatPhong]  WITH CHECK ADD FOREIGN KEY([TenTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([TenTaiKhoan])
GO
ALTER TABLE [dbo].[Phong]  WITH CHECK ADD FOREIGN KEY([MaLoai])
REFERENCES [dbo].[LoaiPhong] ([MaLoai])
GO
USE [master]
GO
ALTER DATABASE [hotelDB] SET  READ_WRITE 
GO
