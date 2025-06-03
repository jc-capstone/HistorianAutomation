using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class ImportExcelGrid : PageBase
    {
        public ImportExcelGrid(IPage basePage) : base(basePage) { }

        public ILocator GridRows => basePage.Locator("(//div[contains(@class,'k-grid-container')]//tr)[1]");
    }
}
