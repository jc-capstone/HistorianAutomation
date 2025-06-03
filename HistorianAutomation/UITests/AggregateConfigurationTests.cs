using HistorianAutomation.Framework;
using HistorianUIAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HistorianUIAutomation.Tests
{
    public class AggregateConfigurationTests : BaseTest
    {
        [Test]
        public async Task AddAggregateConfiguration()
        {
           await TestUtilities.NavigateToHistorian();

            await Pages.HomePage.ConfigurationTabByName("Aggregates").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            await Pages.AggregateConfiguration.AddNewButton.ClickAsync();
            await Interaction.AccessTable(1, 1).ClickAsync();
            var name = Guid.NewGuid().ToString().Substring(0,9);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.AggregateConfiguration.Name), name );
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.AggregateConfiguration.SaveChangesButton.ClickAsync();

            var gridCountAfterSave = await Interaction.GetGridCount();

            Assert.IsTrue(gridCountAfterSave > originalGridCount);
            await Interaction.DeleteRowByName(name);
        }

        [Test]
        public async Task EditAggregateConfiguration()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Aggregates").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            await Pages.AggregateConfiguration.AddNewButton.ClickAsync();
            await Interaction.AccessTable(1, 1).ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 9);

            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.AggregateConfiguration.Name), name);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.AggregateConfiguration.SaveChangesButton.ClickAsync();

            var description = "TEST" + name;
            await Interaction.SetTextForTd(Interaction.AccessTable(name, Pages.AggregateConfiguration.Description), description, true);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.AggregateConfiguration.SaveChangesButton.ClickAsync();

            var text = await Interaction.AccessTable(name, Pages.AggregateConfiguration.Description).TextContentAsync();

            Assert.IsTrue(text == description);
            await Interaction.DeleteRowByName(name);
        }

        [Test]
        public async Task DeleteAggregateConfiguration()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Aggregates").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 9);

            await Pages.AggregateConfiguration.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.AggregateConfiguration.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.AggregateConfiguration.SaveChangesButton.ClickAsync();

            var gridCountAfterAdd = await Interaction.GetGridCount(); ;

            await Interaction.DeleteRowByName(testName);

            var gridCountAfterDelete = await Interaction.GetGridCount(); ;

            Assert.IsTrue(gridCountAfterAdd > originalGridCount);
            Assert.IsTrue(gridCountAfterDelete < gridCountAfterAdd);
        }

        [Test]
        public async Task DownloadTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Aggregates").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 9);

            await Pages.AggregateConfiguration.ExportToExcelButton.ClickAsync();
            var waitForDownloadTask = BasePage.WaitForDownloadAsync();
            await Pages.AggregateConfiguration.ExportToExcelPopupSubmitButton.ClickAsync();

            var dl = await waitForDownloadTask;
            await dl.SaveAsAsync(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + dl.SuggestedFilename);

            var file = Interaction.VerifyFileWasDownloaded("AggregateConfigs");

            Assert.False(String.IsNullOrEmpty(file));

            File.Delete(file);
        }

        [Test]
        public async Task UploadTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Aggregates").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            await Pages.AggregateConfiguration.ImportFromExcelButton.ClickAsync();
            if (await Pages.AggregateConfiguration.UploadExcelInput.IsVisibleAsync()) Assert.Ignore();

            var fileChooser = await BasePage.RunAndWaitForFileChooserAsync(async () =>
            {
                await Pages.AggregateConfiguration.SelectFileButton.ClickAsync();
            });
            await fileChooser.SetFilesAsync(Directory.GetCurrentDirectory() + $"\\TestFiles\\{Pages.AggregateConfiguration.ExcelImportFileName}");

            await Pages.ImportExcelGrid.GridRows.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await Pages.AggregateConfiguration.SubmitImportButton.ClickAsync();

            var gridCountAfterAdd = await Interaction.GetGridCount(); ;

            Assert.IsTrue(gridCountAfterAdd > originalGridCount);

            await Interaction.DeleteRowByName(Pages.AggregateConfiguration.ExcelImportItemName);
        }

        [Test]
        public async Task UrlContainsMultiHistorianStyleGuid()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Aggregates").ClickAsync();
            await BasePage.WaitForURLAsync(url => url.Contains("/aggregates"));
            var urlElements = BasePage.Url.Split("/").ToList();
            var historianGuidString = urlElements[urlElements.IndexOf("aggregates") - 1];
            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Aggregates").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.AggregateConfiguration.TableFilterInput(Pages.AggregateConfiguration.Name).FillAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");
            WaitHelper.WaitFor(500, "Wait for grid to load");
            var origValText = await Pages.AggregateConfiguration.TableFilterInput(Pages.AggregateConfiguration.Name).GetAttributeAsync("value");
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            WaitHelper.WaitFor(500, "Wait for grid to clear");
            var valText = await Pages.FolderPaths.TableFilterInput(Pages.AggregateConfiguration.Name).GetAttributeAsync("value");

            Assert.IsTrue(!String.IsNullOrWhiteSpace(origValText));
            Assert.IsTrue(String.IsNullOrWhiteSpace(valText));
        }

    }
}
