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
        public void AppendToFile(string fileName, List<List<string>> rows, List<string> headings)
        {
            if (!File.Exists(fileName) || File.ReadLines(fileName).Count() == 0)
            {
                var headingsStr = string.Join(",", headings);
                File.AppendAllText(fileName, headingsStr + Environment.NewLine);
            }

            var text = string.Join(Environment.NewLine, rows.Select(x => string.Join(",", x)));
            File.AppendAllText(fileName, text);
        }
    }
}
