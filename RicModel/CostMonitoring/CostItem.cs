using System.Collections.Generic;

namespace RicModel.CostMonitoring
{
    public class CostItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TransactionCost> TransactionCosts { get; set; }
    }
}
