
using System.Collections.Generic;

namespace RicModel.ToolsInventory.Dtos
{
    public class ToolViewDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool PowerTool { get; set; }

        public ICollection<ToolInventoryDetailDto> ToolsInventory { get; set; }
    }
}
