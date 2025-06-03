using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class RealTimeSourcesTest : BaseTest
    {
        [Test]
        public async Task RealTimeSourcesExist()
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
    }
}