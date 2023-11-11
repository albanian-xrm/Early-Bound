using Newtonsoft.Json;
using System.Collections.Generic;

namespace AlbanianXrm.EarlyBound.Models
{
    public class NuGetApi
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("resources")]
        public List<NuGetResource> Resources { get; set; }
    }

    public class NuGetResource
    {
        [JsonProperty("@id")]
        public string @id { get; set; }

        [JsonProperty("@type")]
        public string @type { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    public class NuGetSearch
    {
        [JsonProperty("data")]
        public List<NuGetSearchData> Data { get; set; }
    }

    public class NuGetSearchData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("versions")]
        public List<NuGetVersion> Versions { get; set; }
    }

    public class NuGetVersion
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("@id")]
        public string @Id { get; set; }
    }

    public class NuGetPackage
    {
        [JsonProperty("packageContent")]
        public string PackageContent { get; set;}
    }
}
