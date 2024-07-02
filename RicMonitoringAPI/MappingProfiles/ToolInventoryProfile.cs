using System.Linq;
using AutoMapper;
using RicModel.ToolsInventory;
using RicModel.ToolsInventory.Dtos;

namespace RicMonitoringAPI.MappingProfiles
{
    public class ToolInventoryProfile : Profile
    {
        public ToolInventoryProfile()
        {
            CreateMap<Tool, ToolViewDto>()
                .ForMember(dest => dest.ToolsInventory, opt =>
                    opt.MapFrom(src => src.ToolsInventory
                        .OrderByDescending(o => o.InventoryDateTimeUtc)));

            CreateMap<ToolInventory, ToolInventoryDto>()
                .ForMember(dest => dest.InventoryDate,
                    opt => opt.MapFrom(src => src.InventoryDateTimeUtc.ToString("f")));

            CreateMap<ToolInventory, ToolInventoryViewDto>()
                .ForMember(dest => dest.InventoryDate,
                    opt => opt.MapFrom(src => src.InventoryDateTimeUtc.ToShortDateString()))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.LookupTypeItemAction.Description))
                .ForMember(dest => dest.Action,
                    opt => opt.MapFrom(src => src.LookupTypeItemStatus.Description));


            CreateMap<Tool, ToolViewDetailDto>()
                .ForMember(dest => dest.ToolsInventory, opt =>
                    opt.MapFrom(src => src.ToolsInventory
                        .OrderByDescending(o => o.InventoryDateTimeUtc)));

            CreateMap<ToolInventory, ToolInventoryDetailDto>()
                .ForMember(dest => dest.InventoryDateTime,
                    opt => opt.MapFrom(src => src.InventoryDateTimeUtc.ToString("f")));
        }
    }
}
