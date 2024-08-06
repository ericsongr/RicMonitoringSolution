namespace RicMonitoringAPI.RicXplorer.ViewModels
{
    public class GuestBookingTypeModel
    {
        public int BookingTypeId { get; set; }
        public bool IsActive { get; set; }
        public decimal OnlinePrice { get; set; }
        public decimal Price { get; set; }
        public int NoOfPersons { get; set; }
        public int NoOfPersonsMax { get; set; }
    }
}
