using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class LogTests : BaseTest
    {
        [Test]
        public async Task ValidateTableExists()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Logs").ClickAsync();

            Assert.IsTrue(await Pages.Logs.LogTable.IsVisibleAsync());
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Logs").ClickAsync();

            var name = new Random().Next(10000000, 99999999).ToString();
            var filterInput = Pages.Logs.TableFilterInput(Pages.Logs.TimeStamp -1);

            await filterInput.ClickAsync();
            await BasePage.Keyboard.PressAsync("Control+A");
            await filterInput.PressSequentiallyAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");

            var origValText = await filterInput.GetAttributeAsync("value");
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            await Task.Delay(500);
            var valText = await filterInput.GetAttributeAsync("value");

            Assert.IsTrue(!string.IsNullOrWhiteSpace(origValText));
            Assert.IsTrue(string.IsNullOrWhiteSpace(valText?.Replace("month/day/year", "")));

        }
    }
}
