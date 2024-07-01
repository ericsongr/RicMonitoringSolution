
using System;
using System.Collections.Generic;
using System.Linq;
using RicModel.RicXplorer;

namespace RicModel.RoomRent.Extensions
{
    public static class BookingOptionsExtensions
    {
        public static List<int> GetBookingOptionIds(this BookingType bookingType)
        {
            if (bookingType == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(bookingType.LinkRooms))
                return new List<int>();

            var list = bookingType.LinkRooms
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            return list;
        }
    }
}
