
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountBillingItems]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountBillingItems](
	[AccountBillingItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[BillingReason] [int] NOT NULL,
	[BillingAmount] [money] NOT NULL,
	[BillingReference] [varchar](100) NULL,
	[CreatedUtcDateTime] [datetime] NOT NULL,
	[ProcessedUtcDateTime] [datetime] NULL,
	[MessageId] [varchar](50) NULL,
	[PaymentType] [int] NULL,
 CONSTRAINT [PK_AccountBillingItems] PRIMARY KEY CLUSTERED 
(
	[AccountBillingItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountProductCategory]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_AccountProductCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountProducts]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Price] [money] NOT NULL,
	[OnlinePrice] [money] NOT NULL,
	[IsWebPurchasable] [bit] NOT NULL,
	[AccountProductCategoryId] [int] NOT NULL,
	[MaximumLevelQuantity] [int] NOT NULL,
	[MinimumLevelQuantity] [int] NOT NULL,
	[WarnLevelQuantity] [int] NOT NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_AccountProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Timezone] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[Street] [nvarchar](500) NULL,
	[SubUrb] [nvarchar](500) NULL,
	[State] [nvarchar](500) NULL,
	[PostalCode] [nvarchar](500) NULL,
	[Email] [nvarchar](500) NULL,
	[PhoneNumber] [nvarchar](100) NULL,
	[WebsiteUrl] [varchar](500) NULL,
	[FacebookUrl] [varchar](500) NULL,
	[AddressLine1] [varchar](200) NULL,
	[City] [varchar](200) NULL,
	[DialingCode] [nvarchar](5) NULL,
	[BusinessOwnerName] [varchar](100) NULL,
	[BusinessOwnerPhoneNumber] [varchar](100) NULL,
	[BusinessOwnerEmail] [varchar](100) NULL,
	[GeoCoordinates] [varchar](75) NULL,
	[CompanyFeeFailedPaymentCount] [int] NULL,
	[PaymentIssueSuspensionDate] [datetime] NULL,
	[IsSelected] [bit] NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditAccounts]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditAccounts](
	[AuditAccountId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Timezone] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[Street] [nvarchar](500) NULL,
	[SubUrb] [nvarchar](500) NULL,
	[State] [nvarchar](500) NULL,
	[PostalCode] [nvarchar](500) NULL,
	[Email] [nvarchar](500) NULL,
	[PhoneNumber] [nvarchar](100) NULL,
	[WebsiteUrl] [varchar](500) NULL,
	[FacebookUrl] [varchar](500) NULL,
	[AddressLine1] [varchar](200) NULL,
	[City] [varchar](200) NULL,
	[DialingCode] [nvarchar](5) NULL,
	[BusinessOwnerName] [varchar](100) NULL,
	[BusinessOwnerPhoneNumber] [varchar](100) NULL,
	[BusinessOwnerEmail] [varchar](100) NULL,
	[GeoCoordinates] [varchar](75) NULL,
	[CompanyFeeFailedPaymentCount] [int] NULL,
	[PaymentIssueSuspensionDate] [datetime] NULL,
	[AuditDateTime] [datetime2](7) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
	[IsSelected] [bit] NOT NULL,
 CONSTRAINT [PK_AuditAccounts] PRIMARY KEY CLUSTERED 
(
	[AuditAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditRenters]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRenters](
	[AuditRenterId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AdvanceMonths] [int] NOT NULL,
	[AdvancePaidDate] [datetime2](7) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[NoOfPersons] [int] NOT NULL,
	[RoomId] [int] NOT NULL,
	[DateEndRent] [datetime2](7) NULL,
	[IsEndRent] [bit] NOT NULL,
	[MonthsUsed] [int] NOT NULL,
	[BalanceAmount] [decimal](18, 2) NULL,
	[BalancePaidDate] [datetime2](7) NULL,
	[TotalPaidAmount] [decimal](18, 2) NOT NULL,
	[DueDay] [int] NOT NULL,
	[NextDueDate] [datetime2](7) NOT NULL,
	[PreviousDueDate] [datetime2](7) NOT NULL,
	[AuditDateTime] [datetime2](7) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_AuditRenters] PRIMARY KEY CLUSTERED 
(
	[AuditRenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditRentTransactionPayments]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRentTransactionPayments](
	[AuditRentTransactionPaymentId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[DatePaid] [datetime2](7) NULL,
	[PaymentTransactionType] [int] NOT NULL,
	[AuditDateTime] [datetime2](7) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_AuditRentTransactionPayments] PRIMARY KEY CLUSTERED 
(
	[AuditRentTransactionPaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditRentTransactions]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRentTransactions](
	[AuditRentTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[PaidDate] [datetime2](7) NULL,
	[PaidAmount] [decimal](18, 2) NULL,
	[Balance] [decimal](18, 2) NULL,
	[BalanceDateToBePaid] [datetime2](7) NULL,
	[IsDepositUsed] [bit] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[RoomId] [int] NOT NULL,
	[RenterId] [int] NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[Period] [nvarchar](max) NULL,
	[TransactionType] [int] NOT NULL,
	[IsSystemProcessed] [bit] NOT NULL,
	[SystemDateTimeProcessed] [datetime2](7) NULL,
	[TotalAmountDue] [decimal](18, 2) NOT NULL,
	[IsProcessed] [bit] NOT NULL,
	[AuditDateTime] [datetime2](7) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
	[ExcessPaidAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_AuditRentTransactions] PRIMARY KEY CLUSTERED 
(
	[AuditRentTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditRooms]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRooms](
	[AuditRoomId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Frequency] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[AuditDateTime] [datetime2](7) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_AuditRooms] PRIMARY KEY CLUSTERED 
(
	[AuditRoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingTypeImages]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingTypeImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImageName] [nvarchar](100) NOT NULL,
	[IsShow] [bit] NOT NULL,
	[BookingTypeId] [int] NOT NULL,
 CONSTRAINT [PK_BookingTypeImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingTypeInclusions]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingTypeInclusions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingTypeId] [int] NOT NULL,
	[InclusionId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UtcDateTimeCreated] [datetime] NOT NULL,
	[UtcDateTimeUpdated] [datetime] NULL,
 CONSTRAINT [PK_BookingTypeInclusions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookingTypes]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Image] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[UtcDateTimeCreated] [datetime] NOT NULL,
	[UtcDateTimeUpdated] [datetime] NULL,
	[NoOfPersons] [int] NOT NULL,
	[NoOfPersonsMax] [int] NOT NULL,
	[BookingUrl] [nvarchar](100) NULL,
	[AccountProductId] [int] NOT NULL,
 CONSTRAINT [PK_BookingTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CostItems]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CostItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[BackgroundColor] [varchar](20) NULL,
 CONSTRAINT [PK_CostItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbErrorLogs]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbErrorLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessMessage] [nvarchar](max) NULL,
	[Line] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Procedure] [nvarchar](max) NULL,
	[Number] [int] NOT NULL,
	[Severity] [int] NOT NULL,
	[State] [int] NOT NULL,
	[DateTimeCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DbErrorLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuestBookingDates]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuestBookingDates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateBooked] [datetime] NULL,
	[GuestBookingDetailId] [int] NOT NULL,
 CONSTRAINT [PK_GuestBookingDates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuestBookingDetails]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuestBookingDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArrivalDateLocal] [datetime] NULL,
	[DepartureDateLocal] [datetime] NULL,
	[Country] [nvarchar](100) NOT NULL,
	[LanguagesSpoken] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Contact] [nvarchar](15) NOT NULL,
	[ContactPerson] [nvarchar](100) NULL,
	[LeaveMessage] [nvarchar](1000) NULL,
	[CreatedDateTimeUtc] [datetime] NULL,
	[BookingType] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	CheckedInDateTime DATETIME NULL,
	CheckedInBy nvarchar(256),
	CheckedOutDateTime DATETIME NULL,
	CheckedOutBy nvarchar(256),
 CONSTRAINT [PK_GuestBookingDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuestBookings]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuestBookings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Birthday] [datetime] NULL,
	[Age] [int] NOT NULL,
	[Ages] [int] NOT NULL,
	[GuestBookingDetailId] [int] NOT NULL,
 CONSTRAINT [PK_GuestBookings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LookupTypeItems]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupTypeItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[LookupTypeId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT(0)
 CONSTRAINT [PK_LookupTypeItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LookupTypes]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_LookupTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MobileAppLogs]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MobileAppLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[LogInfo] [nvarchar](max) NULL,
	[UtcCreatedDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MobileAppLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonthlyRentBatch]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonthlyRentBatch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessStartDateTime] [datetime2](7) NOT NULL,
	[ProcesssEndDateTime] [datetime2](7) NULL,
 CONSTRAINT [PK_MonthlyRentBatch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentArrears]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentArrears](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RenterId] [int] NOT NULL,
	[RentTransactionId] [int] NULL,
	[UnpaidAmount] [decimal](18, 2) NOT NULL,
	[IsProcessed] [bit] NOT NULL,
	[Note] [nvarchar](2000) NULL,
	[IsManualEntry] [bit] NOT NULL,
	[ManualEntryDateTimeLocal] [datetime] NULL,
	[ProcessedDateTimeUtc] [datetime] NULL,
 CONSTRAINT [PK_RentArrears] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RenterCommunicationHistory]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RenterCommunicationHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RenterId] [int] NOT NULL,
	[CommunicationUtcDateTime] [datetime] NULL,
	[CommunicationType] [int] NULL,
	[CommunicationText] [nvarchar](max) NULL,
	[CommunicationSentTo] [nvarchar](100) NULL,
	[RequestedBy] [nvarchar](25) NULL,
	[IsSuccessfullySent] [bit] NULL,
	[BatchID] [nvarchar](100) NULL,
	[MessageId] [nvarchar](500) NULL,
	[HasRead] [bit] NOT NULL,
	[AttachmentFileName] [nvarchar](500) NULL,
	[ContentType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Renters]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Renters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AdvanceMonths] [int] NOT NULL,
	[AdvancePaidDate] [datetime2](7) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[NoOfPersons] [int] NOT NULL,
	[RoomId] [int] NOT NULL,
	[DateEndRent] [datetime2](7) NULL,
	[IsEndRent] [bit] NOT NULL,
	[MonthsUsed] [int] NOT NULL,
	[BalanceAmount] [decimal](18, 2) NULL,
	[BalancePaidDate] [datetime2](7) NULL,
	[TotalPaidAmount] [decimal](18, 2) NOT NULL,
	[DueDay] [int] NOT NULL,
	[NextDueDate] [datetime2](7) NOT NULL,
	[PreviousDueDate] [datetime2](7) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[EmailRenterBeforeDueDateEnable] [bit] NOT NULL,
	[Mobile] [varchar](50) NULL,
	[MobileRenterBeforeDueDateEnable] [bit] NOT NULL,
 CONSTRAINT [PK_Renters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentTransactionDetails]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentTransactionDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[RentArrearId] [int] NULL,
 CONSTRAINT [PK_RentTransactionDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentTransactionPayments]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentTransactionPayments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[DatePaid] [datetime2](7) NULL,
	[PaymentTransactionType] [int] NOT NULL,
	[RentTransactionId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RentTransactionPayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentTransactions]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaidDate] [datetime2](7) NULL,
	[PaidAmount] [decimal](18, 2) NULL,
	[Balance] [decimal](18, 2) NULL,
	[BalanceDateToBePaid] [datetime2](7) NULL,
	[Note] [nvarchar](max) NULL,
	[RoomId] [int] NOT NULL,
	[RenterId] [int] NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[Period] [nvarchar](max) NULL,
	[TransactionType] [int] NOT NULL,
	[IsSystemProcessed] [bit] NOT NULL,
	[SystemDateTimeProcessed] [datetime2](7) NULL,
	[TotalAmountDue] [decimal](18, 2) NOT NULL,
	[IsProcessed] [bit] NOT NULL,
	[ExcessPaidAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_RentTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Frequency] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[AccountId] [int] NOT NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[FriendlyName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmsGateway]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmsGateway](
	[SmsGatewayId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[UserName] [nvarchar](500) NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[GatewayUrl] [nvarchar](1000) NULL,
	[DedicatedNumber] [varchar](20) NULL,
 CONSTRAINT [PK_SmsGateway] PRIMARY KEY CLUSTERED 
(
	[SmsGatewayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tools]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tools](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[PowerTool] [bit] NOT NULL,
	[CreatedDateTimeUtc] [datetime] NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDateTimeUtc] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Tools] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToolsInventory]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolsInventory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ToolId] [int] NOT NULL,
	[InventoryDateTimeUtc] [datetime] NULL,
	[Status] [int] NOT NULL,
	[Action] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDateTimeUtc] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedDateTimeUtc] [datetime] NOT NULL,
	[Images] [varchar](1000) NULL,
 CONSTRAINT [PK_ToolsInventory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionCost]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionCost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[CostItemId] [int] NOT NULL,
	[Note] [nvarchar](100) NOT NULL,
	[CostCategoryId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Cost] [money] NOT NULL,
 CONSTRAINT [PK_TransactionCost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IDX_AuditRentTransactionPayments_DatePaid]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IDX_AuditRentTransactionPayments_DatePaid] ON [dbo].[AuditRentTransactionPayments]
(
	[DatePaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LookupTypeItems_LookupTypeId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_LookupTypeItems_LookupTypeId] ON [dbo].[LookupTypeItems]
(
	[LookupTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_RentArrears_IsProcessed]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IDX_RentArrears_IsProcessed] ON [dbo].[RentArrears]
(
	[IsProcessed] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RentArrears_IsManualEntry]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RentArrears_IsManualEntry] ON [dbo].[RentArrears]
(
	[IsManualEntry] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RentArrears_RenterId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RentArrears_RenterId] ON [dbo].[RentArrears]
(
	[RenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RentArrears_RentTransactionId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RentArrears_RentTransactionId] ON [dbo].[RentArrears]
(
	[RentTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_Renters_IsEndRent]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IDX_Renters_IsEndRent] ON [dbo].[Renters]
(
	[IsEndRent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_Renters_Name]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IDX_Renters_Name] ON [dbo].[Renters]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Renters_RoomId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_Renters_RoomId] ON [dbo].[Renters]
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RentTransactionDetails_RentArrearId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RentTransactionDetails_RentArrearId] ON [dbo].[RentTransactionDetails]
(
	[RentArrearId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RentTransactionDetails_TransactionId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RentTransactionDetails_TransactionId] ON [dbo].[RentTransactionDetails]
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_RentTransactionPayments_DatePaid]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IDX_RentTransactionPayments_DatePaid] ON [dbo].[RentTransactionPayments]
(
	[DatePaid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDX_rentTransactions_IsProcessed]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IDX_rentTransactions_IsProcessed] ON [dbo].[RentTransactions]
(
	[IsProcessed] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RentTransactions_RenterId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RentTransactions_RenterId] ON [dbo].[RentTransactions]
(
	[RenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RentTransactions_RoomId]    Script Date: 11/22/2023 7:48:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RentTransactions_RoomId] ON [dbo].[RentTransactions]
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccountProducts] ADD  DEFAULT ((0)) FOR [IsWebPurchasable]
GO
ALTER TABLE [dbo].[AccountProducts] ADD  DEFAULT ((0)) FOR [MaximumLevelQuantity]
GO
ALTER TABLE [dbo].[AccountProducts] ADD  DEFAULT ((0)) FOR [MinimumLevelQuantity]
GO
ALTER TABLE [dbo].[AccountProducts] ADD  DEFAULT ((0)) FOR [WarnLevelQuantity]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [IsSelected]
GO
ALTER TABLE [dbo].[AuditAccounts] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AuditAccounts] ADD  DEFAULT (getdate()) FOR [AuditDateTime]
GO
ALTER TABLE [dbo].[AuditAccounts] ADD  DEFAULT ((0)) FOR [IsSelected]
GO
ALTER TABLE [dbo].[AuditRenters] ADD  DEFAULT ((0)) FOR [IsEndRent]
GO
ALTER TABLE [dbo].[AuditRenters] ADD  DEFAULT ((0)) FOR [MonthsUsed]
GO
ALTER TABLE [dbo].[AuditRenters] ADD  DEFAULT ((0.0)) FOR [TotalPaidAmount]
GO
ALTER TABLE [dbo].[AuditRenters] ADD  DEFAULT ((0)) FOR [DueDay]
GO
ALTER TABLE [dbo].[AuditRenters] ADD  DEFAULT (getdate()) FOR [NextDueDate]
GO
ALTER TABLE [dbo].[AuditRenters] ADD  DEFAULT (getdate()) FOR [PreviousDueDate]
GO
ALTER TABLE [dbo].[AuditRenters] ADD  DEFAULT (getdate()) FOR [AuditDateTime]
GO
ALTER TABLE [dbo].[AuditRentTransactionPayments] ADD  DEFAULT (getdate()) FOR [AuditDateTime]
GO
ALTER TABLE [dbo].[AuditRentTransactionPayments] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[AuditRentTransactions] ADD  DEFAULT (getdate()) FOR [DueDate]
GO
ALTER TABLE [dbo].[AuditRentTransactions] ADD  DEFAULT ((0)) FOR [TransactionType]
GO
ALTER TABLE [dbo].[AuditRentTransactions] ADD  DEFAULT ((0)) FOR [IsSystemProcessed]
GO
ALTER TABLE [dbo].[AuditRentTransactions] ADD  DEFAULT ((0.0)) FOR [TotalAmountDue]
GO
ALTER TABLE [dbo].[AuditRentTransactions] ADD  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[AuditRentTransactions] ADD  DEFAULT (getdate()) FOR [AuditDateTime]
GO
ALTER TABLE [dbo].[AuditRentTransactions] ADD  DEFAULT ((0)) FOR [ExcessPaidAmount]
GO
ALTER TABLE [dbo].[AuditRooms] ADD  DEFAULT (getdate()) FOR [AuditDateTime]
GO
ALTER TABLE [dbo].[BookingTypes] ADD  DEFAULT ((0)) FOR [NoOfPersons]
GO
ALTER TABLE [dbo].[BookingTypes] ADD  DEFAULT ((0)) FOR [NoOfPersonsMax]
GO
ALTER TABLE [dbo].[CostItems] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[DbErrorLogs] ADD  DEFAULT (getdate()) FOR [DateTimeCreated]
GO
ALTER TABLE [dbo].[GuestBookingDetails] ADD  DEFAULT ((0)) FOR [BookingType]
GO
ALTER TABLE [dbo].[GuestBookings] ADD  DEFAULT ((0)) FOR [Age]
GO
ALTER TABLE [dbo].[MobileAppLogs] ADD  DEFAULT (getutcdate()) FOR [UtcCreatedDateTime]
GO
ALTER TABLE [dbo].[MonthlyRentBatch] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [ProcessStartDateTime]
GO
ALTER TABLE [dbo].[RentArrears] ADD  DEFAULT ((0)) FOR [IsManualEntry]
GO
ALTER TABLE [dbo].[Renters] ADD  DEFAULT ((0)) FOR [IsEndRent]
GO
ALTER TABLE [dbo].[Renters] ADD  DEFAULT ((0)) FOR [MonthsUsed]
GO
ALTER TABLE [dbo].[Renters] ADD  DEFAULT ((0.0)) FOR [TotalPaidAmount]
GO
ALTER TABLE [dbo].[Renters] ADD  DEFAULT ((0)) FOR [DueDay]
GO
ALTER TABLE [dbo].[Renters] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [NextDueDate]
GO
ALTER TABLE [dbo].[Renters] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [PreviousDueDate]
GO
ALTER TABLE [dbo].[Renters] ADD  DEFAULT ((0)) FOR [EmailRenterBeforeDueDateEnable]
GO
ALTER TABLE [dbo].[Renters] ADD  CONSTRAINT [DF_Renters_MobileRenterBeforeDueDateEnable]  DEFAULT ((0)) FOR [MobileRenterBeforeDueDateEnable]
GO
ALTER TABLE [dbo].[RentTransactionPayments] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RentTransactions] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [DueDate]
GO
ALTER TABLE [dbo].[RentTransactions] ADD  DEFAULT ((0)) FOR [TransactionType]
GO
ALTER TABLE [dbo].[RentTransactions] ADD  DEFAULT ((0)) FOR [IsSystemProcessed]
GO
ALTER TABLE [dbo].[RentTransactions] ADD  DEFAULT ((0.0)) FOR [TotalAmountDue]
GO
ALTER TABLE [dbo].[RentTransactions] ADD  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[RentTransactions] ADD  DEFAULT ((0)) FOR [ExcessPaidAmount]
GO
ALTER TABLE [dbo].[Tools] ADD  DEFAULT ((0)) FOR [PowerTool]
GO
ALTER TABLE [dbo].[Tools] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ToolsInventory] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ToolsInventory] ADD  DEFAULT (getutcdate()) FOR [CreatedDateTimeUtc]
GO
ALTER TABLE [dbo].[TransactionCost] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[TransactionCost] ADD  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[AccountBillingItems]  WITH CHECK ADD  CONSTRAINT [FK_AccountBillingItems_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
GO
ALTER TABLE [dbo].[AccountBillingItems] CHECK CONSTRAINT [FK_AccountBillingItems_Accounts]
GO
ALTER TABLE [dbo].[AccountProductCategory]  WITH CHECK ADD  CONSTRAINT [FK_AccountProductCategory_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
GO
ALTER TABLE [dbo].[AccountProductCategory] CHECK CONSTRAINT [FK_AccountProductCategory_Accounts]
GO
ALTER TABLE [dbo].[AccountProducts]  WITH CHECK ADD  CONSTRAINT [FK_AccountProducts_AccountProductCategory] FOREIGN KEY([AccountProductCategoryId])
REFERENCES [dbo].[AccountProductCategory] ([Id])
GO
ALTER TABLE [dbo].[AccountProducts] CHECK CONSTRAINT [FK_AccountProducts_AccountProductCategory]
GO
ALTER TABLE [dbo].[AuditAccounts]  WITH CHECK ADD  CONSTRAINT [FK_AuditAccounts_Accounts_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditAccounts] CHECK CONSTRAINT [FK_AuditAccounts_Accounts_Id]
GO
ALTER TABLE [dbo].[AuditRenters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_AuditRenters_Renters] FOREIGN KEY([Id])
REFERENCES [dbo].[Renters] ([Id])
GO
ALTER TABLE [dbo].[AuditRenters] CHECK CONSTRAINT [ForeignKey_AuditRenters_Renters]
GO
ALTER TABLE [dbo].[AuditRenters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_AuditRenters_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditRenters] CHECK CONSTRAINT [ForeignKey_AuditRenters_Room]
GO
ALTER TABLE [dbo].[AuditRentTransactionPayments]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactionPayments_RentTransactions_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[RentTransactionPayments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditRentTransactionPayments] CHECK CONSTRAINT [FK_AuditRentTransactionPayments_RentTransactions_Id]
GO
ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_Renters_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
GO
ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_Renters_RenterId]
GO
ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_RentTransactions_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[RentTransactions] ([Id])
GO
ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_RentTransactions_Id]
GO
ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_Room_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_Room_RoomId]
GO
ALTER TABLE [dbo].[AuditRooms]  WITH CHECK ADD  CONSTRAINT [FK_AuditRooms_Rooms_RoomId] FOREIGN KEY([Id])
REFERENCES [dbo].[Rooms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditRooms] CHECK CONSTRAINT [FK_AuditRooms_Rooms_RoomId]
GO
ALTER TABLE [dbo].[BookingTypeImages]  WITH CHECK ADD  CONSTRAINT [ForeignKey_BookingType_BookingTypeImages_BookingTypeId] FOREIGN KEY([BookingTypeId])
REFERENCES [dbo].[BookingTypes] ([Id])
GO
ALTER TABLE [dbo].[BookingTypeImages] CHECK CONSTRAINT [ForeignKey_BookingType_BookingTypeImages_BookingTypeId]
GO
ALTER TABLE [dbo].[BookingTypeInclusions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_BookingType_BookingTypeInclusions_BookingTypeId] FOREIGN KEY([BookingTypeId])
REFERENCES [dbo].[BookingTypes] ([Id])
GO
ALTER TABLE [dbo].[BookingTypeInclusions] CHECK CONSTRAINT [ForeignKey_BookingType_BookingTypeInclusions_BookingTypeId]
GO
ALTER TABLE [dbo].[BookingTypeInclusions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItem_BookingTypeInclusions_InclusionId] FOREIGN KEY([BookingTypeId])
REFERENCES [dbo].[LookupTypeItems] ([Id])
GO
ALTER TABLE [dbo].[BookingTypeInclusions] CHECK CONSTRAINT [ForeignKey_LookupTypeItem_BookingTypeInclusions_InclusionId]
GO
ALTER TABLE [dbo].[BookingTypes]  WITH CHECK ADD  CONSTRAINT [FK_AccountProduct_BookingTypes_AccountProductId] FOREIGN KEY([AccountProductId])
REFERENCES [dbo].[AccountProducts] ([Id])
GO
ALTER TABLE [dbo].[BookingTypes] CHECK CONSTRAINT [FK_AccountProduct_BookingTypes_AccountProductId]
GO
ALTER TABLE [dbo].[GuestBookingDates]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookings_GuestBookingDates_GuestBookingDetailId] FOREIGN KEY([GuestBookingDetailId])
REFERENCES [dbo].[GuestBookingDetails] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GuestBookingDates] CHECK CONSTRAINT [FK_GuestBookings_GuestBookingDates_GuestBookingDetailId]
GO
ALTER TABLE [dbo].[GuestBookingDetails]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookingDetail_Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GuestBookingDetails] CHECK CONSTRAINT [FK_GuestBookingDetail_Account_AccountId]
GO
ALTER TABLE [dbo].[GuestBookingDetails]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookingDetail_BookingType_Id] FOREIGN KEY([BookingType])
REFERENCES [dbo].[BookingTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GuestBookingDetails] CHECK CONSTRAINT [FK_GuestBookingDetail_BookingType_Id]
GO
ALTER TABLE [dbo].[GuestBookings]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId] FOREIGN KEY([GuestBookingDetailId])
REFERENCES [dbo].[GuestBookingDetails] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GuestBookings] CHECK CONSTRAINT [FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId]
GO
ALTER TABLE [dbo].[GuestBookings]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItems_GuestBookings] FOREIGN KEY([Ages])
REFERENCES [dbo].[LookupTypeItems] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GuestBookings] CHECK CONSTRAINT [ForeignKey_LookupTypeItems_GuestBookings]
GO
ALTER TABLE [dbo].[LookupTypeItems]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItems_LookupTypes] FOREIGN KEY([LookupTypeId])
REFERENCES [dbo].[LookupTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LookupTypeItems] CHECK CONSTRAINT [ForeignKey_LookupTypeItems_LookupTypes]
GO
ALTER TABLE [dbo].[RentArrears]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentArrears_Renter_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentArrears] CHECK CONSTRAINT [ForeignKey_RentArrears_Renter_RenterId]
GO
ALTER TABLE [dbo].[RentArrears]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentArrears_RentTransaction_RentTransactionId] FOREIGN KEY([RentTransactionId])
REFERENCES [dbo].[RentTransactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentArrears] CHECK CONSTRAINT [ForeignKey_RentArrears_RentTransaction_RentTransactionId]
GO
ALTER TABLE [dbo].[RenterCommunicationHistory]  WITH CHECK ADD  CONSTRAINT [FK_RenterCommunicationHistory_Accounts_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RenterCommunicationHistory] CHECK CONSTRAINT [FK_RenterCommunicationHistory_Accounts_RenterId]
GO
ALTER TABLE [dbo].[Renters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_Renter_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Renters] CHECK CONSTRAINT [ForeignKey_Renter_Room]
GO
ALTER TABLE [dbo].[RentTransactionDetails]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentTransactionDetails_RentArrear_RentArrearId] FOREIGN KEY([RentArrearId])
REFERENCES [dbo].[RentArrears] ([Id])
GO
ALTER TABLE [dbo].[RentTransactionDetails] CHECK CONSTRAINT [ForeignKey_RentTransactionDetails_RentArrear_RentArrearId]
GO
ALTER TABLE [dbo].[RentTransactionDetails]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentTransactionDetails_RentTransaction_TransactionId] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[RentTransactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentTransactionDetails] CHECK CONSTRAINT [ForeignKey_RentTransactionDetails_RentTransaction_TransactionId]
GO
ALTER TABLE [dbo].[RentTransactionPayments]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactionPayments_RentTransactions_RentTransactionId] FOREIGN KEY([RentTransactionId])
REFERENCES [dbo].[RentTransactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentTransactionPayments] CHECK CONSTRAINT [FK_RentTransactionPayments_RentTransactions_RentTransactionId]
GO
ALTER TABLE [dbo].[RentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactions_Renters_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
GO
ALTER TABLE [dbo].[RentTransactions] CHECK CONSTRAINT [FK_RentTransactions_Renters_RenterId]
GO
ALTER TABLE [dbo].[RentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactions_Room_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[RentTransactions] CHECK CONSTRAINT [FK_RentTransactions_Room_RoomId]
GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD  CONSTRAINT [FK_Rooms_Accounts_Id] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rooms] CHECK CONSTRAINT [FK_Rooms_Accounts_Id]
GO
ALTER TABLE [dbo].[ToolsInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Action] FOREIGN KEY([Action])
REFERENCES [dbo].[LookupTypeItems] ([Id])
GO
ALTER TABLE [dbo].[ToolsInventory] CHECK CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Action]
GO
ALTER TABLE [dbo].[ToolsInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Status] FOREIGN KEY([Status])
REFERENCES [dbo].[LookupTypeItems] ([Id])
GO
ALTER TABLE [dbo].[ToolsInventory] CHECK CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Status]
GO
ALTER TABLE [dbo].[ToolsInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolsInventory_ToolsInventory_Tools] FOREIGN KEY([ToolId])
REFERENCES [dbo].[Tools] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ToolsInventory] CHECK CONSTRAINT [FK_ToolsInventory_ToolsInventory_Tools]
GO
ALTER TABLE [dbo].[TransactionCost]  WITH CHECK ADD  CONSTRAINT [ForeignKey_TransactionCost_CostItems] FOREIGN KEY([CostItemId])
REFERENCES [dbo].[CostItems] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionCost] CHECK CONSTRAINT [ForeignKey_TransactionCost_CostItems]
GO
ALTER TABLE [dbo].[TransactionCost]  WITH CHECK ADD  CONSTRAINT [ForeignKey_TransactionCost_LookupTypeItems] FOREIGN KEY([CostCategoryId])
REFERENCES [dbo].[LookupTypeItems] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionCost] CHECK CONSTRAINT [ForeignKey_TransactionCost_LookupTypeItems]
GO
/****** Object:  StoredProcedure [dbo].[RentTransactionBatchFile]    Script Date: 11/22/2023 7:48:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RentTransactionBatchFile]
	@CurrentDate	DATETIME
AS

--NOTES
--1. DEBUGGING PURPOSES CHECK IF NEXT DUE DATE ALREADY UPDATED
--2. SELECT 1 / 0 AS Error; --THIS LINE USE TO FORCE ERROR COMMENTED FOR FUTURE USE

IF NOT EXISTS(select 1 from MonthlyRentBatch WHERE CONVERT(VARCHAR(101), ProcessStartDateTime, 11) = CONVERT(VARCHAR(101), @CurrentDate, 11))
BEGIN

	--DECLARE @CurrentDate	DATETIME

	DECLARE @TransactionTypeAsMonthlyRent	INT,
			@DateStart						DATETIME,
			@DateEnd						DATETIME,
			@Period							VARCHAR(100),
			@SystemDateTimeProcessed		DATETIME, 
			@LastDayOfTheMonth				INT,
			@TotalBalance					DECIMAL,
			@PreviousRentArrearId			INT,
			@PreviousRentTransactionId		INT,
			@PreviousTotalBalance			DECIMAL, 
			@TransactionId					INT,
			@TenantGracePeriod				INT

	DECLARE @RenterId		INT,
			@RoomId			INT,
			@DueDate		DATETIME,
			@PaidDate		DATETIME,
			@MonthlyRent	DECIMAL,
			@PaidAmount		DECIMAL,
			@Balance		DECIMAL,
			@TotalAmountDue DECIMAL,
			@IsDeposit		BIT,
			@Note			VARCHAR(100),
			@RentArrearId	INT

	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION
				DECLARE @MonthlyRentBatchId INT

				--SET @CurrentDate = GETDATE()

				--INSERT LOG BATCH DAILY RUN FOR A MONTH
				INSERT INTO MonthlyRentBatch VALUES(MONTH(@CurrentDate), YEAR(@CurrentDate), @CurrentDate, NULL)
				--FETCH @MonthlyRentBatchId FOR PROCESS END DATE  LATER UPDATE
				SET @MonthlyRentBatchId = @@IDENTITY

				SET @SystemDateTimeProcessed = @CurrentDate
				SET @Note = 'PROCESSED BY THE SYSTEM'
				SET @TransactionTypeAsMonthlyRent = 1 -- 1 for Monthly Rent

				SELECT @TenantGracePeriod = Value FROM Settings WHERE [Key] = 'TenantGracePeriod'

				--COMMENT FOR TESTING PURPOSES
				--SET @CurrentDate = DATEADD(MONTH, 1, DATEADD(DAY, @TenantGracePeriod, '2020-04-30'))

				--SET @CurrentDate = DATEADD(DAY, @TenantGracePeriod, '2020-04-30')

				DECLARE ProcessMonthlyRentTransactionsCursor CURSOR FOR
					SELECT 
						r.Id AS RenterId,
						rm.id RoomId, 
						rm.Price MonthlyRent,
						t.PaidAmount,
						t.Balance,
						t.IsDepositUsed,
						t.Id,
						r.NextDueDate
					FROM Rooms rm INNER JOIN Renters r ON rm.id = r.RoomId 
						LEFT JOIN RentTransactions t ON r.Id = t.RenterId AND CONVERT(VARCHAR(11), r.NextDueDate, 101) = CONVERT(VARCHAR(11), t.DueDate, 101)
					where IsEndRent = 0
					AND   IIF(t.IsProcessed IS NULL, 0, t.IsProcessed) = 0
					AND DATEADD(DAY, @TenantGracePeriod, r.NextDueDate) <= @CurrentDate


				OPEN ProcessMonthlyRentTransactionsCursor
				FETCH NEXT FROM ProcessMonthlyRentTransactionsCursor INTO @RenterId, @RoomId, @MonthlyRent, @PaidAmount, @Balance, @IsDeposit, @TransactionId, @DueDate
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @IsDeposit = 0
					--GET THE TOTAL PREVIOUS UNPAID AMOUNT BILL
					SET @PreviousRentArrearId = 0
					SET @PreviousRentTransactionId = 0
					SET @PreviousTotalBalance = 0

					SELECT 
						@PreviousRentArrearId  = Id,
						@PreviousRentTransactionId = RentTransactionId,
						@PreviousTotalBalance = UnpaidAmount
					FROM RentArrears
					WHERE RenterId = @RenterId
					AND IsProcessed = 0
	
					IF @PaidAmount IS NULL
					BEGIN
		
						--DUE DATE TIME
						--SET @DueDate = CAST((CONVERT(VARCHAR(2), @Month) + '/' + CONVERT(VARCHAR(2), @DueDay) + '/' + CONVERT(VARCHAR(4), @Year)) AS DATETIME)
						SET @DateStart = DATEADD(DAY, 1, @DueDate)
						SET @DateEnd = DATEADD(DAY, -1, DATEADD(MONTH, 1, @DateStart))
						SET @Period = FORMAT(@DateStart, 'dd-MMM') + ' to ' + FORMAT(@DateEnd, 'dd-MMM-yyyy')
		
						SET @TotalAmountDue = @MonthlyRent + IIF(@PreviousTotalBalance IS NULL, 0, @PreviousTotalBalance) --MonthlyRent + Balance
		
						--IF THERE'S REMAINING DEPOSIT DEDUCT INSTEAD OF ADDING TO ARREAR
						IF EXISTS(SELECT 1 FROM Renters WHERE Id = @RenterId AND MonthsUsed < AdvanceMonths)
						BEGIN
							SET @IsDeposit = 1
							--UPDATE MonthsUsed FIELD
							UPDATE Renters SET MonthsUsed = MonthsUsed + 1 WHERE Id = @RenterId

							--FETCH ONLY THE EXISTING BALANCE
							SET @TotalBalance = IIF(@PreviousTotalBalance IS NULL, 0, @PreviousTotalBalance) 
							SET @PaidDate = @DueDate
						END 
						ELSE
						BEGIN
							--TOTAL BALANCE HERE
							SET @TotalBalance = @TotalAmountDue;
							SET @PaidDate = NULL
						END
		

						--PROCESSED TRANSACTION BY SYSTEM IF CURRENT MONTH DUE STILL UNPAID
						INSERT INTO RentTransactions(
							RoomId,
							RenterId,
							Balance,
							Note,
							IsDepositUsed,
							DueDate,
							PaidDate,
							Period,
							TransactionType,
							IsSystemProcessed,
							TotalAmountDue,
							SystemDateTimeProcessed,
							IsProcessed)
						VALUES(
							@RoomId,
							@RenterId,
							@TotalBalance,
							@Note,
							@IsDeposit,
							@DueDate,
							@PaidDate,
							@Period,
							@TransactionTypeAsMonthlyRent,
							1,
							@TotalAmountDue,
							@SystemDateTimeProcessed,
							1)
		
						SET @TransactionId = @@IDENTITY

						--START RentTransactionDetails
						--INSERT DATA ON TRANSACTION DETAIL
						--INSERT PREVIOUS SAVE ARREAR
						IF (@PreviousTotalBalance > 0)
						BEGIN
							INSERT INTO RentTransactionDetails(TransactionId, Amount, RentArrearId)
								VALUES(@TransactionId, @PreviousTotalBalance, @PreviousRentArrearId)
						END
						--INSERT NEW UNPAID RENT
						INSERT INTO RentTransactionDetails(TransactionId, Amount, RentArrearId)
								VALUES(@TransactionId, @MonthlyRent, NULL)
						--END RentTransactionDetails

						IF @TotalBalance > 0
						BEGIN
							--INSERT TOTAL ARREAR/UNPAID AMOUNT BILL
							INSERT INTO RentArrears(
								RenterId,
								RentTransactionId,
								UnpaidAmount,
								IsProcessed)
							VALUES
								(@RenterId,
									@TransactionId,
									@TotalBalance,
									0)
						END

						--SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RentTransactionId = @PreviousRentTransactionId
								AND IsManualEntry = 0	

					    --SET THE PREVIOUS TOTAL BALANCE THAT WAS MANUAL ENTRY IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RenterId = @RenterId
								AND IsManualEntry = 1

						UPDATE RentTransactions
							SET IsProcessed = 1
								WHERE Id = @TransactionId
		
					END

					ELSE IF @Balance > 0
					BEGIN
						--INSERT TOTAL ARREAR/UNPAID AMOUNT BILL
						INSERT INTO RentArrears(
							RenterId,
							RentTransactionId,
							UnpaidAmount,
							IsProcessed)
						VALUES
							(@RenterId,
								@TransactionId,
								@Balance,
								0)
		
						--SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RentTransactionId = @PreviousRentTransactionId
								AND IsManualEntry = 0	

						--SET THE PREVIOUS TOTAL BALANCE THAT WAS MANUAL ENTRY IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RenterId = @RenterId
								AND IsManualEntry = 1
		
						--UPDATE THE @SystemDateTimeProcessed
						UPDATE RentTransactions
							SET SystemDateTimeProcessed = @SystemDateTimeProcessed,
								IsProcessed = 1
							WHERE Id = @TransactionId
					END

					ELSE IF @IsDeposit = 1
					BEGIN
		
						IF @PreviousTotalBalance > 0
						BEGIN
							--INSERT TOTAL ARREAR/UNPAID AMOUNT BILL
							INSERT INTO RentArrears(
								RenterId,
								RentTransactionId,
								UnpaidAmount,
								IsProcessed)
							VALUES
								(@RenterId,
								@TransactionId,
								@PreviousTotalBalance,
								0)
						END

						--SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RentTransactionId = @PreviousRentTransactionId
								AND IsManualEntry = 0	

						--SET THE PREVIOUS TOTAL BALANCE THAT WAS MANUAL ENTRY IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RenterId = @RenterId
								AND IsManualEntry = 1

						--UPDATE THE @SystemDateTimeProcessed
						UPDATE RentTransactions
							SET SystemDateTimeProcessed = @SystemDateTimeProcessed,
								IsProcessed = 1
							WHERE Id = @TransactionId
					END

					ELSE IF @PaidDate > 0 AND @Balance IS NULL
					BEGIN

						SELECT @RentArrearId = RentArrearId FROM RentTransactionDetails
						WHERE TransactionId = @TransactionId AND RentArrearId IS NOT NULL
						--TAGGED AS IsProcessed = 1 / true
						UPDATE RentArrears SET IsProcessed = 1, Note = 'Fully paid including arrears' WHERE Id = @RentArrearId

						--UPDATE THE @SystemDateTimeProcessed
						UPDATE RentTransactions
							SET SystemDateTimeProcessed = @SystemDateTimeProcessed,
								IsProcessed = 1
							WHERE Id = @TransactionId
					END

					--UPDATE PREVIOUS AND NEXT DUE DATE
					UPDATE Renters 
						SET PreviousDueDate = NextDueDate,
							NextDueDate = DATEADD(MONTH,1, NextDueDate)
								WHERE Id = @RenterId

					FETCH NEXT FROM ProcessMonthlyRentTransactionsCursor INTO @RenterId, @RoomId, @MonthlyRent, @PaidAmount, @Balance, @IsDeposit, @TransactionId, @DueDate
				END
				CLOSE ProcessMonthlyRentTransactionsCursor
				DEALLOCATE ProcessMonthlyRentTransactionsCursor

				--UPDATE ProcesssEndDateTime SO THAT BATCH DATE FOR TODAY RUN ONCE
				UPDATE MonthlyRentBatch SET ProcesssEndDateTime = GETDATE() WHERE Id = @MonthlyRentBatchId
				--SELECT 1 / 0 AS Error;
			COMMIT TRANSACTION
		END TRY
		BEGIN CATCH
			BEGIN
				ROLLBACK TRANSACTION
				INSERT dbErrorLogs VALUES(
					'Failed daily batch process', 
					ERROR_LINE(),
					ERROR_MESSAGE(),
					ERROR_PROCEDURE(),
					ERROR_NUMBER(),
					ERROR_SEVERITY(),
					ERROR_STATE(),
					GETDATE())
			END
		END CATCH
	END
END
GO
