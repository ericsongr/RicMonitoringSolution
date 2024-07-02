using AutoMapper;
using RicModel.CostMonitoring.Dtos;
using RicModel.CostMonitoring;

namespace RicMonitoringAPI.MappingProfiles
{
    public class CostMonitoringProfile : Profile
    {
        public CostMonitoringProfile()
        {
            CreateMap<CostItem, CostItemDto>();

            CreateMap<TransactionCost, TransactionCostDto>()
                .ForMember(dest => dest.TransactionDate,
                    opt => opt.MapFrom(src => src.TransactionDate.ToShortDateString()))
                .ForMember(dest => dest.CostItemName,
                    opt => opt.MapFrom(src => src.CostItem.Name))
                .ForMember(dest => dest.CostCategoryName,
                    opt => opt.MapFrom(src => src.CostCategory.Description));
        }
    }
}
