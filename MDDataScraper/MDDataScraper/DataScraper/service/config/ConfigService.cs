using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDDataScraper.DataScraper.config;
using Newtonsoft.Json;

namespace MDDataScraper.DataScraper.service.config
{
    public class ConfigService : IConfigService
    {
        public const String CONFIG_FILENAME = "appconfig.json";

        public AppConfig GetAppConfig()
        {
            string json = File.ReadAllText(CONFIG_FILENAME);
            var result = JsonConvert.DeserializeObject<AppConfig>(json);
            return result;
        }
    }
}
