using HistorianAutomation.Framework;
using System;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class ServerDetailsTests : BaseTest
    {
        [Test]
        public async Task VerifyHeaders()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Server Details").ClickAsync();

            foreach (var prop in Pages.ServerDetails.Headers)
            {
                Assert.IsTrue(await Pages.ServerDetails.GetTableHeadersByName(prop).IsVisibleAsync());
            }
        }

        [Test]
        public async Task UrlContainsMultiHistorianStyleGuid()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Server Details").ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/serverDetails"));
            var urlElements = BasePage.Url.Split("/").ToList();
            var historianGuidString = urlElements[urlElements.IndexOf("serverDetails") - 1];
            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
        }
    }
}