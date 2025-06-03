using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class SharedElements : PageBase
    {
        public SharedElements(IPage basePage) : base(basePage) { }

        public ILocator AnimationContainer => basePage.Locator("//div[contains(@class, 'k-animation-container-shown')]");
        public ILocator ManagementPortalHeader => basePage.Locator("//h6[contains(@class, 'MuiTypography-root ') and text() = 'dataPARC Management Portal']");
        public ILocator NoHistorianConnectedMessage => basePage.Locator("//span[text() = 'No Historian(s) Connected']");
        public ILocator ConfigurationHeader => basePage.Locator("(//div[contains(@class, 'MuiButtonBase-root')][1])[1]//div[contains(@class, 'MuiListItemText-root')]");
        public ILocator ClearGridFilterButton => basePage.Locator("(//div[contains(@class,'display-clear-filter-icon')])[1]").GetByRole(AriaRole.Button);


        public ILocator DeleteRecordModelYesButton => basePage.Locator("//div[contains(@role, 'dialog')]//voi-button[@label='Yes']").GetByRole(AriaRole.Button);


    }
}
