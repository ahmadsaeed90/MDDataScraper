using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.service.csvwriter
{
    public interface ICSVWriter
    {
        void AppendToFile(string fileName, List<List<string>> data);
    }
}
