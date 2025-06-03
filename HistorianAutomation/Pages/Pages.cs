using HistorianAutomation.Framework;
using Microsoft.Playwright;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianUIAutomation.Pages
{
    public class Pages
    {
        private readonly IPage _basePage;

        public CollectorConnectedInstance CollectorConnectedInstance { get; set; }
        public CollectorInstances CollectorInstances { get; set; }
        public CollectorSourceConfiguration CollectorSourceConfiguration { get; set; }
        public CollectorSources CollectorSources { get; set; }
        public HomePage HomePage { get; set; }
        public RealtimeSources RealtimeSources { get; set; }
        public AggregateConfiguration AggregateConfiguration { get; set; }
        public InterfaceGroups InterfaceGroups { get; set; }
        public Interfaces Interfaces { get; set; }
        public TagsConfiguration TagsConfiguration { get; set; }
        public DigitalTypes DigitalTypes { get; set; }
        public ServerDetails ServerDetails { get; set; }
        public Logs Logs { get; set; }
        public ImportExcelGrid ImportExcelGrid { get; set; }
        public SharedElements SharedElements { get; set; }
        public Security Security { get; set; }
        public InterfaceSets InterfaceSets { get; set; }
        public FolderPaths FolderPaths { get; set; }

        public Pages(IPage basePage)
        {
            _basePage = basePage;
            CollectorConnectedInstance = new(_basePage);
            CollectorInstances = new(_basePage);
            CollectorSourceConfiguration = new(_basePage);
            CollectorSources = new(_basePage);
            HomePage = new(_basePage);
            RealtimeSources = new(_basePage);
            AggregateConfiguration = new(_basePage);
            InterfaceGroups = new(_basePage);
            Interfaces = new(_basePage);
            TagsConfiguration = new(_basePage);
            DigitalTypes = new(_basePage);
            ServerDetails = new(_basePage);
            Logs = new(_basePage);
            ImportExcelGrid = new(_basePage);
            SharedElements = new(_basePage);
            Security = new(_basePage);
            InterfaceSets = new(_basePage);
            FolderPaths = new(_basePage);
        }
    }
}
