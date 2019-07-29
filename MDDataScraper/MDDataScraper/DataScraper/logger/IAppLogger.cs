using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDDataScraper.DataScraper.logger
{
    public interface IAppLogger
    {
        ILog GetLogger();
    }
}
