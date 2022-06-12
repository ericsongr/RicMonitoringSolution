using System;
using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Interfaces
{
    public interface IGuestBookingDetailRepository : IEntityBaseRepository<GuestBookingDetail>
    {
        IQueryable<GuestBookingDate> Find(DateTime startDate, DateTime endDate, int bookingType);
        IQueryable<GuestBookingDetail> FindBookings(DateTime startDate, DateTime endDate, int bookingType);
    }
}