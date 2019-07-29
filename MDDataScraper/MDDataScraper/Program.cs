using MDDataScraper.DataScraper.logger;
using MDDataScraper.DataScraper.service.config;
using MDDataScraper.DataScraper.service.csvreader;
using MDDataScraper.DataScraper.service.csvwriter;
using MDDataScraper.DataScraper.service.scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new ScraperService(new ConfigService(), new AppLogger(),
                new CSVReader(), new CSVWriter()
                ).ScrapData();
        }
    }
}
