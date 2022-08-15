using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabManagerWinUi.Models
{
    internal class TabGroup
    {
        public string Name { get; set; }
        public IEnumerable<Tab> Tabs { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
