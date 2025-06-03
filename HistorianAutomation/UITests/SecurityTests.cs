using HistorianUIAutomation.Tests;
using Microsoft.Playwright;
using HistorianAutomation.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HistorianUIAutomation.Tests
{
    [Ignore("")]

    public class SecurityTests : BaseTest
    {
        //[Test]
        public async Task CanNavigateToSecuritySection()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.Security.AppSecurityButton.ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/security"));
            Assert.IsTrue(BasePage.Url.Contains("/security"));
        }

        //[Test]
        public async Task SecurityHeadersExist()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.Security.AppSecurityButton.ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/security"));

            foreach (var prop in Pages.Security.SecurityHeaders)
            {
                Assert.IsTrue(await Pages.Security.GetTableHeadersByName(prop).IsVisibleAsync());
            }
        }

        //[Test]
        public async Task RolesTableHeadersExist()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.Security.AppSecurityButton.ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/security"));

            foreach (var prop in Pages.Security.RolesHeaders)
            {
                Assert.IsTrue(await Pages.Security.GetSubTableHeader(prop).IsVisibleAsync());
            }
        }

        //[Test]
        public async Task RolePermissionTableHeadersExist()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.Security.AppSecurityButton.ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/security"));
            await Pages.Security.GetSecuritySectionHeaders("Role Permissions").ClickAsync();
            foreach (var prop in Pages.Security.RolePermissionsHeaders)
            {
                Assert.IsTrue(await Pages.Security.GetSubTableHeader(prop).IsVisibleAsync());
            }
        }

        //[Test]
        public async Task ClaimsTableHeadersExist()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.Security.AppSecurityButton.ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/security"));
            await Pages.Security.GetSecuritySectionHeaders("Claims").ClickAsync();

            foreach (var prop in Pages.Security.ClaimsHeaders)
            {
                Assert.IsTrue(await Pages.Security.GetSubTableHeader(prop).IsVisibleAsync());
            }
        }

        //[Test]
        public async Task RoleClaimsTableHeadersExist()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.Security.AppSecurityButton.ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/security"));
            await Pages.Security.GetSecuritySectionHeaders("Role Claims").ClickAsync();

            foreach (var prop in Pages.Security.RoleClaimsHeaders)
            {
                Assert.IsTrue(await Pages.Security.GetSubTableHeader(prop).IsVisibleAsync());
            }
        }

        //[Test]
        public async Task PermissionClaimsTableHeadersExist()
        {
            await TestUtilities.NavigateToHistorian();
            await Pages.Security.AppSecurityButton.ClickAsync();

            await BasePage.WaitForURLAsync(url => url.Contains("/security"));
            await Pages.Security.GetSecuritySectionHeaders("Permission Claims").ClickAsync();

            foreach (var prop in Pages.Security.PermissionClaimsHeaders)
            {
                Assert.IsTrue(await Pages.Security.GetSubTableHeader(prop).IsVisibleAsync());
            }
        }
    }
}
