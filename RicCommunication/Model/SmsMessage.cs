using System;

namespace RicCommunication.Model
{
    public class SmsMessage
    {
        public string MessageId { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
