using System;
using System.Collections.Generic;

namespace Frontend
{
    public class Thumbnail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HashSet<string> Websites { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

    }
}
