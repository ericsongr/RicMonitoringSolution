using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public IQueryable<GuestBookingDate> Find(DateTime startDate, DateTime endDate, int bookingType)
        {

            return Context.GuestBookingDates
                .Where(o => o.DateBooked >= startDate && o.DateBooked <= endDate
                                                      && o.GuestBookingDetail.BookingType == bookingType);
        }

        public IQueryable<GuestBookingDetail> FindBookings(DateTime startDate, DateTime endDate, int bookingType = 0)
        {

            var guests =  Context.GuestBookingDates.Include(o => o.GuestBookingDetail).ThenInclude(o => o.GuestBookings)
                .Where(o => o.DateBooked >= startDate && o.DateBooked <= endDate);

            if (bookingType != 0)
            {
                guests = guests.Where(o => o.GuestBookingDetail.BookingType == bookingType);
            }

            return guests.Select(o => o.GuestBookingDetail).AsNoTracking();
        }
        
    }
}
