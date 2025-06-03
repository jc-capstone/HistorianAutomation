using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class Logs : PageBase
    {
        public Logs(IPage basePage) : base(basePage) { }

        public ILocator LogTable => basePage.Locator($"//table[contains(@class, 'k-grid-table')]");
        public ILocator TableFilterInput(int index) => basePage.Locator($"(//td[contains(@aria-label,'Filter')][{index}])//div/div/span/span/input");

        #region Columns
        public int TimeStamp => 2;
        public int Severity => 3;
        public int Message => 4;
        #endregion
    }
}
