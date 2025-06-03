using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class HomePage : PageBase
    {
        public HomePage(IPage basePage) : base(basePage) { }

        public ILocator HomeHeaderSelector => basePage.Locator("//span[text() = 'Management Portal']");
        public ILocator ConfigurationTabs => basePage.Locator("//span[contains(@class, 'MuiListItemText-primary')]");
        public ILocator ConfigurationTabByName(String name) => basePage.Locator($"//span[contains(@class, 'MuiListItemText-primary') and text() ='{name}']");
        public ILocator ConfigurationTabByIndex(int index) => basePage.Locator($"(//span[contains(@class, 'MuiListItemText-primary')])[{index}]");
    }
}
