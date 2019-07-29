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
        public string GetLastDate(string fileName, int dateColIndex)
        {
            var allLines = File.ReadLines(fileName);

            if (allLines.Count() <= 1)
                return null;

            var lastLine = allLines.Last();
            var tokens = lastLine.Split(new char[] { ',' });

            var dateStr = tokens[dateColIndex];
            return dateStr;
        }
    }
}
