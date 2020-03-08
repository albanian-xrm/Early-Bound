using System.Collections.Generic;

namespace AlbanianXrm
{
    public class EntitySelection
    {
        public EntitySelection(string logicalName)
        {
            LogicalName = logicalName;
            SelectedAttributes = new HashSet<string>();
            SelectedRelationships = new HashSet<string>();
        }
        public string LogicalName { get; set; }
        public bool AllAttributes { get; set; }
        public bool AllRelationships { get; set; }
        public HashSet<string> SelectedAttributes { get; private set; }
        public HashSet<string> SelectedRelationships { get; private set; }
    }
}
