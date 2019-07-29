using MDDataScraper.DataScraper.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.service.config
{
    public interface IConfigService
    {
        AppConfig GetAppConfig();
    }
}
