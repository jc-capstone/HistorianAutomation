using HistorianAutomation.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class ServerDetails : PageBase
    {
        public ServerDetails(IPage basePage) : base(basePage) { }

        public ILocator GetServerFieldByName(string name) => basePage.Locator($"(//td[text()='{name}'])[1]");
        public ILocator GetServerFieldValueByName(string name) => basePage.Locator($"(//td[text()='{name}'])/following-sibling::td");
        public ILocator GetTableHeadersByName(string name) => basePage.Locator($"(//ul//li//span[text()='{name}'])[1]");

        //public readonly List<String> Properties = new List<String>() {
        //    "Server Name", "Version - Build Date", "IP Address", "OS", "Process", "Process Id", "Process Start Time",
        //    "Process Elapsed Time", "CPU Count", "CPU Usage", "Working Set(MB)","Peak Working Set(MB)","Nonpaged System Memory Size(MB)",
        //    "Paged Memory Size(MB)","Paged System Memory Size(MB)","Peak Paged Memory Size(MB)","Peak Virtual Memory Size(MB)",
        //    "Private Memory Size(MB)","Virtual Memory Size(MB)","SDK Calls Per Second","Handle Count","Thread Count",
        //    "Max Thread Pool Worker Threads Count","Max Thread Pool Async Threads Count","Available Thread Pool Worker Threads Count",
        //    "Available Thread Pool Async Threads Count","Active Tag Count","Incoming Values Per Second","Values Writen Per Second",
        //    "Incoming ILocatortes Per Second","ILocatortes Written Per Second","Pending Queue Count","Active Subscription Count","Timestamp"
        //};


        //add the tab headers on server details page --
        public readonly List<String> Headers = new List<String>()
        {
            "Server", "Subscriptions", "Health", "Interfaces", "Collectors", "Communication", "System"
        };


    }
}
