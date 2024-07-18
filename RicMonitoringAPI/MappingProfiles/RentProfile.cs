using AutoMapper;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent.Dtos.Audits;
using RicModel.RoomRent.Extensions;
using RicMonitoringAPI.RoomRent.Helpers.Extensions;

namespace RicMonitoringAPI.MappingProfiles
{
    public class RentProfile : Profile
    {

        public RentProfile()
        {
            //settings
            CreateMap<Account, AccountDto>();
            CreateMap<MonthlyRentBatch, MonthlyRentBatchDto>();
            CreateMap<AuditRentTransaction, AuditRentTransactionDto>();
            CreateMap<AuditRenter, AuditRenterDto>();
            CreateMap<AuditRoom, AuditRoomDto>();

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
            CreateMap<LookupType, LookupTypeCategoryDto>();
            CreateMap<LookupTypeItem, LookupTypeItemDto>();
            CreateMap<RentTransactionPayment, RentTransactionPaymentDto>()
                .ForMember(dest => dest.DatePaidString,
                            opt => opt.MapFrom(src => src.DatePaid.ToShortDateString()))
                .ForMember(dest => dest.PaymentTransactionType,
                    opt => opt.MapFrom(src => src.GetTransactionPaymentType()));

        }

    }
}