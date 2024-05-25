using System;
using RicModel.Inc;

namespace RicModel.RoomRent.Extensions
{
    public static class BuklodExtensions
    {

        public static string GetAnniversary(this IncBuklod buklod)
        {
            if (buklod == null)
            {
                throw new ArgumentNullException("source");
            }

            if (buklod.Anniversary == null)
            {
                return string.Empty;
            }
            else
            {
                return buklod.Anniversary.Value.ToString("dd-MMM-yyyy");
            }
        }

        public static string GetBirthday(this IncBuklod buklod)
        {
            if (buklod == null)
            {
                throw new ArgumentNullException("source");
            }

            if (buklod.Birthday == null)
            {
                return string.Empty;
            }
            else
            {
                return buklod.Birthday.Value.ToString("dd-MMM-yyyy");
            }
        }

        public static string GetAnniversary2(this IncBuklod buklod)
        {
            if (buklod == null)
            {
                throw new ArgumentNullException("source");
            }

            if (buklod.Anniversary == null)
            {
                return string.Empty;
            }
            else
            {
                return buklod.Anniversary.Value.ToString("MM/dd/yyyy");
            }
        }

        public static string GetBirthday2(this IncBuklod buklod)
        {
            if (buklod == null)
            {
                throw new ArgumentNullException("source");
            }

            if (buklod.Birthday == null)
            {
                return string.Empty;
            }
            else
            {
                return buklod.Birthday.Value.ToString("MM/dd/yyyy");
            }
        }
    }
}
