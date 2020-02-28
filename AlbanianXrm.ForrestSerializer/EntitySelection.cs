using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbanianXrm.ForrestSerializer
{
    public class EntitySelection
    {
        public EntitySelection(string logicalName)
        {
            LogicalName = logicalName;
            SelectedAttributes = new List<string>();
            SelectedRelationships = new List<string>();
        }
        public string LogicalName { get; set; }
        public bool AllAttributes { get; set; }
        public bool AllRelationships { get; set; }
        public List<string> SelectedAttributes { get; set; }
        public List<string> SelectedRelationships { get; set; }
    }
}
