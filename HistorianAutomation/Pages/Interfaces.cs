using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class Interfaces : PageBase
    {
        public Interfaces(IPage basePage) : base(basePage) { }

        public string ExcelImportItemName = "randomTest";
        public string ExcelImportFileName = "dataPARC_Interfaces_TestData.xlsx";


        public ILocator AddNewButton => basePage.Locator("//voi-button[@label='Add New']");
        public ILocator SaveChangesButton => basePage.Locator("//voi-button[@label = 'Save Changes']");
        public ILocator ExportToExcelButton => basePage.GetByText("Export to Excel");
        public ILocator ExportToExcelPopupSubmitButton => basePage.GetByText("Submit");
        public ILocator DeleteAggregateConfirmationYesButton => basePage.GetByText("Yes");
        public ILocator ImportFromExcelButton => basePage.GetByText("Import from Excel");
        public ILocator SelectFileButton => basePage.GetByText("Select File");
        public ILocator UploadExcelInput => basePage.Locator("//input[@type='file']");
        public ILocator PreviewImportButton => basePage.GetByText("Preview Import");
        public ILocator SubmitImportButton => basePage.GetByText("Save Import");
        public ILocator SaveChangesPopupOkButton => basePage.Locator("//span[@class='k-button-text' and text() = 'OK']/..");
        public ILocator DeleteRecordYesButton => basePage.GetByText("Yes");
        public ILocator TableFilterInput(int index) => basePage.Locator($"((//td[contains(@aria-label,'Filter')])[{index}])/div//div/span/input");
        public ILocator InterfaceGroupDropdownButton => basePage.Locator("//span[contains(@class, 'k-dropdownlist')]//button");

        #region Columns
        //public int Edit => 1;
        public int Name => 2;
        public int InterfaceGroup => 3;
        public int HistoryPath => 4;
        public int ArchivePath => 5;
        public int TimeZone => 6;
        public int FileType => 7;
        public int Active => 8;
        public int Delete => 14;
        #endregion
    }
}
