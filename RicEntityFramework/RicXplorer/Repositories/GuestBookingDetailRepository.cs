using System;
using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.RicXplorer.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Repositories
{
    public class GuestBookingDetailRepository : EntityBaseRepository<GuestBookingDetail>, IGuestBookingDetailRepository
    {
        public GuestBookingDetailRepository(RicDbContext context) : base(context)
        {
        }

        public IQueryable<GuestBookingDetail> FindBookings(DateTime startDate, DateTime endDate, int bookingType)
        {

            return Context.GuestBookingDates
                .Where(o => o.DateBooked >= startDate && o.DateBooked <= endDate 
                                                      && o.GuestBookingDetail.BookingType == bookingType)
                .Select(o => o.GuestBookingDetail);
        }
    }
}
