using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.config
{
    public class ScrapPage
    {
        public string URL { get; set; }

        public string FilePath { get; set; }

        public string TableClass { get; set; }

        public string TableId { get; set; }

        public string TableRowsXPath { get; set; }

        public List<ColumnMapping> ColumnMappings { get; set; }
    }
}
