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
    public class TagsConfigurationTests : BaseTest
    {
        [Test]
        public async Task AddTag()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Tags").ClickAsync();

            await Pages.TagsConfiguration.ConfigurationGroup.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            await Pages.TagsConfiguration.ConfigurationInterface.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            WaitHelper.WaitFor(5000, "Grid to Filter");
            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.TagsConfiguration.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.TagsConfiguration.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.TagsConfiguration.SaveChangesButton.ClickAsync();

            int gridCountAfterSave = originalGridCount;
            await WaitHelper.WaitForAsync(async () =>
            {
                gridCountAfterSave = await Interaction.GetGridCount();
                return gridCountAfterSave > originalGridCount ;
            });

            Assert.IsTrue(gridCountAfterSave > originalGridCount);
            await Pages.TagsConfiguration.TableFilterInput(Pages.TagsConfiguration.Name).FillAsync(testName);
            await BasePage.Keyboard.PressAsync("Tab");

            await Interaction.DeleteRowByName(testName);
        }

        [Test]
        public async Task EditTag()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Tags").ClickAsync();

            await Pages.TagsConfiguration.ConfigurationGroup.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            await Pages.TagsConfiguration.ConfigurationInterface.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.TagsConfiguration.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.TagsConfiguration.Name), testName);
            await BasePage.Keyboard.PressAsync("Tab");
            await Pages.TagsConfiguration.SaveChangesButton.ClickAsync();

            var gridCountAfterSave = await Interaction.GetGridCount();

            var description = "TEST";
            await Pages.TagsConfiguration.TableFilterInput(Pages.TagsConfiguration.Name -1).FillAsync(testName);
            await BasePage.Keyboard.PressAsync("Tab");

            await Interaction.SetTextForTd(Interaction.AccessTable(testName, Pages.TagsConfiguration.Description), description);
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.TagsConfiguration.SaveChangesButton.ClickAsync();

            var text = await Interaction.AccessTable(testName, Pages.TagsConfiguration.Description).InnerTextAsync();

            Assert.IsTrue(text == description);
            await Interaction.DeleteRowByName(testName);
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Tags").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.TagsConfiguration.TableFilterInput(Pages.TagsConfiguration.Name).FillAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");
            WaitHelper.WaitFor(500, "Wait for grid to load");
            var origValText = await Pages.TagsConfiguration.TableFilterInput(Pages.TagsConfiguration.Name).GetAttributeAsync("value");
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            WaitHelper.WaitFor(500, "Wait for grid to clear");
            var valText = await Pages.TagsConfiguration.TableFilterInput(Pages.TagsConfiguration.Name).GetAttributeAsync("value");

            Assert.IsTrue(!String.IsNullOrWhiteSpace(origValText));
            Assert.IsTrue(String.IsNullOrWhiteSpace(valText));
        }

        [Test]
        public async Task DownloadTableInformation()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Tags").ClickAsync();

            await Pages.TagsConfiguration.ConfigurationGroup.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });

            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter"); ;

            await Pages.TagsConfiguration.ConfigurationInterface.ClickAsync();
            await Pages.SharedElements.AnimationContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Enter");

            WaitHelper.WaitFor(250, "Grid to Filter");
            await Pages.TagsConfiguration.ExportToExcelButton.ClickAsync();
            var waitForDownloadTask = BasePage.WaitForDownloadAsync();
            await Pages.TagsConfiguration.ExportToExcelPopupSubmitButton.ClickAsync();

            var dl = await waitForDownloadTask;
            await dl.SaveAsAsync(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + dl.SuggestedFilename);

            var file = Interaction.VerifyFileWasDownloaded("Tags");


            Assert.False(String.IsNullOrEmpty(file));

            File.Delete(file);
        }


        //[Test]
        //public async Task DownloadTableInformation()
        //{
        //    await TestUtilities.NavigateToHistorian();
        //    await Pages.HomePage.ConfigurationTabByName("Tags").ClickAsync();

        //    var originalGridCount = await Interaction.GetGridCount();
        //    var testName = Guid.NewGuid().ToString().Substring(0, 9);

        //    await Pages.AggregateConfiguration.ExportToExcelButton.ClickAsync();
        //    var waitForDownloadTask = BasePage.WaitForDownloadAsync();
        //    await Pages.AggregateConfiguration.ExportToExcelPopupSubmitButton.ClickAsync();

        //    var dl = await waitForDownloadTask;
        //    await dl.SaveAsAsync(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads\\" + dl.SuggestedFilename);

        //    var file = Interaction.VerifyFileWasDownloaded("Tags");

        //    Assert.False(String.IsNullOrEmpty(file));

        //    File.Delete(file);
        //}

        [Test]
        public async Task UrlContainsMultiHistorianStyleGuid()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Tags").ClickAsync();
            await BasePage.WaitForURLAsync(url => url.Contains("/tags"));
            var urlElements = BasePage.Url.Split("/").ToList();
            var historianGuidString = urlElements[urlElements.IndexOf("tags") - 1];
            Assert.IsTrue(Guid.TryParse(historianGuidString, out var result));
        }

        //[Test]
        //public async Task UploadTableInformation()
        //{
        //    TestUtilities.NavigateToHistorian();
        //    Interaction.ClickElement(Pages.HomePage.ConfigurationTabByName("Tags"));
        //    var testName = Guid.NewGuid().ToString().Substring(0, 8);
        //    Interaction.ClickElement(Pages.TagsConfiguration.ConfigurationInterface);
        //    WaitHelper.WaitForElementVisible(Pages.SharedElements.AnimationContainer);

        //    new Actions(Driver).KeyDown(Keys.ArrowDown).KeyDown(Keys.Enter).Perform();
        //    var originalGridCount = Interaction.GetGridCount();
        //    Interaction.ClickElement(Pages.TagsConfiguration.ImportFromExcelButton);
        //    if (Interaction.GetElement(Pages.TagsConfiguration.UploadExcelInput) == null) Assert.Ignore();
        //    Interaction.InputText(Pages.TagsConfiguration.UploadExcelInput, Directory.GetCurrentDirectory() + $"\\TestFiles\\{Pages.TagsConfiguration.ExcelImportFileName}", false);
        //    WaitHelper.WaitFor(() => Interaction.GetElements(Pages.ImportExcelGrid.GridRows).Count > 0);

        //    Interaction.ClickElement(Pages.TagsConfiguration.SubmitImportButton);
        //    WaitHelper.WaitFor(() => Interaction.GetGridCount() > originalGridCount);
        //    var gridCountAfterAdd = Interaction.GetGridCount();
        //    Assert.IsTrue(gridCountAfterAdd >= originalGridCount);

        //    Interaction.DeleteRowByName(Pages.TagsConfiguration.ExcelImportItemName);
        //    Interaction.ClickElement(Pages.TagsConfiguration.ConfirmDeletePopupButton);
        //}
    }
}
