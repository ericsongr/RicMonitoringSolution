using RicModel.RoomRent;

namespace RicModel.CostMonitoring
{
    public class TransactionCost
    {
        public int Id { get; set; }

        public int CostItemId { get; set; }
        public virtual CostItem CostItem { get; set; }

        public int CostCategoryId { get; set; }
        public virtual LookupTypeItem CostCategory { get; set; }

        public decimal Cost { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
