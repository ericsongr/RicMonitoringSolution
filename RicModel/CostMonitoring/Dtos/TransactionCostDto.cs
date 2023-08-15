using RicModel.RoomRent;

namespace RicModel.CostMonitoring.Dtos
{
    public class TransactionCostDto
    {
        public int Id { get; set; }

        public int CostItemId { get; set; }
        public string CostItemName { get; set; }

        public int CostCategoryId { get; set; }
        public string CostCategoryName { get; set; }


        public string Note { get; set; }
    }
}
