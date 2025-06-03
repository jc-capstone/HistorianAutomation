using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class RealtimeSources : PageBase
    {
        public RealtimeSources(IPage basePage) : base(basePage) { }

        public ILocator RealTimeSourcesTableLastUpdatedData => basePage.Locator($"//tbody//tr//td[13]");
        public ILocator RealTimeSourcesTableRowData => basePage.Locator($"//tbody//tr");
        public ILocator GroupFilterInput => basePage.Locator("//th[@aria-label = 'Filter'][4]//input");
        public ILocator EditSourceButton => basePage.Locator("//voi-button[@label='Edit Source']");
        public ILocator GridItemFromRealTimeSourcesGrid(int index = 1) => basePage.Locator($"//table[contains(@class, 'k-grid-table')]//tbody//tr[{index}]");

        public int RunStatusColumn => 8;
        public int InterfaceColumn => 3;
        public int LastWriteTimeColumn => 14;
    }
}
