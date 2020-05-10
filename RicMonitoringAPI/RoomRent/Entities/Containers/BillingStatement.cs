namespace RicMonitoringAPI.RoomRent.Entities.Containers
{
    public class BillingStatement
    {
        public string RenterName { get; set; }
        public string RoomName { get; set; }
        public string Period { get; set; }
        public string DueDate { get; set; }
        public string MonthlyRent { get; set; }
        public bool HasPreviousBalance { get; set; }
        public string PreviousUnpaidAmount { get; set; }
        public string TotalAmountDue { get; set; }
    }
}
