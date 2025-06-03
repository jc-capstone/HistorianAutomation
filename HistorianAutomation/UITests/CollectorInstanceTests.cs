using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HistorianUIAutomation.Tests
{
    public class CollectorInstanceTests : BaseTest
    {
        [Test]
        public async Task CollectorInstanceConfigExists()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Collector Instances").ClickAsync();

            Assert.True(await Pages.CollectorInstances.CollectorInstanceRows.CountAsync() > 0);
        }

        [Test]
        public async Task AddCollectorInstance()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Collector Instances").ClickAsync();

            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            var originalGridCount = await Interaction.GetGridCount();
            await Pages.CollectorInstances.AddNewButton.ClickAsync();

            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.CollectorInstances.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.CollectorInstances.SaveChangesButton.ClickAsync();

            int gridCountAfterAdd = originalGridCount;
            await WaitHelper.WaitForAsync(async () =>
            {
                gridCountAfterAdd = await Interaction.GetGridCount();
                return gridCountAfterAdd > originalGridCount;
            });

            Assert.IsTrue(gridCountAfterAdd > originalGridCount);

            await Pages.CollectorInstances.TableFilterInput(Pages.CollectorInstances.Name).FillAsync(testName);
            await BasePage.Keyboard.PressAsync("Tab");

            await Interaction.DeleteRowByName(testName);
        }

        [Test]
        public async Task EditCollectorInstance()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Collector Instances").ClickAsync();

            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            var originalGridCount = await Interaction.GetGridCount();
            await Pages.CollectorInstances.AddNewButton.ClickAsync();

            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.CollectorInstances.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.CollectorInstances.SaveChangesButton.ClickAsync();

            int gridCountAfterAdd = originalGridCount;
            await WaitHelper.WaitForAsync(async () =>
            {
                gridCountAfterAdd = await Interaction.GetGridCount();
                return gridCountAfterAdd > originalGridCount;
            });

            Assert.IsTrue(gridCountAfterAdd > originalGridCount);

            await Pages.CollectorInstances.TableFilterInput(Pages.CollectorInstances.Name).FillAsync(testName);
            await BasePage.Keyboard.PressAsync("Tab");
            var test = Interaction.AccessTable(testName, Pages.CollectorInstances.Description);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.CollectorInstances.Description), testName, true);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.CollectorInstances.SaveChangesButton.ClickAsync();

            var text = await Interaction.AccessTable(1, Pages.CollectorInstances.Description).InnerTextAsync();

            Assert.IsTrue(text == testName);

            await Interaction.DeleteRowByName(testName);
        }

        [Test]
        public async Task DeleteCollectorInstance()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Collector Instances").ClickAsync();

            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            var originalGridCount = await Interaction.GetGridCount();
            await Pages.CollectorInstances.AddNewButton.ClickAsync();

            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.CollectorInstances.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.CollectorInstances.SaveChangesButton.ClickAsync();

            int gridCountAfterSave = originalGridCount;
            await WaitHelper.WaitForAsync(async () =>
            {
                gridCountAfterSave = await Interaction.GetGridCount();
                return gridCountAfterSave > originalGridCount;
            });

            await Pages.CollectorInstances.TableFilterInput(Pages.CollectorInstances.Name).FillAsync(testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await WaitHelper.WaitForAsync(async () => await Interaction.GetGridCount() < gridCountAfterSave);

            await Interaction.DeleteRowByName(testName, 1);

            int gridCountAfterDelete = gridCountAfterSave;
            await WaitHelper.WaitForAsync(async () =>
            {
                gridCountAfterDelete = await Interaction.GetGridCount();
                return gridCountAfterDelete > gridCountAfterSave;
            });

            Assert.IsTrue(gridCountAfterSave > originalGridCount);
            Assert.IsTrue(gridCountAfterSave > gridCountAfterDelete);
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Collector Instances").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.CollectorInstances.TableFilterInput(Pages.CollectorInstances.Name).FillAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");
            WaitHelper.WaitFor(500, "Wait for grid to load");
            var origValText = await Pages.CollectorInstances.TableFilterInput(Pages.CollectorInstances.Name).GetAttributeAsync("value");
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            WaitHelper.WaitFor(500, "Wait for grid to clear");

            var valText = await Pages.FolderPaths.TableFilterInput(Pages.CollectorInstances.Name).GetAttributeAsync("value");

            Assert.IsTrue(!string.IsNullOrWhiteSpace(origValText));
            Assert.IsTrue(string.IsNullOrWhiteSpace(valText));
        }
    }
}
