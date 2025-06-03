using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class CollectorSourceConfiguration : PageBase
    {
        public CollectorSourceConfiguration(IPage basePage) : base(basePage) { }

        public readonly string OpcLocalHostUrl = "opcda://vvq-dataserver.parcqa.com/HistorianPlayback.Paper.1";
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

        public ILocator CollectorSourceConfigurationGroup => basePage.Locator($"(//*[@class = 'k-input-value-text'])[1]/../..");
        public ILocator CollectorSourceConfigurationInterface => basePage.Locator($"//span[contains(@class,'k-dropdownlist')][2]");
        public ILocator CollectorSourceConfigurationDatasourceType => basePage.Locator($"(//*[@class = 'k-input-value-text'])[3]");
        public ILocator CollectorInstanceIdInput => basePage.Locator($"(//td[text() = 'Collector Instance Id']/following::input)[1]");
        public ILocator AddNewOpcDaServerButton => basePage.Locator($"//span[text() = 'Add New']");
        public ILocator OpcDaServerUrlInput => basePage.Locator($"(//td[text() = 'Opc Da Server(s)']/following-sibling::td[1]//input)[2]");
        public ILocator CancelChangesButton => basePage.Locator($"//span[text() = 'Cancel Changes']/..");
        public ILocator WarningDialog => basePage.Locator($"//div[contains(@class, 'k-window-title') and text() = 'Warning']");
        public ILocator WarningDialogYesButton => basePage.GetByText("Yes");
        public ILocator WarningDialogNoButton => basePage.Locator($"//div[contains(@class, 'k-dialog-content')]//*[text()='No']");

        #region Columns
        public int Instance => 2;
        #endregion
    }
}
