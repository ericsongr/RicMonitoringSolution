using RicModel.RicXplorer;

namespace RicModel.RoomRent
{
    public class GuestCheckList
    {
        public int Id { get; set; }
        public int GuestBookingDetailId { get; set; }
        public int CheckListId { get; set; }
        public bool IsChecked { get; set; }
        public string Notes { get; set; }

        public virtual GuestBookingDetail GuestBookingDetail { get; set; }
        public virtual LookupTypeItem CheckList { get; set; }
    }
}
