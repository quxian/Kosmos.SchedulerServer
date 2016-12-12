using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kosmos.SchedulerServer
{
    public static class DownloaderServersAddressCache
    {
        public static List<string> Urls;

        static DownloaderServersAddressCache()
        {
            Urls = new List<string>();
        }
    }
}