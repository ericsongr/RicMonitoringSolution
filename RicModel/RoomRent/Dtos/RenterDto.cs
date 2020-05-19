using RicModel.Helpers;

namespace RicModel.RoomRent.Dtos
{
    public class RenterDto : Renter
    {
        public string AdvancePaidDateString
        {
            get { return AdvancePaidDate.ToString("dd-MMM-yyyy"); }
        }
        public string StartDateString
        {
            get { return StartDate.ToString("dd-MMM-yyyy"); }
        }
        public string DueDayString
        {
            get { return $"{DueDay}{CommonFunctions.GetSuffix(DueDay.ToString())}"; }
        }
    }
}
