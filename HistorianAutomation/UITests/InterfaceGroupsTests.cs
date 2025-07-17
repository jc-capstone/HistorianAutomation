using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{       
    public class InterfaceGroupsTests : BaseTest
    {
        [Test]
        public async Task AddInterfaceGroup()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Groups").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.InterfaceGroups.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.InterfaceGroups.Name), testName);

            var historyPathCell = Interaction.AccessTable(1, Pages.InterfaceGroups.HistorianPath);
            await historyPathCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.InterfaceGroups.SaveChangesButton.ClickAsync();

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            Assert.IsTrue(gridCountAfterSave > originalGridCount);

            await Interaction.DeleteRowByName(testName);
            if (await Pages.Interfaces.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.Interfaces.DeleteRecordYesButton.ClickAsync();
            }
        }

        [Test]
        public async Task EditInterfaceGroup()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Groups").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.InterfaceGroups.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.InterfaceGroups.Name), testName);

            var historyPathCell = Interaction.AccessTable(1, Pages.InterfaceGroups.HistorianPath);
            await historyPathCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowUp");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.InterfaceGroups.SaveChangesButton.ClickAsync();

            while (await Interaction.GetGridCount() <= originalGridCount)
            {
                await Task.Delay(200);
            }

            var historianPathCell = Interaction.AccessTable(testName, Pages.InterfaceGroups.HistorianPath);
            await historianPathCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.InterfaceGroups.SaveChangesButton.ClickAsync();

            var text = await historianPathCell.TextContentAsync();

            Assert.IsTrue(!string.IsNullOrEmpty(text));
            await Interaction.DeleteRowByName(testName);
        }

        [Test]
        public async Task DeleteAnInterfaceGroup()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Groups").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.InterfaceGroups.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.InterfaceGroups.Name), testName);

            var historyPathCell = Interaction.AccessTable(1, Pages.InterfaceGroups.HistorianPath);
            await historyPathCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowUp");
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.InterfaceGroups.SaveChangesButton.ClickAsync();
            await Task.Delay(1000);
            int gridCountAfterAdd = await Interaction.GetGridCount();

            await Interaction.DeleteRowByName(testName);
            if (await Pages.Interfaces.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.Interfaces.DeleteRecordYesButton.ClickAsync();
            }

            await Task.Delay(1000);
            int gridCountAfterDelete = await Interaction.GetGridCount();


            Assert.IsTrue(gridCountAfterAdd > originalGridCount);
            Assert.IsTrue(gridCountAfterDelete < gridCountAfterAdd);
        }

        [Test]
        public async Task DownloadTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Groups").ClickAsync();

            await Pages.InterfaceGroups.ExportToExcelButton.ClickAsync();
            var waitForDownloadTask = BasePage.WaitForDownloadAsync();
            await Pages.InterfaceGroups.ExportToExcelPopupSubmitButton.ClickAsync();

            var dl = await waitForDownloadTask;
            var filePath = System.IO.Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads", dl.SuggestedFilename);
            await dl.SaveAsAsync(filePath);

            var file = Interaction.VerifyFileWasDownloaded("InterfaceGroups");

            Assert.False(string.IsNullOrEmpty(file));

            System.IO.File.Delete(file);
        }

        [Test]
        public async Task UrlContainsMultiHistorianStyleGuid()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Groups").ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/interfaceGroups"));
            var urlElements = BasePage.Url.Split("/").ToList();
            var historianGuidString = urlElements[urlElements.IndexOf("interfaceGroups") - 1];
            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Groups").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            var filterInput = Pages.InterfaceGroups.TableFilterInput(Pages.InterfaceGroups.Name -1);
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
    }
}
