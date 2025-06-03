

using HistorianAutomation.Framework;
using Microsoft.Playwright;
using HistorianUIAutomation.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;



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
        public ILocator GridPager(int tableIndex = 1) => _basePage.Locator($"(//span[contains(@class,'k-pager-info')])[{tableIndex}]");
        public ILocator AccessTable(int row, int column, int tableIndex = 1) => _basePage.Locator($"(((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}]");
        public ILocator AccessTableInput(int row, int column, int tableIndex = 1) => _basePage.Locator($"((((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}])//input");
        //public ILocator AccessTable(string rowName, int column, int tableIndex = 1) => _basePage.Locator($"((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr/td[text() ='{rowName}'])/..//td[{column}]");
        public ILocator AccessTable(string rowName, int column, int tableIndex = 1) => _basePage.Locator($"((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr//*[text() ='{rowName}' or @value='{rowName}'])/ancestor::tr//td[{column}]");
        public ILocator AccessTableWithSpan(string rowName, int column, int tableIndex = 1) => _basePage.Locator($"(((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr//*[text() ='{rowName}' or @value = '{rowName}' or @value = ''])/../..//td[{column}])[1]");
        public ILocator AccessTableForDropDown(int row, int column, int tableIndex = 1) => _basePage.Locator($"((((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}]).//span[1]");
        public ILocator AccessTableForDropDownClickDropDown(int row, int column, int tableIndex = 1) => _basePage.Locator($"((((//table[contains(@class,'k-grid-table')])[{tableIndex}]//tbody//tr)[{row}]//td)[{column}]//span)[1]");
        public ILocator AccessDropDownList => _basePage.Locator($"//div[@class = 'k-list-content']//ul");
        public ILocator SelectDropdownOption(string nameContains) => _basePage.Locator($"//div[contains(@class, 'k-animation-container-shown')]//span[contains(text(),'{nameContains}')]");
        public string GetCurrentDateTimeInFileFormat() => DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
        
        public async Task<int> GetGridCount(int tableIndex = 1)
        {
            var gridData = await WaitHelper.WaitForAsync(async () =>
            {
                Thread.Sleep(3000);
                var pager = GridPager(tableIndex);
                await pager.ClickAsync(new() { Trial = true });
                return int.Parse((await pager.InnerHTMLAsync()).Split()[4]);
            });
            return gridData;
        }

        public async Task SetTextForTd(ILocator by, string text, bool clearText = false)
        {
            await by.ClickAsync();
            var input = by.Locator("input");
            if (clearText) await input.ClearAsync();
            await input.FillAsync(text);
        }

        public async Task  DeleteRowByName(string name, int tableIndex = 1, int offsetFromLast = 0)
        {
            var rowByNameXpath = $"((//table[contains(@class,'k-grid-table')])[{tableIndex}])//tr//td[contains(text(), '{name}')]/..";
            //some tables keep the value within a span
            var rowByNameAltXpath = $"(((//table[contains(@class,'k-grid-table')])[{tableIndex}])//tr//span[contains(text(), '{name}')]/..)[1]";


            var deleteButtonByNameXpath = $"((//table[contains(@class,'k-grid-table')])[{tableIndex}])//tr//td/following-sibling::td[last()-{(1 + offsetFromLast).ToString()}]//button";
            if (await _basePage.Locator(rowByNameXpath).IsVisibleAsync())
            {
                await _basePage.Locator(rowByNameXpath).ClickAsync();
            }
            else
            {
                await _basePage.Locator(rowByNameAltXpath).ClickAsync();
            }
            var test = "//*[contains(@class,'k-i-delete')]/ancestor::button";
            await _basePage.Locator(test).ClickAsync();

            await Task.Delay(500);

            if (await _pages.SharedElements.DeleteRecordModelYesButton.IsVisibleAsync())
            {
                await _pages.SharedElements.DeleteRecordModelYesButton.ClickAsync();
            }
        }


        public string VerifyFileWasDownloaded(string type = "")
        {
            var dataInformation = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");

            var existingFilePath = "";
            string Path = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads";
            string[] filePaths = Directory.GetFiles(Path);
            foreach (string p in filePaths)
            {
                if (p.Contains(type) && p.Contains(dataInformation))
                {
                    existingFilePath = p;
                }
            }
            return existingFilePath;
        }

        public async Task InputText(ILocator by, string text, bool clearInput = true)
        {
            if (clearInput) await by.ClearAsync();
            await by.FillAsync(text);

        }


    }
}
