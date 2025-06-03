using HistorianUIAutomation.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistorianAutomation.Framework;

namespace HistorianUIAutomation.Tests
{
    public class CollectorConnectedInstanceTests : BaseTest
    {
        [Test]
        public async Task CollectorInstanceRecievingData()
        {
            await TestUtilities.NavigateToHistorian();
            var collectorInstanceLocator = Pages.CollectorConnectedInstance.GetCollectorInstanceByIndex();
            if (!(await collectorInstanceLocator.IsVisibleAsync()))
            {
                Assert.Ignore("No Collector Instances configured");
            }
            await collectorInstanceLocator.ClickAsync();
            await BasePage.WaitForTimeoutAsync(500);

            await Pages.CollectorConnectedInstance.CollectorInstanceRealtimeSourcesByIndex().ClickAsync();
            var tableDataLocators = Pages.RealtimeSources.RealTimeSourcesTableLastUpdatedData;
            var tableDataElements = await tableDataLocators.AllAsync();
            var tableData = await Task.WhenAll(tableDataElements.Select(async x => await x.InnerTextAsync()));
            var tableDataList = tableData.ToList();

            if (tableDataList.Count == 0)
            {
                Assert.Ignore("No RealTime Sources Configured");
            }

            var itemsRecievingRealTimeData = tableDataList.Where(data =>
            {
                if (string.IsNullOrWhiteSpace(data)) return false;
                if (!DateTime.TryParse(data, out var dt)) return false;
                return dt.AddMinutes(3) > DateTime.Now;
            }).ToList();

            Assert.True(itemsRecievingRealTimeData.Count > 0);
        }

        [Test]
        public async Task EditingCollectorNavigatesToEditScreen()
        {
            await TestUtilities.NavigateToHistorian();
            var collectorInstanceLocator = Pages.CollectorConnectedInstance.GetCollectorInstanceByIndex();
            if (!(await collectorInstanceLocator.IsVisibleAsync()))
            {
                Assert.Ignore("No Collector Instances configured");
            }
            await collectorInstanceLocator.ClickAsync();
            await BasePage.WaitForTimeoutAsync(500);

            await Pages.CollectorConnectedInstance.CollectorInstanceRealtimeSourcesByIndex().ClickAsync();
            var tableDataLocators = Pages.RealtimeSources.RealTimeSourcesTableLastUpdatedData;
            var tableDataElements = await tableDataLocators.AllAsync();
            var tableData = await Task.WhenAll(tableDataElements.Select(async x => await x.InnerTextAsync()));
            var tableDataList = tableData.ToList();

            if (tableDataList.Count == 0)
            {
                Assert.Ignore("No RealTime Sources Configured");
            }

            await Pages.RealtimeSources.GridItemFromRealTimeSourcesGrid().ClickAsync();
            await Pages.RealtimeSources.EditSourceButton.ClickAsync();
            await BasePage.WaitForTimeoutAsync(1000);

            var url = BasePage.Url;
            Assert.True(url.Contains("/Datasources"));
        }
    }
}
