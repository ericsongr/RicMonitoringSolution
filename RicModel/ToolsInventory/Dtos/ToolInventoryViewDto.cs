using System;
using System.Collections.Generic;

namespace RicModel.ToolsInventory.Dtos
{
    public class ToolInventoryViewDto
    {
        public int Id { get; set; }

        public List<string> FilesInBase64 { get; set; }

        public string InventoryDate { get; set; }
       
        public string Status { get; set; }
        public string Action { get; set; }


    }
}
