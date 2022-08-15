using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabManagerWinUi.Models
{
    internal class Tab
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
    }
}
