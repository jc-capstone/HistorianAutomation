using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public  class TagsConfiguration : PageBase
    {
        public TagsConfiguration(IPage basePage) : base(basePage) { }

        public string ExcelImportItemName = "Test22";
        public string ExcelImportFileName = "dataPARC_Tags_TestData.xlsx";

        public ILocator AddNewButton => basePage.Locator("//voi-button[@label='Add New']");
        public ILocator SaveChangesButton => basePage.Locator("//voi-button[@label = 'Save Changes']");
        public ILocator ExportToExcelButton => basePage.GetByText("Export to Excel");
        public ILocator ExportToExcelPopupSubmitButton => basePage.GetByText("Submit");
        public ILocator DeleteAggregateConfirmationYesButton => basePage.GetByText("Yes");
        public ILocator ImportFromExcelButton => basePage.GetByText("Import from Excel");
        public ILocator SelectFileButton => basePage.GetByText("Select File");
        public ILocator UploadExcelInput => basePage.Locator("//input[@type='file']");
        public ILocator SubmitImportButton => basePage.GetByText("Save Import");
        public ILocator TableFilterInput(int index) => basePage.Locator($"((//td[contains(@aria-label,'Filter')])[{index}])/div//div/span/input");
                                                                        //
        
        public ILocator TagNameFilterInput => basePage.Locator("//th[@aria-label='Filter' and @aria-colindex='1']//input");
        public ILocator ConfigurationGroup => basePage.Locator($"(//*[@class = 'k-input-value-text'])[1]");
        public ILocator ConfigurationInterface => basePage.Locator($"(//*[@class = 'k-input-value-text'])[2]");
        //public ILocator SaveChangesPopupOkButton => basePage.Locator("//span[@class='k-button-text' and text() = 'OK']/..");
        //public ILocator ConfirmDeletePopupButton => basePage.Locator("//span[@class='k-button-text' and text() = 'Yes']/..");

        #region Columns
        public int Name => 2;
        public int Active => 3;
        public int Description => 4;
        public int Interface => 5;
        public int PlotMin => 7;
        public int PlotMax => 8;
        public int Deviation => 19;
        public int minDeadBand => 21;
        public int MaxDeadband => 22;
        public int Delete => 36;
        #endregion
    }
}
