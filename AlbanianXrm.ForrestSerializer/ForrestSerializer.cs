using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlbanianXrm
{
    public class ForrestSerializer
    {
        readonly string path;
        public ForrestSerializer(string path)
        {
            this.path = path;
        }

        public void Serialize(Dictionary<string, EntitySelection> entitySelections)
        {
            if (entitySelections == null) throw new ArgumentNullException(nameof(entitySelections));
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                Serialize(entitySelections, writer);
            }
        }

        internal static void Serialize(Dictionary<string, EntitySelection> entitySelections, StreamWriter writer)
        {
            foreach (var entitySelection in entitySelections.Values.OrderBy(o => o.LogicalName))
            {
                if (!(entitySelection.SelectedAttributes.Any() ||
                      entitySelection.SelectedRelationships.Any() ||
                      entitySelection.AllAttributes ||
                      entitySelection.AllRelationships)) continue;
                writer.WriteLine("╠╦═" + entitySelection.LogicalName);
                if (entitySelection.SelectedAttributes.Any() || entitySelection.AllAttributes)
                {
                    writer.WriteLine("║╠╦═Attributes");
                }
                foreach (var selectedAttribute in entitySelection.SelectedAttributes.OrderBy(o => o))
                {
                    writer.WriteLine("║║╠═" + selectedAttribute);
                }
                if (entitySelection.SelectedRelationships.Any() || entitySelection.AllRelationships)
                {
                    writer.WriteLine("║╠╦═Relationships");
                }
                foreach (var selectedRelationship in entitySelection.SelectedRelationships.OrderBy(o => o))
                {
                    writer.WriteLine("║║╠═" + selectedRelationship);
                }
            }
        }

        public Dictionary<string, EntitySelection> Deserialize()
        {
            if (!File.Exists(path)) return new Dictionary<string, EntitySelection>(); ;

            using (StreamReader reader = new StreamReader(path))
            {
                return Deserialize(reader);
            }
        }

        internal static Dictionary<string, EntitySelection> Deserialize(StreamReader reader)
        {
            Dictionary<string, EntitySelection> result = new Dictionary<string, EntitySelection>();
            string line;
            bool expectingEntity = true;
            bool expectingAttributeOrRelationship = false;
            bool readingAttribute = false;
            bool readingRelationship = false;
            EntitySelection entitySelection = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("╠╦═", StringComparison.InvariantCulture))
                {
                    if (expectingEntity)
                    {
                        entitySelection = new EntitySelection(line.Substring(3));
                        result.Add(entitySelection.LogicalName, entitySelection);
                        expectingEntity = false;
                        readingAttribute = false;
                        readingRelationship = false;
                        expectingAttributeOrRelationship = true;
                    }
                    else
                    {
                        return new Dictionary<string, EntitySelection>();
                    }
                }
                else if (line.StartsWith("║╠╦═", StringComparison.InvariantCulture))
                {
                    if (expectingAttributeOrRelationship)
                    {
                        if (line == "║╠╦═Attributes" && !readingAttribute)
                        {
                            readingAttribute = true;
                            expectingEntity = true;
                            entitySelection.AllAttributes = true;
                        }
                        else if (line == "║╠╦═Relationships" && !readingRelationship)
                        {
                            readingAttribute = false;
                            readingRelationship = true;
                            expectingEntity = true;
                            expectingAttributeOrRelationship = false;
                            entitySelection.AllRelationships = true;
                        }
                        else
                        {
                            return new Dictionary<string, EntitySelection>();
                        }
                    }
                    else
                    {
                        return new Dictionary<string, EntitySelection>();
                    }
                }
                else if (line.StartsWith("║║╠═", StringComparison.InvariantCulture))
                {
                    if (readingAttribute)
                    {
                        expectingEntity = true;
                        expectingAttributeOrRelationship = true;
                        entitySelection.AllAttributes = false;
                        entitySelection.SelectedAttributes.Add(line.Substring(4));
                    }
                    else if (readingRelationship)
                    {
                        expectingEntity = true;
                        expectingAttributeOrRelationship = false;
                        entitySelection.AllRelationships = false;
                        entitySelection.SelectedAttributes.Add(line.Substring(4));
                    }
                    else
                    {
                        return new Dictionary<string, EntitySelection>();
                    }
                }
                else
                {
                    return new Dictionary<string, EntitySelection>();
                }
            }
            return result;
        }
    }
}
