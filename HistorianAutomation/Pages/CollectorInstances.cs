using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class CollectorInstances : PageBase
    {
        public CollectorInstances(IPage basePage) : base(basePage) { }

        public ILocator AddNewButton => basePage.Locator("//voi-button[@label='Add New']");
        public ILocator SaveChangesButton => basePage.Locator("//voi-button[@label = 'Save Changes']");
        public ILocator ExportToExcelButton => basePage.GetByText("Export to Excel");
        public ILocator ExportToExcelPopupSubmitButton => basePage.GetByText("Submit");
        public ILocator DeleteAggregateConfirmationYesButton => basePage.GetByText("Yes");
        public ILocator ImportFromExcelButton => basePage.GetByText("Import from Excel");
        public ILocator SelectFileButton => basePage.GetByText("Select File");
        public ILocator UploadExcelInput => basePage.Locator("//input[@type='file']");
        public ILocator SubmitImportButton => basePage.GetByText("Save Import");
        public ILocator TableFilterInput(int index) => basePage.Locator($"((//td[contains(@aria-label,'Filter')])[{index - 1}])/div//div/span/input");
        public ILocator CollectorInstanceILocatorName(string name) => basePage.Locator($"(//td[text()='{name}'])[1]");
        public ILocator CollectorInstanceRows => basePage.Locator("//table[contains(@class,'k-table')]//tr");
        public ILocator CollectorInstanceRealTimeSourcesByIndex(int index) => basePage.Locator($"(//*[text()='Realtime Sources'])[{index}]");


        #region Collector Instance Columns
        public int Instance => 2;
        public int Name => 3;
        public int Description => 4;
        public int Delete => 5;
        #endregion
    }
}
