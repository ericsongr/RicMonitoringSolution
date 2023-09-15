using System.Collections.Generic;

namespace RicModel.ToolsInventory.Dtos
{
    public class CreateNewToolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> FilesInBase64 { get; set; }
        public bool PowerTool { get; set; }
        
     
    }
}
