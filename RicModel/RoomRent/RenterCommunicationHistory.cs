using System;

namespace RicModel.RoomRent
{
    public partial class RenterCommunicationHistory
    {
        public long Id { get; set; }
        
        public int RenterId { get; set; }
        public virtual Renter Renter { get; set; }

        public DateTime? CommunicationUtcdateTime { get; set; }
        public int? CommunicationType { get; set; }
        public string CommunicationText { get; set; }
        public string CommunicationSentTo { get; set; }
        public string RequestedBy { get; set; }
        public bool? IsSuccessfullySent { get; set; }
        public string BatchId { get; set; }
        public string MessageId { get; set; }
        public bool? HasRead { get; set; }
        public string AttachmentFileName { get; set; }
        public string ContentType { get; set; }
    }
}
