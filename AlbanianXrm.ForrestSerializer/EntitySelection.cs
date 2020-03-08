using System.Collections.Generic;

namespace AlbanianXrm
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
        public List<string> SelectedAttributes { get; private set; }
        public List<string> SelectedRelationships { get; private set; }
    }
}
