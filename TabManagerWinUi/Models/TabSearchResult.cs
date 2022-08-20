using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabManagerWinUi.Models
{
    internal class TabSearchResult
    {
        public TabGroup TabGroup { get; init; }
        public List<Tab> Tab { get; set; }
        
    }
}
