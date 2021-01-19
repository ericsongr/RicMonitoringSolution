using System;

namespace RicModel.RoomRent
{
    public class MobileAppLog
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string LogInfo { get; set; }
        public DateTime UtcCreatedDateTime { get; set; }
    }
}
