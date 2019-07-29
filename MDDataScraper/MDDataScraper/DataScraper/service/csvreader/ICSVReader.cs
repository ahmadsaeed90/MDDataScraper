using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.service.csvreader
{
    public interface ICSVReader
    {
        string GetLastDate(string fileName, int dateColIndex);
    }
}
