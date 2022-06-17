USE [CzyDobreDataBase]
GO
ALTER DATABASE [CzyDobreDataBase] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CzyDobreDataBase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CzyDobreDataBase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET ARITHABORT OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CzyDobreDataBase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CzyDobreDataBase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CzyDobreDataBase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CzyDobreDataBase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CzyDobreDataBase] SET  MULTI_USER 
GO
ALTER DATABASE [CzyDobreDataBase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CzyDobreDataBase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CzyDobreDataBase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CzyDobreDataBase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CzyDobreDataBase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CzyDobreDataBase] SET QUERY_STORE = OFF
GO
USE [CzyDobreDataBase]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [CzyDobreDataBase]
GO
/****** Object:  Table [dbo].[AspNetCategories]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetCategories](
	[Id_Category] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NULL,
	[CategoryDescription] [nvarchar](250) NULL,
 CONSTRAINT [PK_AspNetCategories] PRIMARY KEY CLUSTERED 
(
	[Id_Category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetCities]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetCities](
	[Id_City] [int] IDENTITY(1,1) NOT NULL,
	[Id_Voivode] [int] NOT NULL,
	[LocalizationCity] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_AspNetCities] PRIMARY KEY CLUSTERED 
(
	[Id_City] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetCoordinates]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetCoordinates](
	[Id_Localization] [int] IDENTITY(1,1) NOT NULL,
	[LocalizationName] [nvarchar](100) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
 CONSTRAINT [PK_AspNetCoordinates] PRIMARY KEY CLUSTERED 
(
	[Id_Localization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetImages]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetImages](
	[Id_Image] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Id_Product] [int] NULL,
 CONSTRAINT [PK_AspNetImages] PRIMARY KEY CLUSTERED 
(
	[Id_Image] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetIngredients]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetIngredients](
	[Id_Ingredients] [int] NOT NULL,
	[IngredientsName] [varchar](max) NULL,
 CONSTRAINT [PK_AspNetIngredients] PRIMARY KEY CLUSTERED 
(
	[Id_Ingredients] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetPlaces]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetPlaces](
	[Id_Place] [int] IDENTITY(1,1) NOT NULL,
	[PlaceName] [nvarchar](300) NULL,
	[City] [nchar](150) NULL,
	[ZipCode] [nchar](100) NULL,
	[State] [nchar](200) NULL,
	[Country] [nchar](150) NULL,
 CONSTRAINT [PK_AspNetPlaces] PRIMARY KEY CLUSTERED 
(
	[Id_Place] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetProducts]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetProducts](
	[Id_Product] [int] IDENTITY(1,1) NOT NULL,
	[Id_Category] [int] NOT NULL,
	[ProductDescription] [nvarchar](250) NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Opinion_Counter] [int] NOT NULL,
	[AvarageTaste] [int] NULL,
	[AvarageService] [int] NULL,
	[AvarageIngredients] [int] NULL,
	[Who] [nvarchar](128) NOT NULL,
	[UniqName] [nvarchar](250) NOT NULL,
	[Id_Place] [int] NULL,
	[CzyDobre] [bit] NOT NULL,
	[Objections] [nvarchar](300) NULL,
 CONSTRAINT [PK_AspNetProducts] PRIMARY KEY CLUSTERED 
(
	[Id_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRatingPictures]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRatingPictures](
	[Id_Picture] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Id_Rating] [int] NULL,
 CONSTRAINT [PK_AspNetRatingPictures] PRIMARY KEY CLUSTERED 
(
	[Id_Picture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRatings]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRatings](
	[Id_Rating] [int] IDENTITY(1,1) NOT NULL,
	[RateService] [smallint] NOT NULL,
	[RateTaste] [smallint] NOT NULL,
	[RateIngredients] [smallint] NOT NULL,
	[RateComposition] [smallint] NOT NULL,
	[Comment] [nvarchar](250) NULL,
	[CzyDobre] [bit] NOT NULL,
	[RateTotal] [float] NULL,
	[Id_Product] [int] NOT NULL,
	[Who] [nvarchar](128) NULL,
	[Objections] [nvarchar](300) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_AspNetRatings] PRIMARY KEY CLUSTERED 
(
	[Id_Rating] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRestaurantCalendarDays]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRestaurantCalendarDays](
	[Id_RestaurantCalendarDay] [int] NOT NULL,
	[RestaurantCalendarDay] [nvarchar](50) NULL,
 CONSTRAINT [PK_AspNetRestaurantCalendarDays] PRIMARY KEY CLUSTERED 
(
	[Id_RestaurantCalendarDay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRestaurantCalendars]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRestaurantCalendars](
	[Id_RestaurantCalendar] [int] NOT NULL,
	[Id_RestaurantCalendarDay] [int] NULL,
	[RestaurantCalendarHoursOpen] [time](7) NULL,
	[RestaurantCalendarHoursClosed] [time](7) NULL,
	[Id_Restaurant] [int] NULL,
 CONSTRAINT [PK_AspNetRestaurantCalendars] PRIMARY KEY CLUSTERED 
(
	[Id_RestaurantCalendar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRestaurantOpenings]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRestaurantOpenings](
	[Id_RestaurantOpening] [int] NOT NULL,
	[Id_RestaurantCalendar] [int] NULL,
 CONSTRAINT [PK_AspNetRestaurantOpenings] PRIMARY KEY CLUSTERED 
(
	[Id_RestaurantOpening] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRestaurants]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRestaurants](
	[Id_Restaurant] [int] NOT NULL,
	[Id_Localization] [int] NULL,
	[Id_RestaurantOpening] [int] NULL,
	[RestaurantPhoneNumber] [nvarchar](12) NULL,
	[RestaurantEmail] [nvarchar](100) NULL,
	[RestaurantFanpage] [nvarchar](100) NULL,
	[RestaurantImage] [varchar](max) NULL,
	[RestaurantName] [nvarchar](100) NULL,
 CONSTRAINT [PK_AspNetRestaurants] PRIMARY KEY CLUSTERED 
(
	[Id_Restaurant] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRestaurantsTypes]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRestaurantsTypes](
	[Id_RestaurantType] [int] NOT NULL,
	[RestaurantTypeName] [nvarchar](90) NULL,
	[RestaurantTypeDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRestaurantsTypes] PRIMARY KEY CLUSTERED 
(
	[Id_RestaurantType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[AspNetRoles_Id] [nvarchar](128) NOT NULL,
	[AspNetUsers_Id] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[AspNetRoles_Id] ASC,
	[AspNetUsers_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[FirstName] [nvarchar](64) NULL,
	[LastName] [nvarchar](64) NULL,
	[NickName] [nvarchar](255) NOT NULL,
	[LastBanDays] [int] NULL,
	[BanComment] [varchar](max) NULL,
	[WhoGaveBan] [nvarchar](50) NULL,
	[AvatarUrl] [nvarchar](250) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetVoivodes]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetVoivodes](
	[Id_Voivode] [int] IDENTITY(1,1) NOT NULL,
	[VoivodeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AspNetVoivodes] PRIMARY KEY CLUSTERED 
(
	[Id_Voivode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[C__MigrationHistory]    Script Date: 15/06/2022 18:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[C__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_C__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_AspNetCities_AspNetVoivodes]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetCities_AspNetVoivodes] ON [dbo].[AspNetCities]
(
	[Id_Voivode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_AspNetImages_AspNetProducts]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetImages_AspNetProducts] ON [dbo].[AspNetImages]
(
	[Id_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_AspNetProducts_AspNetCategories]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetProducts_AspNetCategories] ON [dbo].[AspNetProducts]
(
	[Id_Category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_AspNetProducts_AspNetPlaces]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetProducts_AspNetPlaces] ON [dbo].[AspNetProducts]
(
	[Id_Place] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_FK_AspNetProducts_AspNetUsers]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetProducts_AspNetUsers] ON [dbo].[AspNetProducts]
(
	[Who] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_AspNetRatingPictures_AspNetRating]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetRatingPictures_AspNetRating] ON [dbo].[AspNetRatingPictures]
(
	[Id_Rating] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_AspNetRating_AspNetProducts]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetRating_AspNetProducts] ON [dbo].[AspNetRatings]
(
	[Id_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_FK_AspNetRating_AspNetUsers]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetRating_AspNetUsers] ON [dbo].[AspNetRatings]
(
	[Who] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_RestaurantCalendar_RestaurantCalendarDay]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_RestaurantCalendar_RestaurantCalendarDay] ON [dbo].[AspNetRestaurantCalendars]
(
	[Id_RestaurantCalendarDay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_RestaurantOpening_RestaurantCalendarDay]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_RestaurantOpening_RestaurantCalendarDay] ON [dbo].[AspNetRestaurantOpenings]
(
	[Id_RestaurantCalendar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Restaurants_RestaurantOpening]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Restaurants_RestaurantOpening] ON [dbo].[AspNetRestaurants]
(
	[Id_RestaurantOpening] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_FK_AspNetUserRoles_AspNetUsers]    Script Date: 15/06/2022 18:37:54 ******/
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserRoles_AspNetUsers] ON [dbo].[AspNetUserRoles]
(
	[AspNetUsers_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetCities]  WITH CHECK ADD  CONSTRAINT [FK_AspNetCities_AspNetVoivodes] FOREIGN KEY([Id_Voivode])
REFERENCES [dbo].[AspNetVoivodes] ([Id_Voivode])
GO
ALTER TABLE [dbo].[AspNetCities] CHECK CONSTRAINT [FK_AspNetCities_AspNetVoivodes]
GO
ALTER TABLE [dbo].[AspNetImages]  WITH CHECK ADD  CONSTRAINT [FK_AspNetImages_AspNetProducts] FOREIGN KEY([Id_Product])
REFERENCES [dbo].[AspNetProducts] ([Id_Product])
GO
ALTER TABLE [dbo].[AspNetImages] CHECK CONSTRAINT [FK_AspNetImages_AspNetProducts]
GO
ALTER TABLE [dbo].[AspNetProducts]  WITH CHECK ADD  CONSTRAINT [FK_AspNetProducts_AspNetCategories] FOREIGN KEY([Id_Category])
REFERENCES [dbo].[AspNetCategories] ([Id_Category])
GO
ALTER TABLE [dbo].[AspNetProducts] CHECK CONSTRAINT [FK_AspNetProducts_AspNetCategories]
GO
ALTER TABLE [dbo].[AspNetProducts]  WITH CHECK ADD  CONSTRAINT [FK_AspNetProducts_AspNetPlaces] FOREIGN KEY([Id_Place])
REFERENCES [dbo].[AspNetPlaces] ([Id_Place])
GO
ALTER TABLE [dbo].[AspNetProducts] CHECK CONSTRAINT [FK_AspNetProducts_AspNetPlaces]
GO
ALTER TABLE [dbo].[AspNetProducts]  WITH CHECK ADD  CONSTRAINT [FK_AspNetProducts_AspNetUsers] FOREIGN KEY([Who])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetProducts] CHECK CONSTRAINT [FK_AspNetProducts_AspNetUsers]
GO
ALTER TABLE [dbo].[AspNetRatingPictures]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRatingPictures_AspNetRating] FOREIGN KEY([Id_Rating])
REFERENCES [dbo].[AspNetRatings] ([Id_Rating])
GO
ALTER TABLE [dbo].[AspNetRatingPictures] CHECK CONSTRAINT [FK_AspNetRatingPictures_AspNetRating]
GO
ALTER TABLE [dbo].[AspNetRatings]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRating_AspNetProducts] FOREIGN KEY([Id_Product])
REFERENCES [dbo].[AspNetProducts] ([Id_Product])
GO
ALTER TABLE [dbo].[AspNetRatings] CHECK CONSTRAINT [FK_AspNetRating_AspNetProducts]
GO
ALTER TABLE [dbo].[AspNetRatings]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRating_AspNetUsers] FOREIGN KEY([Who])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetRatings] CHECK CONSTRAINT [FK_AspNetRating_AspNetUsers]
GO
ALTER TABLE [dbo].[AspNetRestaurantCalendars]  WITH CHECK ADD  CONSTRAINT [FK_RestaurantCalendar_RestaurantCalendarDay] FOREIGN KEY([Id_RestaurantCalendarDay])
REFERENCES [dbo].[AspNetRestaurantCalendarDays] ([Id_RestaurantCalendarDay])
GO
ALTER TABLE [dbo].[AspNetRestaurantCalendars] CHECK CONSTRAINT [FK_RestaurantCalendar_RestaurantCalendarDay]
GO
ALTER TABLE [dbo].[AspNetRestaurantOpenings]  WITH CHECK ADD  CONSTRAINT [FK_RestaurantOpening_RestaurantCalendarDay] FOREIGN KEY([Id_RestaurantCalendar])
REFERENCES [dbo].[AspNetRestaurantCalendarDays] ([Id_RestaurantCalendarDay])
GO
ALTER TABLE [dbo].[AspNetRestaurantOpenings] CHECK CONSTRAINT [FK_RestaurantOpening_RestaurantCalendarDay]
GO
ALTER TABLE [dbo].[AspNetRestaurants]  WITH CHECK ADD  CONSTRAINT [FK_Restaurants_RestaurantOpening] FOREIGN KEY([Id_RestaurantOpening])
REFERENCES [dbo].[AspNetRestaurantOpenings] ([Id_RestaurantOpening])
GO
ALTER TABLE [dbo].[AspNetRestaurants] CHECK CONSTRAINT [FK_Restaurants_RestaurantOpening]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles] FOREIGN KEY([AspNetRoles_Id])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers] FOREIGN KEY([AspNetUsers_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
GO
USE [master]
GO
ALTER DATABASE [CzyDobreDataBase] SET  READ_WRITE 
GO
