﻿using log4net;
using MDDataScraper.DataScraper.config;
using MDDataScraper.DataScraper.logger;
using MDDataScraper.DataScraper.service.config;
using MDDataScraper.DataScraper.service.csvreader;
using MDDataScraper.DataScraper.service.csvwriter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.service.scraper
{
    public class ScraperService
    {
        private readonly IConfigService _configService;
        private readonly ILog _logger;
        private readonly ICSVReader _csvReader;
        private readonly ICSVWriter _csvWriter;

        public ScraperService(IConfigService configService, IAppLogger appLogger, ICSVReader csvReader, ICSVWriter csvWriter)
        {
            _configService = configService;
            _logger = appLogger.GetLogger();
            _csvReader = csvReader;
            _csvWriter = csvWriter;
        }

        public void ScrapData()
        {
            var config = _configService.GetAppConfig();
            _logger.Info("Loaded configuration");
            var pages = config.ScrapPages;

            using (var driver = new ChromeDriver())
            {
                foreach (var page in pages)
                {
                    if (page.Ignore)
                    {
                        _logger.Info("Ignoring Page: " + page.URL);
                        continue;
                    }

                    var tableData = ScrapTableData(page, driver);
                    _logger.Info(tableData);

                    var dateCol = page.ColumnMappings.Find(x => x.IsDate == true);

                    if (dateCol == null)
                    {
                        throw new Exception("Cannot find date column in the column mappings");
                    }

                    var filtered = FilterNewRecords(tableData, page, dateCol);
                    _logger.Info("new records = " + filtered.Count);

                    filtered.Reverse();

                    if (filtered.Count > 0)
                    {
                        _logger.Info("FOUND new data on web, appending to file");
                        var headings = page.ColumnMappings.Select(x => x.HeadingInFile).ToList();
                        _csvWriter.AppendToFile(page.FilePath, filtered, headings);
                        _logger.Info("done");
                    }
                    else
                    {
                        _logger.Info("NOT found new data on web");
                    }
                }
            }
        }

        private List<List<string>> FilterNewRecords(List<List<string>> tableData, ScrapPage page, ColumnMapping dateCol)
        {
            var dateColIndex = page.ColumnMappings.IndexOf(dateCol);

            var latestDateInFileStr = _csvReader.GetLastDate(page.FilePath, dateColIndex);
            _logger.Info("latestDateInFileStr = " + latestDateInFileStr);

            if (string.IsNullOrWhiteSpace(latestDateInFileStr))
                return tableData;   //all records are new

            var latestDateInFile = DateTime.ParseExact(latestDateInFileStr, dateCol.OutputDateFormat, CultureInfo.InvariantCulture);

            var filtered = tableData.Where(x => 
                    DateTime.ParseExact(x[dateColIndex], dateCol.OutputDateFormat, CultureInfo.InvariantCulture) > latestDateInFile);

            return filtered.ToList();
        }

        private List<List<string>> ScrapTableData(ScrapPage page, ChromeDriver driver)
        {
            var tableData = new List<List<string>>();
            
            _logger.Info($"Scraping page [{page.URL}]");
            driver.Navigate().GoToUrl(page.URL);
            _logger.Info("page loaded");

            // Wait for table to load
            new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                .Until(ExpectedConditions.ElementExists(
                    string.IsNullOrWhiteSpace(page.TableId) ? By.ClassName(page.TableClass) : By.Id(page.TableId)
                    ));

            _logger.Info("finding rows");
            var tableRows = driver.FindElements(By.XPath(page.TableRowsXPath));
            _logger.Info("Table rows count = " + tableRows.Count);

            foreach (var row in tableRows)
            {
                var rowData = ReadRowData(row, page.ColumnMappings);
                tableData.Add(rowData);
            }
            

            return tableData;
        }

        private List<string> ReadRowData(IWebElement row, List<ColumnMapping> columnMappings)
        {
            _logger.Info("Loading row");
            var cols = new List<string>(columnMappings.Count);

            for (var i = 0; i < columnMappings.Count; i++)
            {
                cols.Add(ReadColumnData(row, columnMappings[i]));
            }

            return cols;
        }

        private string ReadColumnData(IWebElement row, ColumnMapping columnMapping)
        {
            string strValue;

            // Replace with fixed value from column setting if any
            if (!string.IsNullOrWhiteSpace(columnMapping.Value))
            {
                strValue = columnMapping.Value;
            }
            else
            {
                var colElem = row.FindElement(By.XPath("./" + columnMapping.WebColumnRelativeXPath));
                strValue = colElem.GetAttribute("innerHTML") ?? string.Empty;

                strValue = Sanitize(strValue);

                strValue = ApplyReplace(strValue, columnMapping.Replace);

                // Apply transformation
                if (columnMapping.IsDate == true && columnMapping.OutputDateFormat != columnMapping.WebDateFormat)
                {
                    strValue = ConvertDateFormat(strValue, columnMapping.WebDateFormat, columnMapping.OutputDateFormat);
                }
            }

            return strValue;
        }

        private string ApplyReplace(string strValue, List<ReplacePair> replace)
        {
            if (replace != null && replace.Any())
            {
                foreach (var x in replace)
                {
                    strValue = strValue.Replace(x.Old, x.New);
                }
            }
            return strValue;
        }

        private string Sanitize(string str)
        {
            // Remove comma from value
            str = str.Replace(",", "");

            return str;
        }

        private string ConvertDateFormat(string strValue, string inputDateFormat, string outputDateFormat)
        {
            var dateTime = DateTime.ParseExact(strValue, inputDateFormat, CultureInfo.InvariantCulture);
            return dateTime.ToString(outputDateFormat);
        }

        

    }
}
