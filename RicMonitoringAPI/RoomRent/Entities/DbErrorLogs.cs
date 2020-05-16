using System;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class DbErrorLog
    {
        public int Id { get; set; }
        public string ProcessMessage { get; set; }
        public int Line { get; set; }
        public string Message { get; set; }
        public string Procedure { get; set; }
        public int Number { get; set; }
        public int Severity { get; set; }
        public int State { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
