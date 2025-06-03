using HistorianAutomation.Framework;
using HistorianUIAutomation.Pages;
using System;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Tests
{
    public class FolderPathsTests : BaseTest
    {
        [Test]
        public async Task AddFolderPath()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Folder Paths").ClickAsync();

            var name = Guid.NewGuid().ToString();
            var folderPathString = "C:\\Test";
            var originalGridCount = await Interaction.GetGridCount();

            await Pages.FolderPaths.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Name), name, true);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.FolderPath), folderPathString);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Description), name);

            await Pages.FolderPaths.SaveChangesButton.ClickAsync();

            if (await Pages.FolderPaths.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.FolderPaths.SaveChangesPopupOkButton.ClickAsync();
            }

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            if (await Pages.FolderPaths.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.FolderPaths.SaveChangesPopupOkButton.ClickAsync();
            }

            Assert.IsTrue(gridCountAfterSave > originalGridCount);

            await Interaction.DeleteRowByName(name);
        }

        [Test]
        public async Task EditFolderPaths()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Folder Paths").ClickAsync();

            var name = Guid.NewGuid().ToString();
            var folderPathString = "C:\\Test";
            var originalGridCount = await Interaction.GetGridCount();

            await Pages.FolderPaths.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Name), name, true);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.FolderPath), folderPathString);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Description), name);

            await Pages.FolderPaths.SaveChangesButton.ClickAsync();

            if (await Pages.FolderPaths.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.FolderPaths.SaveChangesPopupOkButton.ClickAsync();
            }

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            var filterInput = Pages.FolderPaths.TableFilterInput(Pages.FolderPaths.Name);
            await filterInput.FillAsync(name);
            await BasePage.Keyboard.PressAsync("Tab");

            var newname = name + "test";
            var newFolderPathString = "C:\\TestUpdate";


            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.FolderPath), newFolderPathString, true);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Description), newname, true);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Name), newname);
            await Pages.SharedElements.ClearGridFilterButton.ClickAsync();
            await filterInput.FillAsync(newname);

            await Pages.FolderPaths.SaveChangesButton.ClickAsync();

            if (await Pages.FolderPaths.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.FolderPaths.SaveChangesPopupOkButton.ClickAsync();
            }
            var nameText = await Interaction.AccessTable(1, Pages.FolderPaths.Name).TextContentAsync();
            var folderPathText = await Interaction.AccessTable(1, Pages.FolderPaths.FolderPath).TextContentAsync();
            var descriptionText = await Interaction.AccessTable(1, Pages.FolderPaths.Description).TextContentAsync();

            Assert.IsTrue(nameText == newname);
            Assert.IsTrue(newFolderPathString == folderPathText);
            Assert.IsTrue(newname == descriptionText);

            await Interaction.DeleteRowByName(newname);
        }

        [Test]
        public async Task DeleteFolderPaths()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Folder Paths").ClickAsync();

            var name = Guid.NewGuid().ToString();
            var folderPathString = "C:\\Test";
            var originalGridCount = await Interaction.GetGridCount();

            await Pages.FolderPaths.AddNewButton.ClickAsync();
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Name), name, true);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.FolderPath), folderPathString);
            await Interaction.SetTextForTd(Interaction.AccessTable(1, Pages.FolderPaths.Description), name);

            await Pages.FolderPaths.SaveChangesButton.ClickAsync();

            if (await Pages.FolderPaths.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.FolderPaths.SaveChangesPopupOkButton.ClickAsync();
            }

            int gridCountAfterSave = await Interaction.GetGridCount();
            while (gridCountAfterSave <= originalGridCount)
            {
                await Task.Delay(200);
                gridCountAfterSave = await Interaction.GetGridCount();
            }

            if (await Pages.FolderPaths.SaveChangesPopupOkButton.IsVisibleAsync())
            {
                await Pages.FolderPaths.SaveChangesPopupOkButton.ClickAsync();
            }

            Assert.IsTrue(gridCountAfterSave > originalGridCount);

            await Interaction.DeleteRowByName(name);

            if (await Pages.FolderPaths.DeleteRecordYesButton.IsVisibleAsync())
            {
                await Pages.FolderPaths.DeleteRecordYesButton.ClickAsync();
            }
            int gridCountAfterDelete = await Interaction.GetGridCount();
            while (gridCountAfterDelete >= gridCountAfterSave)
            {
                await Task.Delay(200);
                gridCountAfterDelete = await Interaction.GetGridCount();
            }
            Assert.IsTrue(gridCountAfterDelete == originalGridCount);
        }

        [Test]
        public async Task EnsureGridFiltersProperlyClear()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.HomePage.ConfigurationTabByName("Folder Paths").ClickAsync();

            var name = Guid.NewGuid().ToString().Substring(0, 8);

            var filterInput = Pages.FolderPaths.TableFilterInput(Pages.FolderPaths.Name);
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