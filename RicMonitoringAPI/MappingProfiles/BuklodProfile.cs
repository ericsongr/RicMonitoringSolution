using AutoMapper;
using RicModel.Inc.Dtos;
using RicModel.Inc;
using RicModel.RoomRent.Extensions;

namespace RicMonitoringAPI.MappingProfiles
{
    public class BuklodProfile : Profile
    {
        public BuklodProfile()
        {
            CreateMap<IncBuklodCreateDto, IncBuklod>();
            CreateMap<IncBuklod, IncBuklodViewDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.LastName}, {src.FirstName}"))
                .ForMember(dest => dest.AnniversaryString,
                    opt => opt.MapFrom(src => src.GetAnniversary()))
                .ForMember(dest => dest.BirthdayString,
                    opt => opt.MapFrom(src => src.GetBirthday()));

            CreateMap<IncBuklod, IncBuklodDto>()
                .ForMember(dest => dest.AnniversaryString,
                    opt => opt.MapFrom(src => src.GetAnniversary2()))
                .ForMember(dest => dest.BirthdayString,
                    opt => opt.MapFrom(src => src.GetBirthday2()));
        }
    }
}
