using System;
using System.Linq;
using AutoMapper;
using RicModel.CostMonitoring;
using RicModel.CostMonitoring.Dtos;
using RicModel.RicXplorer;
using RicModel.RicXplorer.Dtos;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Extensions;
using RicModel.ToolsInventory;
using RicModel.ToolsInventory.Dtos;
using RicMonitoringAPI.RicXplorer.ViewModels;
using RicMonitoringAPI.RoomRent.Helpers.Extensions;

namespace RicMonitoringAPI
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //settings
            CreateMap<Setting, SettingDto>()
                   .ForMember(dest => dest.DataType,
                               opt => opt.MapFrom(src => src.GetDataType()))
                   .ForMember(dest => dest.RealValue,
                       opt => opt.MapFrom(src => src.GetRealValue()));

            //rooms
            CreateMap<RoomForCreateDto, Room>();
            CreateMap<Room, RoomDto>();

            //renters
            CreateMap<RenterForCreateDto, Renter>();
            CreateMap<Renter, RenterDto>();

            //transactions
            CreateMap<RentTransactionForCreateDto, RentTransaction>();
            CreateMap<RentTransaction, RentTransactionDto>();

            CreateMap<RentTransaction2, RentTransaction2Dto>()
                .ForMember(dest => dest.DueDate,
                            opt => opt.MapFrom(src => src.GetDueDate()))
                .ForMember(dest => dest.Period,
                                        opt => opt.MapFrom(src => src.GetPeriod()));

            //histories
            CreateMap<RentTransaction, RentTransactionHistoryDto>()
                .ForMember(dest => dest.PreviousBalance,
                                            opt => opt.MapFrom(src => src.GetPreviousBalance() + src.Renter.RentArrears.GetManualUnpaidAmountEntry()))
                .ForMember(dest => dest.PaidOrUsedDepositDateString,
                    opt => opt.MapFrom(src => src.GetPaidOrUsedDepositDate()))
                .ForMember(dest => dest.MonthlyRent,
                    opt => opt.MapFrom(src => src.GetMonthlyRent()))
                .ForMember(dest => dest.CurrentBalance,
                    opt => opt.MapFrom(src => src.Balance))
                .ForMember(dest => dest.IsDepositUsed,
                    opt => opt.MapFrom(src => src.CheckIfUsedDeposit()))
                .ForMember(dest => dest.BalanceDateToBePaidString,
                    opt => opt.MapFrom(src => src.GetBalanceDateToBePaid()))
                .ForMember(dest => dest.Payments,
                    opt => opt.MapFrom(src => src.GetPayments()));

            CreateMap<LookupType, LookupTypeDto>();
            CreateMap<LookupTypeItem, LookupTypeItemDto>();
            CreateMap<RentTransactionPayment, RentTransactionPaymentDto>()
                .ForMember(dest => dest.DatePaidString,
                            opt => opt.MapFrom(src => src.DatePaid.ToShortDateString()))
                .ForMember(dest => dest.PaymentTransactionType,
                    opt => opt.MapFrom(src => src.GetTransactionPaymentType()));

            CreateMap<BookingType, BookingTypeDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.AccountProduct.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.AccountProduct.Description))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.AccountProduct.Price.ToString("#,#00")))
                .ForMember(dest => dest.OnlinePrice,
                    opt => opt.MapFrom(src => src.AccountProduct.OnlinePrice.ToString("#,#00")))
                .ForMember(dest => dest.BookingTypeInclusions,
                            opt => opt.MapFrom(src => src.BookingTypeInclusions.Where(o => o.IsActive).ToList()))
                .ForMember(dest => dest.BookingTypeImages,
                    opt => opt.MapFrom(src => src.BookingTypeImages.Where(o => o.IsShow).ToList()));

            CreateMap<BookingTypeInclusion, BookingTypeInclusionDto>()
                .ForMember(dest => dest.InclusionName,
                    opt => opt.MapFrom(src => src.LookupTypeItem.Description));

            CreateMap<BookingTypeImage, BookingTypeImageDto>();

            //ricxplorer
            CreateMap<GuestBookingDetailDto, GuestBookingDetail>()
                .ForMember(dest => dest.CreatedDateTimeUtc,
                    opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.GuestBookings,
                            opt => opt.MapFrom(src => src.GuestBookings));

            CreateMap<GuestBookingDto, GuestBooking>();
            CreateMap<GuestBooking, GuestBookingDto>();

            CreateMap<GuestBookingDetail, GuestBookingDetailDto>()
                .ForMember(dest => dest.BookingTypeName,
                    opt => opt.MapFrom(src => src.BookingTypeModel.AccountProduct.Name))
                .ForMember(dest => dest.ArrivalDateString,
                    opt => opt.MapFrom(src => src.ArrivalDate.ToString("dddd, MMMM dd yyyy")))
                .ForMember(dest => dest.DepartureDateString,
                    opt => opt.MapFrom(src => src.DepartureDate.ToString("dddd, MMMM dd yyyy")))
                .ForMember(dest => dest.CreatedDateTimeUtcString,
                    opt => opt.MapFrom(src => src.CreatedDateTimeUtc.ToString("f")));

            //cost monitoring
            CreateMap<CostItem, CostItemDto>();

            CreateMap<TransactionCost, TransactionCostDto>()
                .ForMember(dest => dest.TransactionDate,
                    opt => opt.MapFrom(src => src.TransactionDate.ToShortDateString()))
                .ForMember(dest => dest.CostItemName,
                    opt => opt.MapFrom(src => src.CostItem.Name))
                .ForMember(dest => dest.CostCategoryName,
                    opt => opt.MapFrom(src => src.CostCategory.Description));

            //tool inventory
            //CreateMap<Tool, ToolDto>()
            //    .ForMember(dest => dest.Images, opt => opt.Ignore()); //ignore

            CreateMap<Tool, ToolViewDto>()
                .ForMember(dest => dest.ToolsInventory, opt => 
                    opt.MapFrom(src => src.ToolsInventory
                        .OrderByDescending(o => o.InventoryDateTimeUtc)));

            CreateMap<ToolInventory, ToolInventoryDto>()
                .ForMember(dest => dest.InventoryDateTime,
                opt => opt.MapFrom(src => src.InventoryDateTimeUtc.ToString("f")));
            

            CreateMap<Tool, ToolViewDetailDto>()
                .ForMember(dest => dest.ToolsInventory, opt =>
                    opt.MapFrom(src => src.ToolsInventory
                        .OrderByDescending(o => o.InventoryDateTimeUtc)));
            
            CreateMap<ToolInventory, ToolInventoryDetailDto>()
                .ForMember(dest => dest.InventoryDateTime,
                opt => opt.MapFrom(src => src.InventoryDateTimeUtc.ToString("f")));

            //    .ForMember(dest => dest.CostItemName,
            //        opt => opt.MapFrom(src => src.CostItem.Name))
            //    .ForMember(dest => dest.CostCategoryName,
            //        opt => opt.MapFrom(src => src.CostCategory.Description));

        }
       
    }
}