using RicModel.Helpers;

namespace RicModel.RoomRent.Dtos
{
    public class RenterDto : Renter
    {
        public string Base64 { get; set; }

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
