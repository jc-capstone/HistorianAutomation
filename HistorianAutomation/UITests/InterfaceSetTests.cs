using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class InterfaceSetTests : BaseTest
    {
        [Test]
        public async Task AddInterfaceSet()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Sets").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.InterfaceSets.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.InterfaceSets.Name), testName);

            var groupCell = Interaction.AccessTable(1, Pages.InterfaceSets.InterfaceGroup);
            await groupCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Tab");

            await groupCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.InterfaceSets.SaveChangesButton.ClickAsync();

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            Assert.IsTrue(gridCountAfterSave > originalGridCount);

            await Interaction.DeleteRowByName(testName);
            if (await Pages.InterfaceSets.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.InterfaceSets.DeleteRecordYesButton.ClickAsync();
            }
        }

        [Test]
        public async Task DeleteInterfaceSet()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Sets").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.InterfaceSets.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.InterfaceSets.Name), testName);

            var groupCell = Interaction.AccessTable(1, Pages.InterfaceSets.InterfaceGroup);
            await groupCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Tab");

            await groupCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.InterfaceSets.SaveChangesButton.ClickAsync();

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            Assert.IsTrue(gridCountAfterSave > originalGridCount);

            await Interaction.DeleteRowByName(testName);
            if (await Pages.InterfaceSets.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.InterfaceSets.DeleteRecordYesButton.ClickAsync();
            }

            int gridCountAfterDelete = await Interaction.GetGridCount();
            while (gridCountAfterDelete >= gridCountAfterSave)
            {
                await Task.Delay(200);
                gridCountAfterDelete = await Interaction.GetGridCount();
            }

            Assert.IsTrue(gridCountAfterSave > gridCountAfterDelete);
        }

        //[Test]
        public async Task EditInterfaceSet()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Sets").ClickAsync();

            var originalGridCount = await Interaction.GetGridCount();
            var testName = Guid.NewGuid().ToString().Substring(0, 8);

            await Pages.InterfaceSets.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.InterfaceSets.Name), testName);

            var groupCell = Interaction.AccessTable(1, Pages.InterfaceSets.InterfaceGroup);
            await groupCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Tab");

            await groupCell.DblClickAsync();
            await BasePage.Keyboard.PressAsync("ArrowDown");
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.InterfaceSets.SaveChangesButton.ClickAsync();

            var filterInput = Pages.InterfaceSets.TableFilterInput(Pages.InterfaceSets.Name);
            await filterInput.FillAsync(testName);
            await BasePage.Keyboard.PressAsync("Tab");

            await Pages.InterfaceSets.GetInterfacesEditButtonILocatorIndex().ClickAsync();

            var checkboxes = await Pages.InterfaceSets.InterfaceEditCheckboxes.AllAsync();
            if (checkboxes.Count == 0)
            {
                Assert.Inconclusive("No Interfaces in Group");
                return;
            }

            var checkbox = Pages.InterfaceSets.GetInterfacesEditButtonILocatorIndex();
            var originalValue = await checkbox.IsCheckedAsync();
            await checkbox.ClickAsync();
            var afterEditValue = await checkbox.IsCheckedAsync();

            await Pages.InterfaceSets.InterfaceEditSaveButton.ClickAsync();
            await Pages.InterfaceSets.InterfaceEditCloseButton.ClickAsync();

            await Pages.InterfaceSets.GetInterfacesEditButtonILocatorIndex().ClickAsync();

            Assert.That(originalValue != afterEditValue);
            Assert.That(afterEditValue == await checkbox.IsCheckedAsync());

            await Pages.InterfaceSets.InterfaceEditCloseButton.ClickAsync();

            await Interaction.DeleteRowByName(testName);
            if (await Pages.InterfaceSets.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.InterfaceSets.DeleteRecordYesButton.ClickAsync();
            }
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Interface Sets").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            var filterInput = Pages.InterfaceSets.TableFilterInput(Pages.InterfaceSets.Name);
            await filterInput.FillAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");
            await Task.Delay(500);
            var origValText = await filterInput.GetAttributeAsync("value");
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            await Task.Delay(500);
            var valText = await filterInput.GetAttributeAsync("value");

            Assert.IsTrue(!string.IsNullOrWhiteSpace(origValText));
            Assert.IsTrue(string.IsNullOrWhiteSpace(valText));
        }
    }
}