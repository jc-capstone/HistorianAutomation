using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class InterfaceSets : PageBase
    {
        public InterfaceSets(IPage basePage) : base(basePage) { }


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


        public ILocator SaveChangesPopupOkButton => basePage.Locator("//span[@class='k-button-text' and text() = 'OK']/..");
        public ILocator DeleteRecordYesButton => basePage.GetByText("Yes");
        public ILocator InterfaceGroupDropdownButton => basePage.Locator("//span[contains(@class, 'k-dropdownlist')]//button");
        public ILocator GetInterfacesEditButtonILocatorIndex(int count = 1) => basePage.Locator($"(//span[contains(@class,'k-i-window k-button-icon')])[{count}]");
        public ILocator InterfaceSetInterfaceSelectionDialog => basePage.Locator("//span[text() = 'Interface Set Interface Selection']/../ancestor::div[contains(@class, 'k-window windowDialog')]");
        public ILocator InterfaceEditCheckboxesILocatorIndex(int count = 1) => basePage.Locator($"(//span[text() = 'Interface Set Interface Selection']/../ancestor::div[contains(@class, 'k-window windowDialog')]//input[@type='checkbox'])[{count}]");
        public ILocator InterfaceEditCheckboxes => basePage.Locator($"//span[text() = 'Interface Set Interface Selection']/../ancestor::div[contains(@class, 'k-window windowDialog')]//input[@type='checkbox']");
        public ILocator InterfaceEditSaveButton => basePage.Locator($"//div[contains(@class, 'k-window windowDialog')]//span[text() = 'Save Changes']/..");
        public ILocator InterfaceEditCloseButton => basePage.Locator($"//div[contains(@class, 'k-window windowDialog')]//span[text() = 'Close']/..");

        #region Columns
        public int Name => 2;
        public int InterfaceGroup => 3;
        public int Scope => 4;
        public int Interfaces => 5;
        public int Active => 6;
        #endregion
    }
}
