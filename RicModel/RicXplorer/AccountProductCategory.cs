using System.Collections.Generic;
using RicModel.RoomRent;

namespace RicModel.RicXplorer
{
    public class AccountProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public virtual ICollection<AccountProduct> AccountProducts { get; set; }
    }
}
