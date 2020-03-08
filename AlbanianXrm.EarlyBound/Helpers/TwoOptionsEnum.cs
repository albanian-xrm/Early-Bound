﻿using System.ComponentModel;

namespace AlbanianXrm.EarlyBound.Helpers
{
    public enum TwoOptionsEnum : int
    {
        [Description("NO")]
        NO = 0,
        [Description("Enumeration")]
        Enumeration = 1,
        [Description("Boolean Constants")]
        Constants = 2
    }
}
