                            using HistorianAutomation.Framework;
using Microsoft.Playwright;
using HistorianUIAutomation.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Framework
{
    public class Interaction
    {
        public IPage _basePage { get; set; }
        public HistorianUIAutomation.Pages.Pages _pages { get; set; }
        public Interaction(IPage basePage, HistorianUIAutomation.Pages.Pages pages)
        {
            _basePage = basePage;
            _pages = pages;
        }

        public ILocator GridPager(int tableIndex = 1) =>
            _basePage.Locator($"(//span[contains(@class,'k-pager-info')])[{tableIndex}]");

        public ILocator AccessTable(int row, int column, int tableIndex = 1) =>
            _basePage.Locator($"(((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}]");

        public ILocator AccessTableInput(int row, int column, int tableIndex = 1) =>
            _basePage.Locator($"((((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}])//input");

        public ILocator AccessTable(string rowName, int column, int tableIndex = 1) =>
            _basePage.Locator($"((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr//*[text() ='{rowName}' or @value='{rowName}'])/ancestor::tr//td[{column}]");

        public ILocator AccessTableWithSpan(string rowName, int column, int tableIndex = 1) =>
            _basePage.Locator($"(((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr//*[text() ='{rowName}' or @value = '{rowName}' or @value = ''])/../..//td[{column}])[1]");

        public ILocator AccessTableForDropDown(int row, int column, int tableIndex = 1) =>
            _basePage.Locator($"((((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}]).//span[1]");

        public ILocator AccessTableForDropDownClickDropDown(int row, int column, int tableIndex = 1) =>
            _basePage.Locator($"((((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}]//span)[1]");

        public ILocator AccessDropDownList =>
            _basePage.Locator($"//div[@class = 'k-list-content']//ul");

        public ILocator SelectDropdownOption(string nameContains) =>
            _basePage.Locator($"//div[contains(@class, 'k-animation-container-shown')]//span[contains(text(),'{nameContains}')]");

        public string GetCurrentDateTimeInFileFormat() =>
            DateTime.Now.ToString("yyyyMMdd");

        /// <summary>
        /// Gets the grid count for the specified table index.
        /// </summary>
        public async Task<int> GetGridCount(int tableIndex = 1)
        {
            var pager = GridPager(tableIndex);
            await pager.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await pager.ClickAsync(new() { Trial = true });
            var html = await pager.InnerHTMLAsync();
            var parts = html.Split();
            if (parts.Length < 5 || !int.TryParse(parts[4], out int count))
                throw new InvalidOperationException("Could not parse grid count from pager.");
            return count;
        }

        /// <summary>
        /// Sets text for a table cell, optionally clearing it first.
        /// </summary>
        public async Task SetTextForTd(ILocator by, string text, bool clearText = false)
        {
            await by.ClickAsync();
            var input = by.Locator("input");
            if (clearText) await input.ClearAsync();
            await input.FillAsync(text);
        }

        /// <summary>
        /// Deletes a row by name, with optional table index and offset.
        /// </summary>
        public async Task DeleteRowByName(string name, int tableIndex = 1, int offsetFromLast = 0)
        {
            var rowByNameXpath = $"((//table[contains(@class,'k-grid-table')])[{tableIndex}])//tr//td[contains(text(), '{name}')]/..";
            var rowByNameAltXpath = $"(((//table[contains(@class,'k-grid-table')])[{tableIndex}])//tr//span[contains(text(), '{name}')]/..)[1]";
            var deleteButton = "//*[contains(@class,'k-i-delete')]/ancestor::button";
            var lastGridFilter = "(//input[contains(@aria-label, 'Filter')])[last()]";

            if (await _basePage.Locator(rowByNameXpath).IsVisibleAsync())
                await _basePage.Locator(rowByNameXpath).ClickAsync();
            else
                await _basePage.Locator(rowByNameAltXpath).ClickAsync();

            await _basePage.Locator(lastGridFilter).ClickAsync();
            await _basePage.Locator(deleteButton).ClickAsync();

            await Task.Delay(500);

            if (await _pages.SharedElements.DeleteRecordModelYesButton.IsVisibleAsync())
                await _pages.SharedElements.DeleteRecordModelYesButton.ClickAsync();
        }

        /// <summary>
        /// Verifies a file was downloaded by type and date.
        /// </summary>
        public string VerifyFileWasDownloaded(string type = "")
        {
            var dataInformation = DateTime.Now.ToString("yyyyMMdd");
            string path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads";
            string[] filePaths = Directory.GetFiles(path);
            return filePaths.FirstOrDefault(p => p.Contains(type) && p.Contains(dataInformation)) ?? "";
        }

        /// <summary>
        /// Fills an input element, optionally clearing it first.
        /// </summary>
        public async Task InputText(ILocator by, string text, bool clearInput = true)
        {
            if (clearInput) await by.ClearAsync();
            await by.FillAsync(text);
        }

        /// <summary>
        /// Gets all properties and attributes of the first element matched by the locator.
        /// </summary>
        public async Task<Dictionary<string, object>> GetAllElementPropertiesAsync(ILocator locator)
        {
            return await locator.EvaluateAsync<Dictionary<string, object>>(@"el => {
                const props = {};
                for (const key of Object.getOwnPropertyNames(el)) {
                    props[key] = el[key];
                }
                if (el.attributes) {
                    props['attributes'] = {};
                    for (const attr of el.attributes) {
                        props['attributes'][attr.name] = attr.value;
                    }
                }
                return props;
            }");
        }

        /// <summary>
        /// Scrolls the page all the way to the right.
        /// </summary>
        public async Task ScrollPageToRightAsync()
        {
            await _basePage.EvaluateAsync(@"() => {
                const el = document.scrollingElement || document.documentElement || document.body;
                el.scrollLeft = el.scrollWidth;
            }");
        }
    }
}
