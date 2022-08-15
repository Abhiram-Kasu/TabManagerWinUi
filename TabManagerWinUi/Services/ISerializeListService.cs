using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabManagerWinUi.Models;

namespace TabManagerWinUi.Services
{
    internal interface ISerializeListService
    {
        Task<bool> DoesExistingPrefsExist();
        Task<IEnumerable<TabGroup>> GetTabGroups();
        Task UpdateTabGroups(IEnumerable<TabGroup> tabGroups);
    }
}
