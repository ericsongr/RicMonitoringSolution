namespace RicModel.RoomRent
{
    public class SmsGateway
    {
        public int SmsGatewayId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string GatewayUrl { get; set; }
        public string DedicatedNumber { get; set; }
    }
}
