using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class FolderPaths : PageBase
    {
        public FolderPaths(IPage basePage) : base(basePage) { }

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
        public ILocator SaveChangesPopupOkButton => basePage.Locator("//span[@class='k-button-text' and text() = 'OK']/..");
        public ILocator DeleteRecordYesButton => basePage.GetByText("Yes");


        #region Columns
        public int Name => 2;
        public int FolderPath => 3;
        public int Description => 4;
        public int Delete => 5;
        #endregion
    }
}
