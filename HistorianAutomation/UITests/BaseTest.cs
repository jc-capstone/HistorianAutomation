using HistorianAutomation;
using HistorianAutomation.Framework;
using HistorianUIAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;


namespace HistorianUIAutomation.Tests
{
    [Parallelizable(ParallelScope.Self)]
    public class BaseTest
    {
        public TestUtilities TestUtilities { get; set; }
        public Dictionary<String, String> Configuration { get; set; }
        public HistorianUIAutomation.Pages.Pages Pages { get; set; }
        protected IBrowserContext Browser { get; private set; }
        protected IPage BasePage { get; set; }
        public Interaction Interaction { get; set; }

        public BaseTest()
        {

        }

        [SetUp]
        public async Task StartTest()
        {
            Configuration = ConfigurationProvider.GetConfiguration();

            BrowserNewContextOptions contextOptions = new()
            {
                //BaseURL = "",
                ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 },
                ScreenSize = new ScreenSize() { Width = 1920, Height = 1080 },
                IgnoreHTTPSErrors = true
            };
            var playwright = await Playwright.CreateAsync();
            playwright.Selectors.SetTestIdAttribute("id");
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true, SlowMo = 250, DownloadsPath = (System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads") });
            var context = await browser.NewContextAsync(contextOptions);
            BasePage = await context.NewPageAsync();
            Pages = new Pages.Pages(BasePage);
            Interaction = new Interaction(BasePage, Pages);
            TestUtilities = new TestUtilities(Configuration, BasePage, Pages);
        }

        [TearDown]
        public async Task EndTest()
        {
            if (BasePage != null)
            {
                await BasePage.CloseAsync();
            }
            if (Browser != null)
            {
                await Browser.CloseAsync();
            }
        }
    }
}
