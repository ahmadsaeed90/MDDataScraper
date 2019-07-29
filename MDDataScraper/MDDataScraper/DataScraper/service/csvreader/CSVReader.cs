using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.service.csvreader
{
    public class CSVReader : ICSVReader
    {
        public string GetLastDate(string fileName)
        {
            var lines = File.ReadLines(fileName).Last();
            return "";
        }
    }
}
