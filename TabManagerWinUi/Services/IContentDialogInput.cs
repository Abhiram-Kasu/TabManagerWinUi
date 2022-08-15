using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabManagerWinUi.Services
{
    internal interface IContentDialogInput
    {
        public Task<string> InputTextDialogAsync(string title);
    }
}
