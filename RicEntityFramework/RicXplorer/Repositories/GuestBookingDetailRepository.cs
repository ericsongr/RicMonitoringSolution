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

        public IQueryable<GuestBookingDetail> FindBookings(DateTime startDate, DateTime endDate)
        {
            return Context.GuestBookingDetails.Where(o => o.ArrivalDate >= startDate && o.ArrivalDate <= endDate ||
                                                          o.DepartureDate >= startDate && o.DepartureDate <= endDate ||
                                                          startDate >= o.ArrivalDate && endDate <= o.DepartureDate);
        }
    }
}
