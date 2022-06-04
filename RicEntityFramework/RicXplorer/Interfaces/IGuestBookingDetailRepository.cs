using System;
using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Interfaces
{
    public interface IGuestBookingDetailRepository : IEntityBaseRepository<GuestBookingDetail>
    {
        IQueryable<GuestBookingDetail> FindBookings(DateTime startDate, DateTime endDate);
    }
}