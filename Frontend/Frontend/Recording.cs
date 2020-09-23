using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public class Recording
    {
        public List<dynamic> Actions { get; set; } = new List<dynamic>();
    }

    public class Thumbnail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Uri> Websites { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

    }
}
