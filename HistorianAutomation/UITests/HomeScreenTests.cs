using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using NUnit.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class HomeScreenTests : BaseTest
    {
        [Test]
        public async Task VerifyHeaderText()
        {
            await TestUtilities.NavigateToHistorian();
            var header = Pages.HomePage.HomeHeaderSelector;
            Assert.IsTrue(await header.IsVisibleAsync());
        }

        [TestCase]
        public async Task CheckTabs()
        {
            var tabList = new List<string> { "Folder Paths", "Interface Groups", "Interfaces", "Interface Sets", "Tags", "Digital Types", "Aggregates",
                "Aggregate Interfaces", "Aggregate Monitor", "Aggregate Job Monitor", "Collector Instances", "Collector Sources", "Server Details", "Logs"};

            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabs.First.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            var tabs = await Pages.HomePage.ConfigurationTabs.AllAsync();
            var tabTexts = await Task.WhenAll(tabs.Select(async t => await t.InnerTextAsync()));

            foreach (var x in tabList)
            {
                Assert.IsTrue(tabTexts.Any(tab => tab == x));
            }
        }
    }
}
