using HistorianAutomation.Framework;
using Microsoft.Playwright;

namespace HistorianUIAutomation.Pages
{
    public class Security : PageBase
    {
        public Security(IPage basePage) : base(basePage) { }

        public readonly List<String> SecurityHeaders = new List<String>() {"Roles","Role Permissions", "Claims", "Role Claims", "Permission Claims"};
        public readonly List<String> RolesHeaders = new List<String>() {"Name"};
        public readonly List<String> RolePermissionsHeaders = new List<String>() { "Role", "Permission" };
        public readonly List<String> ClaimsHeaders = new List<String>() { "Name", "Value" };
        public readonly List<String> RoleClaimsHeaders = new List<String>() { "Role", "Claim" };
        public readonly List<String> PermissionClaimsHeaders = new List<String>() { "Claim", "Permission" };
        
        public ILocator AppSecurityButton => basePage.Locator($"(//span[text() = 'Security'])[1]/../..");
        public ILocator GetSecuritySectionHeaders(string name) => basePage.Locator($"//ul[contains(@class, 'k-tabstrip-items')]//span[text()='{name}']");
        public ILocator GetTableHeadersByName(string name) => basePage.Locator($"(//ul//li//span[text()='{name}'])[1]");
        public ILocator GetSecurityTableHeader(string name) => basePage.Locator($"(//ul//li//span[text()='{name}'])[1]");
        public ILocator GetSubTableHeader(string name) => basePage.Locator($"(//div[contains(@class, 'k-content')]//span[text()='{name}'])[1]");
    }
}
