using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class CollectorConnectedInstance : PageBase
    {
        public CollectorConnectedInstance(IPage basePage) : base(basePage) { }

        public ILocator GetCollectorInstanceByIndex(int index = 1) => basePage.Locator($"(//*[@data-testid = 'ChevronRightIcon'])[{index}]/..");
        public ILocator CollectorInstanceRealtimeSourcesByIndex() => basePage.Locator($"//span[text() = 'Realtime Sources']/ancestor::a");

    }
}
