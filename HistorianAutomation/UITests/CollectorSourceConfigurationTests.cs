using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class CollectorSourceConfigurationTests : BaseTest
    {
        [Test]
        public async Task VerifyCollectorIsGettingRealtimeData()
        {
            await TestUtilities.NavigateToHistorian();
            if (!await Pages.CollectorConnectedInstance.CollectorInstanceRealtimeSourcesByIndex().IsVisibleAsync())
            {
                Assert.Ignore("No Collector Instances configured");
            }

            await Pages.CollectorInstances.CollectorInstanceRealTimeSourcesByIndex(1).ClickAsync();
            await Pages.RealtimeSources.RealTimeSourcesTableRowData.First.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            var rowData = await Pages.RealtimeSources.RealTimeSourcesTableRowData.AllAsync();

            if (rowData.Count == 0)
            {
                Assert.Ignore("No RealTime Sources Configured");
            }

            var realTimeDataRow = new List<ILocator>();
            for (int index = 0; index < rowData.Count; index++)
            {
                var runStatus = await Interaction.AccessTable(index + 1, Pages.RealtimeSources.RunStatusColumn).InnerTextAsync();
                var lastWriteTime = await Interaction.AccessTable(index + 1, Pages.RealtimeSources.LastWriteTimeColumn).InnerTextAsync();

                if (string.IsNullOrWhiteSpace(lastWriteTime)) continue;

                if (runStatus == "Running" &&
                    DateTime.Parse(lastWriteTime.Replace(" ", "")).AddMinutes(3) >= DateTime.Now)
                {
                    realTimeDataRow.Add(rowData[index]);
                }
            }

            Assert.IsTrue(realTimeDataRow.Count > 0);
        }

        //[Test]
        public async Task AddCollectorSource()
        {
            await TestUtilities.NavigateToHistorian();
            await Task.Delay(5000); // For UI refresh after click

            await Pages.HomePage.ConfigurationTabByName("Collector Sources").ClickAsync();

            await Pages.CollectorSourceConfiguration.AddNewButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            var originalGridCount = await Interaction.GetGridCount();
            await Pages.CollectorSourceConfiguration.AddNewButton.ClickAsync();

            await Pages.CollectorSourceConfiguration.CollectorSourceConfigurationGroup.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            await Pages.CollectorSourceConfiguration.CollectorSourceConfigurationInterface.ClickAsync();
            // Optionally wait for animation if needed
            // await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            await Pages.CollectorSourceConfiguration.CollectorSourceConfigurationDatasourceType.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            var instanceName = new Random().Next(10000000, 99999999).ToString();
            await Pages.CollectorSourceConfiguration.CollectorInstanceIdInput.FillAsync(instanceName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.CollectorSourceConfiguration.AddNewOpcDaServerButton.ClickAsync();

            await Pages.CollectorSourceConfiguration.SaveChangesButton.ClickAsync();

            if (await Pages.CollectorSourceConfiguration.WarningDialogYesButton.IsVisibleAsync())
            {
                await Pages.CollectorSourceConfiguration.WarningDialogYesButton.ClickAsync();
            }

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            Assert.That(gridCountAfterSave > originalGridCount);

            var filterInput = Pages.CollectorSourceConfiguration.TableFilterInput(Pages.CollectorSourceConfiguration.Instance);
            await filterInput.FillAsync(instanceName);
            await BasePage.Keyboard.PressAsync("Tab");

            await Interaction.DeleteRowByName(instanceName);
            if (await Pages.CollectorSourceConfiguration.WarningDialogYesButton.IsVisibleAsync())
            {
                await Pages.CollectorSourceConfiguration.WarningDialogYesButton.ClickAsync();
            }
        }

        [Test]
        public async Task UrlContainsMultiHistorianStyleGuid()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Collector Sources").ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/Datasources"));
            var urlElements = BasePage.Url.Split("/").ToList();
            var historianGuidString = urlElements[urlElements.IndexOf("Datasources") - 1];
            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Collector Sources").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            var filterInput = Pages.CollectorSourceConfiguration.TableFilterInput(Pages.CollectorSourceConfiguration.Instance);
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
