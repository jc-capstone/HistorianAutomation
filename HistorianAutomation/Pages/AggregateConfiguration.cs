
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class AggregateConfiguration : PageBase
    {
        public AggregateConfiguration(IPage basePage) : base(basePage) { }

        public string ExcelImportItemName = "TEST";
        public string ExcelImportFileName = "dataPARC_AggregateConfigs_TestData.xlsx";

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


        #region Columns
        public int Name => 2;
        public int Description => 3;
        public int AggregateType => 4;
        public int ProcessingInterval => 4;
        public int TreatUncertainAsBad => 5;
        public int UseUTC => 6;
        public int Alignment => 7;
        public int InterfacesAssignment => 8;
        public int Active => 9;
        public int Delete => 10 ;
        #endregion
    }
}
