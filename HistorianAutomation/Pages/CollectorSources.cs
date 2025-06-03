using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class CollectorSources : PageBase
    {
        public CollectorSources(IPage basePage) : base(basePage) { }

        public ILocator CollectorSourcesILocatorName(string name) => basePage.Locator($"(//input[@value='{name}'])[1]");
        public ILocator CollectorSourcesRows => basePage.Locator("//table[@class='k-grid-table']//tr");
    }
}
