using System.Collections.Generic;

namespace RicModel.ToolsInventory.Dtos
{
    public class ToolViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public bool PowerTool { get; set; }

        public ICollection<ToolInventoryDto> ToolsInventory { get; set; }
    }
}
