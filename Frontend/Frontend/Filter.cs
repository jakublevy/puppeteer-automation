﻿using System.Collections.Generic;

namespace Frontend
{
    public class Filter
    {
        //Each property contains values that should be hidden when using a filter.

        public List<string> EventTypes { get; set; } = new List<string>();
        public List<string> Targets { get; set; } = new List<string>();
        public List<string> Status { get; set; } = new List<string>();
    }
}
