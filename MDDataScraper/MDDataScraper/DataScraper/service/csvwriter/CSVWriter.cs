using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.service.csvwriter
{
    public class CSVWriter : ICSVWriter
    {
        public void AppendToFile(string fileName, List<List<string>> rows)
        {
            var text = string.Join(Environment.NewLine, rows.Select(x => string.Join(",", x)));
            File.AppendAllText(fileName, text);
        }
    }
}
