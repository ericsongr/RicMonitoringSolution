using System.Collections.Generic;

namespace RicModel.RicXplorer
{
    public class AccountProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OnlinePrice { get; set; }
        public bool IsWebPurchasable { get; set; }
        public int MaximumLevelQuantity { get; set; }
        public int MinimumLevelQuantity { get; set; }
        public int WarnLevelQuantity { get; set; }

        public int AccountProductCategoryId { get; set; }
        public virtual AccountProductCategory AccountProductCategory { get; set; }
        public ICollection<BookingType> BookingTypes { get; set; }

    }
}
