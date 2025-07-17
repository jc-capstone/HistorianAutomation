using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class InterfaceTests : BaseTest
    {
        [Test]
        public async Task AddInterface()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interfaces").ClickAsync();

            await Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.Interfaces.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.Interfaces.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.Interfaces.SaveChangesButton.ClickAsync();

            if (await Pages.Interfaces.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.Interfaces.SaveChangesPopupOkButton.ClickAsync();
            }

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            if (await Pages.Interfaces.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.Interfaces.SaveChangesPopupOkButton.ClickAsync();
            }

            Assert.IsTrue(gridCountAfterSave > originalGridCount);

            await Interaction.DeleteRowByName(testName);
        }

        [Test]
        public async Task EditInterface()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interfaces").ClickAsync();

            await Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.Interfaces.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.Interfaces.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.Interfaces.SaveChangesButton.ClickAsync();

            if (await Pages.Interfaces.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.Interfaces.SaveChangesPopupOkButton.ClickAsync();
            }

            while (await Interaction.GetGridCount() <= originalGridCount)
            {
                await Task.Delay(200);
            }

            await Interaction.AccessTable(testName, Pages.Interfaces.HistoryPath).DblClickAsync();
            await BasePage.Keyboard.TypeAsync(testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.Interfaces.SaveChangesButton.ClickAsync();

            if (await Pages.Interfaces.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.Interfaces.SaveChangesPopupOkButton.ClickAsync();
            }

            var text = await Interaction.AccessTable(testName, Pages.Interfaces.HistoryPath).TextContentAsync();

            Assert.IsFalse(string.IsNullOrEmpty(text));
            await Interaction.DeleteRowByName(testName);
            if (await Pages.Interfaces.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.Interfaces.DeleteRecordYesButton.ClickAsync();
            }
        }

        [Test]
        public async Task DeleteAnInterface()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interfaces").ClickAsync();

            await Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.Interfaces.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.Interfaces.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.Interfaces.SaveChangesButton.ClickAsync();

            if (await Pages.Interfaces.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.Interfaces.SaveChangesPopupOkButton.ClickAsync();
            }

            int gridCountAfterAdd = await Interaction.GetGridCount();
            while (gridCountAfterAdd <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterAdd = await Interaction.GetGridCount();
            }

            if (await Pages.Interfaces.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.Interfaces.SaveChangesPopupOkButton.ClickAsync();
            }
            await Interaction.DeleteRowByName(testName);

            if (await Pages.Interfaces.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.Interfaces.DeleteRecordYesButton.ClickAsync();
            }
            int gridCountAfterDelete = await Interaction.GetGridCount();
            while (gridCountAfterDelete >= gridCountAfterAdd)
            {
                await Task.Delay(200);
                gridCountAfterDelete = await Interaction.GetGridCount();
            }

            Assert.IsTrue(gridCountAfterAdd > originalGridCount);
            Assert.IsTrue(gridCountAfterDelete < gridCountAfterAdd);
        }

        [Test]
        public async Task DownloadTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interfaces").ClickAsync();

            await Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup.ClickAsync();
            await Task.Delay(500);
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            await Pages.Interfaces.ExportToExcelButton.ClickAsync();
            var waitForDownloadTask = BasePage.WaitForDownloadAsync();
            await Pages.Interfaces.ExportToExcelPopupSubmitButton.ClickAsync();

            var dl = await waitForDownloadTask;
            var filePath = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads", dl.SuggestedFilename);
            await dl.SaveAsAsync(filePath);

            var file = Interaction.VerifyFileWasDownloaded("Interfaces");

            Assert.False(string.IsNullOrEmpty(file));

            File.Delete(file);
        }

        [Test]
        public async Task UrlContainsMultiHistorianStyleGuid()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interfaces").ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/interfaces"));
            var urlElements = BasePage.Url.Split("/").ToList();
            var historianGuidString = urlElements[urlElements.IndexOf("interfaces") - 1];
            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interfaces").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.Interfaces.TableFilterInput(Pages.Interfaces.Name).FillAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");
            await Task.Delay(500);
            var origValText = await Pages.Interfaces.TableFilterInput(Pages.Interfaces.Name).GetAttributeAsync("value");
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            await Task.Delay(500);
            var valText = await Pages.Interfaces.TableFilterInput(Pages.Interfaces.Name).GetAttributeAsync("value");

            Assert.IsTrue(!string.IsNullOrWhiteSpace(origValText));
            Assert.IsTrue(string.IsNullOrWhiteSpace(valText));
        }

        [Test]
        public async Task UploadTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interfaces").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            await Pages.Interfaces.ImportFromExcelButton.ClickAsync();

            // If the upload input is not visible, skip the test
            if (!await Pages.Interfaces.UploadExcelInput.IsEnabledAsync())
                Assert.Ignore();

            // Handle file chooser and upload
            var fileChooser = await BasePage.RunAndWaitForFileChooserAsync(async () =>
            {
                await Pages.Interfaces.SelectFileButton.ClickAsync();
            });
            await fileChooser.SetFilesAsync(System.IO.Path.Combine(
                Directory.GetCurrentDirectory(), "TestFiles", Pages.Interfaces.ExcelImportFileName));

            // Wait for the import grid to appear
            await Pages.ImportExcelGrid.GridRows.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            if (await Pages.Interfaces.PreviewImportButton.IsVisibleAsync())
            {
                await Pages.Interfaces.PreviewImportButton.ClickAsync();
            }
            await Pages.Interfaces.SubmitImportButton.ClickAsync();

            // Wait for the grid to update
            int gridCountAfterAdd = await Interaction.GetGridCount();
            while (gridCountAfterAdd <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterAdd = await Interaction.GetGridCount();
            }

            Assert.IsTrue(gridCountAfterAdd > originalGridCount);

            // Clean up: delete the imported row
            await Interaction.DeleteRowByName(Pages.Interfaces.ExcelImportItemName);
            if (await Pages.Interfaces.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.Interfaces.DeleteRecordYesButton.ClickAsync();
            }
        }


    }
}

























//using HistorianAutomation.Framework;
//using HistorianUIAutomation.Pages;
//using Microsoft.Playwright;
//using OpenQA.Selenium.Interactions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HistorianUIAutomation.Tests
//{
//    public class InterfaceTests : BaseTest
//    {
//        [Test]
//        public async Task AddInterface()
//        {
//            TestUtilities.NavigateToHistorian();
//            Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Interfaces"));

//            Interaction.ClickElement(Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup);
//            WaitHelper.WaitForElementVisible(Pages.SharedElements.AnimationContainer);
//            new Actions(Driver).KeyDown(Keys.ArrowDown).KeyDown(Keys.Enter).Perform();

//            var originalGridCount = Interaction.GetGridCount();
//            var testName = Guid.NewGuid().ToString().Substring(0, 8);

//            Interaction.ClickElement(Pages.Interfaces.AddNewButton);
//            Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.Interfaces.Name), testName + Keys.Tab);

//            Interaction.ClickElement(Pages.Interfaces.SaveChangesButton);

//            if (Interaction.GetElement(Pages.Interfaces.SaveChangesPopupOkButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.SaveChangesPopupOkButton);
//            }
//            var gridCountAfterSave = originalGridCount;

//            WaitHelper.WaitFor(() =>
//            {
//                gridCountAfterSave = Interaction.GetGridCount();
//                return gridCountAfterSave > originalGridCount;
//            });

//            if (Interaction.GetElement(Pages.Interfaces.SaveChangesPopupOkButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.SaveChangesPopupOkButton);
//            }

//            Assert.IsTrue(gridCountAfterSave > originalGridCount);

//            Interaction.DeleteRowByName(testName);
//        }

//        [Test]
//        public async Task EditInterface()
//        {
//            TestUtilities.NavigateToHistorian();
//            Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Interfaces"));

//            Interaction.ClickElement(Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup);
//            WaitHelper.WaitForElementVisible(Pages.SharedElements.AnimationContainer);
//            new Actions(Driver).KeyDown(Keys.ArrowDown).KeyDown(Keys.Enter).Perform();

//            var originalGridCount = Interaction.GetGridCount();
//            var testName = Guid.NewGuid().ToString().Substring(0, 8);

//            Interaction.ClickElement(Pages.Interfaces.AddNewButton);
//            Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.Interfaces.Name), testName + Keys.Tab);
//            Interaction.ClickElement(Pages.Interfaces.SaveChangesButton);

//            if (Interaction.GetElement(Pages.Interfaces.SaveChangesPopupOkButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.SaveChangesPopupOkButton);
//            }

//            WaitHelper.WaitFor(() => Interaction.GetGridCount() > originalGridCount);

//            var newPath = "TEST";
//            Interaction.ClickElement(Interaction.AccessTable(testName, Pages.Interfaces.HistoryPath));
//            new Actions(Driver).KeyDown(Keys.ArrowDown).KeyDown(Keys.ArrowDown).KeyDown(Keys.Enter).Perform();
//            Interaction.ClickElement(Pages.Interfaces.SaveChangesButton);

//            if (Interaction.GetElement(Pages.Interfaces.SaveChangesPopupOkButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.SaveChangesPopupOkButton);
//            }

//            var text = Interaction.GetElement(Interaction.AccessTable(testName, Pages.Interfaces.HistoryPath)).GetAttribute("innerHTML");

//            Assert.IsFalse(String.IsNullOrEmpty(text));
//            Interaction.DeleteRowByName(testName);
//            if (Interaction.GetElement(Pages.Interfaces.DeleteRecordYesButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.DeleteRecordYesButton);
//            }
//        }

//        [Test]
//        public async Task DeleteAnInterface()
//        {
//            TestUtilities.NavigateToHistorian();
//            Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Interfaces"));

//            Interaction.ClickElement(Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup);

//            WaitHelper.WaitForElementVisible(Pages.SharedElements.AnimationContainer);
//            new Actions(Driver).KeyDown(Keys.ArrowDown).KeyDown(Keys.Enter).Perform();

//            var originalGridCount = Interaction.GetGridCount();
//            var testName = Guid.NewGuid().ToString().Substring(0, 8);

//            Interaction.ClickElement(Pages.Interfaces.AddNewButton);
//            Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.Interfaces.Name), testName + Keys.Tab);
//            Interaction.ClickElement(Pages.Interfaces.SaveChangesButton);

//            if (Interaction.GetElement(Pages.Interfaces.SaveChangesPopupOkButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.SaveChangesPopupOkButton);
//            }

//            var gridCountAfterAdd = originalGridCount;
//            WaitHelper.WaitFor(() =>
//            {
//                gridCountAfterAdd = Interaction.GetGridCount();
//                return gridCountAfterAdd > originalGridCount;
//            });

//            if (Interaction.GetElement(Pages.Interfaces.SaveChangesPopupOkButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.SaveChangesPopupOkButton);
//            }
//            Interaction.DeleteRowByName(testName);

//            if (Interaction.GetElement(Pages.Interfaces.DeleteRecordYesButton) != null)
//            {
//                Interaction.ClickElement(Pages.Interfaces.DeleteRecordYesButton);
//            }
//            var gridCountAfterDelete = gridCountAfterAdd;
//            WaitHelper.WaitFor(() =>
//            {
//                gridCountAfterDelete = Interaction.GetGridCount();
//                return gridCountAfterDelete > gridCountAfterAdd;
//            });

//            Assert.IsTrue(gridCountAfterAdd > originalGridCount);
//            Assert.IsTrue(gridCountAfterDelete < gridCountAfterAdd);
//        }

//        [Test]
//        public async Task DownloadTableInformation()
//        {
//            TestUtilities.NavigateToHistorian();
//            Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Interfaces"));

//            Interaction.ClickElement(Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup);
//            WaitHelper.WaitFor(500, "TableLoad");
//            //WaitHelper.WaitForElementVisible(Pages.SharedElements.AnimationContainer);
//            new Actions(Driver).KeyDown(Keys.ArrowDown).KeyDown(Keys.Enter).Perform();

//            var originalGridCount = Interaction.GetGridCount();
//            var testName = Guid.NewGuid().ToString().Substring(0, 8);

//            Interaction.ClickElement(Pages.Interfaces.ExportToExcelButton);
//            Interaction.ClickElement(Pages.Interfaces.ExportToExcelPopupSubmitButton);

//            WaitHelper.WaitFor(() => !String.IsNullOrEmpty(Interaction.VerifyFileWasDownloaded("Interfaces")));

//            var file = Interaction.VerifyFileWasDownloaded("Interfaces");

//            Assert.False(String.IsNullOrEmpty(file));

//            File.Delete(file);
//        }

//        [Test]
//        public async Task UrlContainsMultiHistorianStyleGuid()
//        {
//            TestUtilities.NavigateToHistorian();
//            Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Interfaces"));

//            WaitHelper.WaitFor(() => Driver.Url.Contains("/interfaces"));
//            var urlElements = Driver.Url.Split("/").ToList();
//            var historianGuidString = urlElements[urlElements.IndexOf("interfaces") - 1];
//            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
//        }

//        [Test]
//        public async Task EnsureGridFiltersProperlyClear()
//        {
//            TestUtilities.NavigateToHistorian();
//            Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Interfaces"));

//            var name = Guid.NewGuid().ToString().Substring(0, 8);

//            Interaction.GetElement(Pages.Interfaces.TableFilterInput(Pages.Interfaces.Name)).SendKeys(name + Keys.Tab);
//            WaitHelper.WaitFor(500, "Wait for grid to load");
//            var origValText = Interaction.GetElement(Pages.Interfaces.TableFilterInput(Pages.Interfaces.Name)).GetAttribute("value");
//            Interaction.ClickElement(SharedElements.ClearGridFilterButton);
//            WaitHelper.WaitFor(500, "Wait for grid to clear");
//            var valText = Interaction.GetElement(Pages.Interfaces.TableFilterInput(Pages.Interfaces.Name)).GetAttribute("value");

//            Assert.IsTrue(!String.IsNullOrWhiteSpace(origValText));
//            Assert.IsTrue(String.IsNullOrWhiteSpace(valText));
//        }
//        //[Test]
//        //public async Task UploadTableInformation()
//        //{
//        //    TestUtilities.NavigateToHistorian();
//        //    Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Interfaces"));
//        //    var originalGridCount = Interaction.GetGridCount();
//        //    var testName = Guid.NewGuid().ToString().Substring(0, 8);
//        //    Interaction.ClickElement(Pages.Interfaces.ImportFromExcelButton);

//        //    if (Interaction.GetElement(Pages.Interfaces.UploadExcelInput) == null) Assert.Ignore();

//        //    Interaction.InputText(Pages.Interfaces.UploadExcelInput, Directory.GetCurrentDirectory() + $"\\TestFiles\\{Pages.Interfaces.ExcelImportFileName}", false);
//        //    WaitHelper.WaitFor(() => Interaction.GetElements(Pages.ImportExcelGrid.GridRows).Count > 0);
//        //    Interaction.ClickElement(Pages.Interfaces.SubmitImportButton);
//        //    WaitHelper.WaitFor(() => Interaction.GetGridCount() > originalGridCount);

//        //    var gridCountAfterAdd = Interaction.GetGridCount();
//        //    Assert.IsTrue(gridCountAfterAdd >= originalGridCount);

//        //    Interaction.DeleteRowByName(Pages.Interfaces.ExcelImportItemName);
//        //}
//    }


//}
