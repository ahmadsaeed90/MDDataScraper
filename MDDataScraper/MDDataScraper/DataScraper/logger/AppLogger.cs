using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.logger
{
    public class AppLogger: IAppLogger
    {
        private readonly ILog _log = LogManager.GetLogger("DataScraper");

        public ILog GetLogger()
        {
            return _log;
        }
    }
}
