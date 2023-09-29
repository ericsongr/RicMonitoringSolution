using System;
using RicModel.RoomRent;

namespace RicModel.ToolsInventory
{
    public class ToolInventory
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public Tool Tool { get; set; }
        public string Images { get; set; }

        public DateTime InventoryDateTimeUtc { get; set; }
        public int Status { get; set; }
        public LookupTypeItem LookupTypeItemStatus { get; set; }
        public int Action { get; set; }
        public LookupTypeItem LookupTypeItemAction { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTimeUtc { get; set; }
        public string DeletedBy { get; set; }

    }
}
