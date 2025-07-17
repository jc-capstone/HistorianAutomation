using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.IO;
using System.IO;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HistorianUIAutomation.Tests
{
    public class DigitalTypesTests : BaseTest
    {
        [Test]
        public async Task AddDigitalType()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();
                            
            await Pages.DigitalTypes.AddNewButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.DigitalTypes.AddNewButton.ClickAsync();

            var x = Interaction.AccessTable(1, Pages.DigitalTypes.Name);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.DigitalTypes.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await BasePage.Keyboard.PressAsync("Tab");

            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.DigitalTypes.SaveChangesButton.ClickAsync();

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            Assert.IsTrue(gridCountAfterSave > originalGridCount);
            await Interaction.DeleteRowByName(testName);
        }

        [Test]
        public async Task EditDigitalType()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();

            await Pages.DigitalTypes.AddNewButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.DigitalTypes.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.DigitalTypes.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await BasePage.Keyboard.PressAsync("Tab");

            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.DigitalTypes.SaveChangesButton.ClickAsync();

            if (await Pages.DigitalTypes.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.DigitalTypes.SaveChangesPopupOkButton.ClickAsync();
            }

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            if (await Pages.DigitalTypes.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.DigitalTypes.SaveChangesPopupOkButton.ClickAsync();
            }

            while (await Interaction.GetGridCount() <= originalGridCount)
            {
                await Task.Delay(200);
            }

            var description = "TEST" + Guid.NewGuid().ToString().Substring(0, 8);
            var test = Interaction.AccessTable(testName, Pages.DigitalTypes.Name);

            await Interaction.SetTextForTd(Interaction.AccessTableWithSpan(testName, Pages.DigitalTypes.Name), description, true);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.DigitalTypes.SaveChangesButton.ClickAsync();

            if (await Pages.DigitalTypes.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.DigitalTypes.SaveChangesPopupOkButton.ClickAsync();
            }
            var text = await Interaction.AccessTableWithSpan(description, Pages.DigitalTypes.Name).InnerTextAsync();

            Assert.IsTrue(text == description);
            await Interaction.DeleteRowByName(description);
        }

        [Test]
        public async Task DownloadTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();

            await Pages.DigitalTypes.ExportToExcelButton.ClickAsync();
            var waitForDownloadTask = BasePage.WaitForDownloadAsync();
            await Pages.DigitalTypes.ExportToExcelPopupSubmitButton.ClickAsync();

            var dl = await waitForDownloadTask;
            var filePath = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads", dl.SuggestedFilename);
            await dl.SaveAsAsync(filePath);

            var file = Interaction.VerifyFileWasDownloaded("DigitalTypeMapping");

            Assert.False(string.IsNullOrEmpty(file));
            Assert.False(string.IsNullOrEmpty(file));

            File.Delete(file);
        }

        [Test]
        public async Task AddValueMapping()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();

            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Interaction.AccessTable(1, Pages.DigitalTypes.Name).ClickAsync();
            var originalGridCount = await Interaction.GetGridCount(2);
            await Pages.DigitalTypes.ValueMappingAddNewButton.ClickAsync();

            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.DigitalTypes.Name, 2), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.DigitalTypes.ValueMappingSaveChangesButton.ClickAsync();

            if (await Pages.DigitalTypes.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.DigitalTypes.SaveChangesPopupOkButton.ClickAsync();
            }
            await Task.Delay(1000);
            int gridCountAfterSave = await Interaction.GetGridCount(2);

            if (await Pages.DigitalTypes.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.DigitalTypes.SaveChangesPopupOkButton.ClickAsync();
            }
            Assert.IsTrue(gridCountAfterSave > originalGridCount);

            //Add delete
        }

        [Test]
        public async Task EditValueMapping()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();

            var testName = new Random().Next(1000000, 9999999).ToString();

            await Interaction.AccessTable(2, Pages.DigitalTypes.Name).ClickAsync();
            var originalGridCount = await Interaction.GetGridCount(2);
            await Pages.DigitalTypes.ValueMappingAddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.DigitalTypes.Name, 2), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.DigitalTypes.ValueMappingSaveChangesButton.ClickAsync();

            int gridCountAfterSave = await Interaction.GetGridCount(2);
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount(2);
            }

            var newName = testName + "2";
            await Interaction.SetTextForTd(Interaction.AccessTableWithSpan(testName, Pages.DigitalTypes.IntegerValue, 2), newName, true);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.DigitalTypes.ValueMappingSaveChangesButton.ClickAsync();

            var text = await Interaction.AccessTableWithSpan(newName, Pages.DigitalTypes.IntegerValue, 2).TextContentAsync();

            Assert.IsTrue(text == newName);
        }

        [Test]
        public async Task DownloadValueMappingsTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();

            await Pages.DigitalTypes.ExportToExcelValueMappingsButton.ClickAsync();
            var waitForDownloadTask = BasePage.WaitForDownloadAsync();
            await Pages.DigitalTypes.ExportToExcelPopupSubmitButton.ClickAsync();

            var dl = await waitForDownloadTask;
            var filePath = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads", dl.SuggestedFilename);
            await dl.SaveAsAsync(filePath);

            var file = Interaction.VerifyFileWasDownloaded("DigitalTypeMappingDetails");

            Assert.False(string.IsNullOrEmpty(file));

            File.Delete(file);
        }

        [Test]
        public async Task UrlContainsMultiHistorianStyleGuid()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/digitalTypes"));
            var urlElements = BasePage.Url.Split("/").ToList();
            var historianGuidString = urlElements[urlElements.IndexOf("digitalTypes") - 1];
            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Digital Types").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            var filterInput = Pages.DigitalTypes.TableFilterInput(Pages.DigitalTypes.Name);
            await filterInput.FillAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");
            await Task.Delay(500);
            var origValText = await filterInput.GetAttributeAsync("value");
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            await Task.Delay(500);
            var valText = await filterInput.GetAttributeAsync("value");

            Assert.IsTrue(!string.IsNullOrWhiteSpace(origValText));
            Assert.IsTrue(string.IsNullOrWhiteSpace(valText));
        }

        //[Test]
        //public async Task UploadTableInformationDefinitions()
        //{
        //    TestUtilities.NavigateToHistorian();
        //    Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Digital Types"));
        //    var testName = Guid.NewGuid().ToString().Substring(0, 8);

        //    var originalGridCount = Interaction.GetGridCount();
        //    Interaction.ClickElement(Pages.DigitalTypes.ImportFromExcelButtonDefinitions);
        //    Interaction.InputText(Pages.DigitalTypes.UploadExcelInput, Directory.GetCurrentDirectory() + $"\\TestFiles\\{Pages.DigitalTypes.ExcelImportFileName}", false);
        //    WaitFor(500, "");
        //    Interaction.ClickElement(Pages.DigitalTypes.SubmitImportButton);
        //    WaitHelper.WaitFor(() => Interaction.GetGridCount() > originalGridCount);
        //    var gridCountAfterAdd = Interaction.GetGridCount();
        //    Assert.IsTrue(gridCountAfterAdd > originalGridCount);
        //}

        //[Test]
        //public async Task UploadTableInformationValues()
        //{
        //    TestUtilities.NavigateToHistorian();
        //    Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Digital Types"));
        //    var testName = Guid.NewGuid().ToString().Substring(0, 8);

        //    var originalGridCount = Interaction.GetGridCount();
        //    Interaction.ClickElement(Pages.DigitalTypes.ImportFromExcelButtonDefinitions);
        //    Interaction.InputText(Pages.DigitalTypes.UploadExcelInput, Directory.GetCurrentDirectory() + $"\\TestFiles\\{Pages.DigitalTypes.ExcelImportFileName}", false);
        //    WaitFor(500, "");
        //    Interaction.ClickElement(Pages.DigitalTypes.SubmitImportButton);
        //    WaitHelper.WaitFor(() => Interaction.GetGridCount() > originalGridCount);
        //    var gridCountAfterAdd = Interaction.GetGridCount();
        //    Assert.IsTrue(gridCountAfterAdd > originalGridCount);
        //}
    }
}
