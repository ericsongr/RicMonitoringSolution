using System;
using System.Collections.Generic;

namespace RicModel.ToolsInventory
{
    public class Tool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public bool PowerTool { get; set; }
        
        public DateTime CreatedDateTimeUtc { get; set; }
        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTimeUtc { get; set; }
        public string DeletedBy { get; set; }


        public virtual ICollection<ToolInventory> ToolsInventory { get; set; }

    }
}
