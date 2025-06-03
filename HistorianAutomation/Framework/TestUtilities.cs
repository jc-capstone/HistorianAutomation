using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HistorianAutomation.Framework
{
    public class TestUtilities
    {
        private Dictionary<string, string> config;
        public IPage BasePage { get; set; }
        public HistorianUIAutomation.Pages.Pages Pages { get; set; }
        public TestUtilities(Dictionary<string, string> _config, IPage basePage, Pages pages)
        {
            config = _config;
            BasePage = basePage;
            Pages = pages;
        }

        public int HistorianIdUrlIndex = 4;

        public async Task NavigateToHistorian()
        {
            var env = config.GetValueOrDefault("env");
            var envUrl = config.GetValueOrDefault(env);
            await BasePage.GotoAsync(envUrl);

            await Pages.SharedElements.ConfigurationHeader.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            ILocator openIcon = BasePage.Locator("(//span[text() = 'Configuration']/..)[1]");

            if (await openIcon.IsVisibleAsync())
            {
                await openIcon.ClickAsync();
            }
        }

        public bool UrlContainsHistorianId(string url, string id)
        {
            return url.Contains(id);
        }

        public bool NumberMatchesLocalizedFormat(string input)
        {
            Regex regex = new Regex(@"-?\d{1,3}(?:\.\d{3})*(?:,\d+)?");
            Match match = regex.Match(input);
            Decimal result = Decimal.Zero;
            if (match.Success)
                result = Decimal.Parse(match.Value, GetCulture());
            return match.Success;
        }

        public CultureInfo GetCulture()
        {
            if (String.IsNullOrEmpty(config.GetValueOrDefault("Localization")))
            {
                return CultureMapping.GetValueOrDefault("en");
            }
            return CultureMapping.GetValueOrDefault(config.GetValueOrDefault("Localization"));
        }

        private Dictionary<string, CultureInfo> CultureMapping = new Dictionary<string, CultureInfo>()
        {
            { "en" , new CultureInfo("en-US") },
            { "de" , new CultureInfo("de-DE") },
            { "ja" , new CultureInfo("ja-JP") },
        };
    }
}
