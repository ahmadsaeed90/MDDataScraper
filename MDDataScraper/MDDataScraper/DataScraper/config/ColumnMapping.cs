using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.config
{
    public class ColumnMapping
    {
        public string WebColumnRelativeXPath { get; set; }

        public string Value { get; set; }

        public bool? IsDate { get; set; }

        public string WebDateFormat { get; set; }

        public string OutputDateFormat { get; set; }
    }
}
