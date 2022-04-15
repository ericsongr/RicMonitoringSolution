using AutoMapper;
using RicModel.RicXplorer;
using RicModel.RoomRent.Dtos;

namespace RicMonitoringAPI.RicXplorer.MapCustomResolver
{
    public class BookingTypeDetailResolver : IValueResolver<BookingTypeDetail, BookingTypeDetailDto, BookingTypeDetailDto>
    {
        public BookingTypeDetailDto Resolve(BookingTypeDetail source, BookingTypeDetailDto destination, BookingTypeDetailDto member, ResolutionContext context)
        {
            return Mapper.Map<BookingTypeDetailDto>(source);
        }
    }
}
