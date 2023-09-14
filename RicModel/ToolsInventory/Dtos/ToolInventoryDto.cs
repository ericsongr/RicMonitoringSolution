using System;

namespace RicModel.ToolsInventory.Dtos
{
    public class ToolInventoryDto
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        
        public string InventoryDateTime { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTimeUtc { get; set; }
        public string DeletedBy { get; set; }

    }
}
