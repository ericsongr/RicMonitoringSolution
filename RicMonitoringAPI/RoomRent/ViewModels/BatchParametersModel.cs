namespace RicMonitoringAPI.RoomRent.ViewModels
{
    public class BatchParametersModel
    {
        public string IncomingDueDateRegisteredDevicesJsonString { get; set; }
        public string DueDateRegisteredDevicesJsonString { get; set; }
        public string SettledPaymentRegisteredDevicesJsonString { get; set; }
        public string TenantBatchFileCompletedRegisteredDevicesJsonString { get; set; }
    }
}
