using RicMonitoringAPI.Api.Helpers;

namespace RicMonitoringAPI.RoomRent.Entities.Parameters
{
    public class RentTransactionResourceParameters : BaseResourceParameters
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}