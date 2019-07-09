using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AlbanianXrm.EarlyBound
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    public class Settings
    {
        public string OutputPath { get; set; }
        public string Namespace { get; set; }
    }
}