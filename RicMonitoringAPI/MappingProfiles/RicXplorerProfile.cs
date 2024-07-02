using AutoMapper;
using RicModel.RicXplorer;
using RicModel.RoomRent.Dtos;
using RicModel.RoomRent;
using RicMonitoringAPI.RicXplorer.ViewModels;
using System;
using System.Linq;
using RicModel.RicXplorer.Dtos;
using RicModel.RoomRent.Extensions;

namespace RicMonitoringAPI.MappingProfiles
{
    public class RicXplorerProfile : Profile
    {
        public RicXplorerProfile()
        {
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

            CreateMap<GuestBookingDetailDto, GuestBookingDetail>()
                .ForMember(dest => dest.CreatedDateTimeUtc,
                    opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.GuestBookings,
                            opt => opt.MapFrom(src => src.GuestBookings));

            CreateMap<GuestBookingDto, GuestBooking>();
            CreateMap<GuestBooking, GuestBookingDto>();

            CreateMap<GuestBookingDetail, GuestBookingListDto>()
                .ForMember(dest => dest.BookingTypeName,
                    opt => opt.MapFrom(src => src.BookingTypeModel.AccountProduct.Name))
                .ForMember(dest => dest.ArrivalDateString,
                    opt => opt.MapFrom(src => src.ArrivalDate.ToString("dddd, MMMM dd yyyy")))
                .ForMember(dest => dest.DepartureDateString,
                    opt => opt.MapFrom(src => src.DepartureDate.ToString("dddd, MMMM dd yyyy")))
                .ForMember(dest => dest.CreatedDateTimeUtcString,
                    opt => opt.MapFrom(src => src.CreatedDateTimeUtc.ToString("MMM dd, yyyy @ hh:mm tt")));

            CreateMap<GuestBookingDetail, GuestBookingDetailDto>()
                .ForMember(dest => dest.BookingTypeName,
                    opt => opt.MapFrom(src => src.BookingTypeModel.AccountProduct.Name))
                .ForMember(dest => dest.BookingOptionIds,
                    opt => opt.MapFrom(src => src.BookingTypeModel.GetBookingOptionIds()))
                .ForMember(dest => dest.ArrivalDateString,
                    opt => opt.MapFrom(src => src.ArrivalDate.ToString("dddd, MMMM dd yyyy")))
                .ForMember(dest => dest.DepartureDateString,
                    opt => opt.MapFrom(src => src.DepartureDate.ToString("dddd, MMMM dd yyyy")))
                .ForMember(dest => dest.CreatedDateTimeUtcString,
                    opt => opt.MapFrom(src => src.CreatedDateTimeUtc.ToString("f")))
                .ForMember(dest => dest.CreatedDateTimeUtcString2,
                    opt => opt.MapFrom(src => src.CreatedDateTimeUtc.ToString("MMM dd, yyyy @ hh:mm tt")))
                .ForMember(dest => dest.RoomId,
                    opt => opt.MapFrom(src => src.RoomOrBed == null ? 0 : src.RoomOrBed.LookupTypes.Id))
                .ForMember(dest => dest.RoomName,
                    opt => opt.MapFrom(src => src.RoomOrBed == null ? "" : src.RoomOrBed.LookupTypes.Name))
                .ForMember(dest => dest.RoomOrBedName,
                    opt => opt.MapFrom(src => src.RoomOrBed == null ? "" : $"{src.RoomOrBed.Notes} [{src.RoomOrBed.Description}]"));

            CreateMap<CheckListForCheckInOutGuest, GuestCheckListDetailDto>()
                .ForMember(dest => dest.CheckListId,
                    opt => opt.MapFrom(src => src.LookupTypeItemId))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.LookupTypeItem.Description));

            CreateMap<LookupType, GuestRoomTypesAvailabilityDto>()
                .ForMember(dest => dest.GuestRoomsOrBeds,
                    opt => opt.MapFrom(src => src.LookupTypeItems));

            CreateMap<LookupTypeItem, GuestRoomsOrBedsAvailabilityDto>();
        }
    }
}
