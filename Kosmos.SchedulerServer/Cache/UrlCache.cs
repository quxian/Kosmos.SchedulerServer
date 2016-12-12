using EntityFramework.Extensions;
using Kosmos.SchedulerServer.DbContext;
using Kosmos.SchedulerServer.Model;
using Kosmos.Singleton;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Kosmos.SchedulerServer
{
    public static class UrlCache
    {
        public static ConcurrentBag<Url> Urls { get; set; }
        public static ConcurrentBag<string> DownloadedUrls { get; set; }
        public static ConcurrentBag<string> ErrorUrls { get; set; }
        public static ConcurrentQueue<Url> ForUrls { get; set; }

        private static Task _task;

        private static object _lock = new object();
        static UrlCache()
        {
            Urls = new ConcurrentBag<Url>();
            DownloadedUrls = new ConcurrentBag<string>();
            ErrorUrls = new ConcurrentBag<string>();
            ForUrls = new ConcurrentQueue<Url>();

            _task = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromMinutes(2));
                        using (var dbContext = new AppDbContext())
                        {
                            CacheToDb(dbContext);
                        }

                        File.WriteAllText("C:/Temp/SchedulerServer.txt", DateTime.Now.ToLongTimeString());
                    }
                    catch (Exception e)
                    {
                        SingleHttpClient.PostException(e);

                        File.WriteAllText("C:/Temp/SchedulerServer.txt", e.Message);
                    }
                }
            });
        }

        public static void CacheToDb(AppDbContext dbContext)
        {
            lock (_lock)
            {
                dbContext.DownloadErrorUrls.AddRange(ErrorUrls.Select(x => new DownloadErrorUrl { UrlHashCode = x }));
                ErrorUrls = new ConcurrentBag<string>();

                var dbHashCode = dbContext.Urls.Select(x => x.HashCode);
                var urlsHashCode = Urls.Select(x => x.HashCode);

                var exceptHashCode = urlsHashCode?.Except(dbHashCode).ToList();

                if (exceptHashCode.Count > 0)
                {
                    var exceptUrls = Urls
                        .AsParallel()
                        .Where(x => exceptHashCode.Any(h => x.HashCode == h))
                        .ToList();

                    dbContext.Urls.AddRange(exceptUrls);
                    dbContext.SaveChanges();
                }

                dbContext.Urls.Where(x => DownloadedUrls.Any(h => h == x.HashCode)).Update(x => new Url { IsDownloaded = true });
                dbContext.SaveChanges();

                DownloadedUrls = new ConcurrentBag<string>();
            }
        }

        public static void Init(List<Url> urls)
        {
            try
            {
                urls?.ForEach(url =>
                {
                    Urls.Add(url);
                    if (!url.IsDownloaded)
                        ForUrls.Enqueue(url);
                });
            }
            catch (Exception e)
            {
                SingleHttpClient.PostException(e);

                File.WriteAllText("C:/Temp/SchedulerServer.txt", e.Message);
            }
        }

        public static void Reinitialization(List<Url> urls)
        {
            try
            {
                Urls = new ConcurrentBag<Url>();
                ForUrls = new ConcurrentQueue<Url>();
                urls?.ForEach(url =>
                {
                    Urls.Add(url);
                    if (!url.IsDownloaded)
                        ForUrls.Enqueue(url);
                });
            }
            catch (Exception e)
            {
                SingleHttpClient.PostException(e);

                File.WriteAllText("C:/Temp/SchedulerServer.txt", e.Message);
            }
        }
    }
}