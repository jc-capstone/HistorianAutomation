using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class DigitalTypes : PageBase
    {
        public DigitalTypes(IPage basePage) : base(basePage) { }

        public string ExcelImportItemName = "Test22";
        public string ExcelImportFileName = "dataPARC_DigitalTypeMappings_TestData.xlsx";

        public ILocator AddNewButton => basePage.Locator("(//voi-button[@label='Add New'])[1]");
        public ILocator SaveChangesButton => basePage.Locator("(//voi-button[@label = 'Save Changes'])[1]");
        public ILocator ExportToExcelButton => basePage.Locator("(//voi-button[@label='Export to Excel'])[2]").GetByRole(AriaRole.Button);
        public ILocator ExportToExcelPopupSubmitButton => basePage.GetByText("Submit");
        public ILocator DeleteAggregateConfirmationYesButton => basePage.GetByText("Yes");
        public ILocator ImportFromExcelButton => basePage.GetByText("Import from Excel");
        public ILocator SelectFileButton => basePage.GetByText("Select File");
        public ILocator UploadExcelInput => basePage.Locator("//input[@type='file']");
        public ILocator SubmitImportButton => basePage.GetByText("Save Import");
        public ILocator TableFilterInput(int index) => basePage.Locator($"((//td[contains(@aria-label,'Filter')])[{index -1}])/div//div/span/input");

        public ILocator InputTagTypeDropdown => basePage.Locator($"(//*[@class = 'k-input-value-text'])[1]");
        public ILocator ActivityDropdown => basePage.Locator($"(//*[@class = 'k-input-value-text'])[2]");
        public ILocator SaveChangesPopupOkButton => basePage.Locator("//span[@class='k-button-text' and text() = 'OK']/..");
        public ILocator ConfirmDeletePopupButton => basePage.Locator("//span[@class='k-button-text' and text() = 'Yes']/..");
        public ILocator ImportFromExcelButtonDefinitions => basePage.Locator("(//span[text() = 'Import from Excel'])[1]");
        public ILocator ImportFromExcelButtonValues => basePage.Locator("(//span[text() = 'Import from Excel'])[2]");

        #region Value Mappings
        public ILocator ValueMappingAddNewButton => basePage.Locator("(//voi-button[@label='Add New'])[2]");
        public ILocator ValueMappingSaveChangesButton => basePage.Locator("(//voi-button[@label='Save Changes'])[2]");
        public ILocator ValueMappingExportToExcelButton => basePage.Locator("(//voi-button[@label='Export to Excel'])[2]");
        public ILocator ExportToExcelValueMappingsButton => basePage.Locator("(//voi-button[@label='Export to Excel'])[2]");
        #endregion

        #region Digital Types Columns
        public int Name => 2;
        public int InputTagType => 3;
        public int Active => 4;
        public int Delete => 6;
        #endregion

        #region Value Mappings Columns
        public int InputIntValue => 2;
        public int IntegerValue => 3;
        public int TextValue => 4;
        #endregion
    }
}
