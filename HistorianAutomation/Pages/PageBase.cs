
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class PageBase
    {
        internal IPage basePage;
        public PageBase(IPage page)
        {
            basePage = page;
        }
    }
}
