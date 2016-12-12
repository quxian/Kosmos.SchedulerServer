using EntityFramework.Extensions;
using Kosmos.SchedulerServer.DbContext;
using Kosmos.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kosmos.SchedulerServer.Controllers
{
    public class SchedulerController : ApiController
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;

        private static Task _task;
        private static bool _isRuning = false;
        private static bool _isDo = true;
        private static object _lock = new object();

        public SchedulerController(AppDbContext dbContext, HttpClient httpClinet)
        {
            _dbContext = dbContext;
            _httpClient = httpClinet;
        }

        [HttpGet]
        [Route("api/Scheduler/AddDownloaderServersAddress")]
        public async Task<IHttpActionResult> AddDownloaderServersAddress(string address)
        {
            if (DownloaderServersAddressCache.Urls.IndexOf(address) > -1)
                return Ok(address);
            DownloaderServersAddressCache.Urls.Add(address);
            return Ok(address);
        }

        [HttpGet]
        [Route("api/Scheduler/AddDownloaderServersAddress/List")]
        public async Task<IHttpActionResult> ListDownloaderServersAddress()
        {
            return Ok(DownloaderServersAddressCache.Urls);
        }

        [HttpGet]
        [Route("api/Scheduler/AddDownloaderServersAddress/Delete")]
        public async Task<IHttpActionResult> DeleteDownloaderServersAddress(string address)
        {
            DownloaderServersAddressCache.Urls.Remove(address);
            return Ok(address);
        }

        [HttpPost]
        [Route("api/Scheduler/AddDownloaderServersAddress")]
        public async Task<IHttpActionResult> AddDownloaderServersAddress(List<string> address)
        {
            var newAddress = address.Except(DownloaderServersAddressCache.Urls);
            DownloaderServersAddressCache.Urls.AddRange(newAddress);
            return Ok(newAddress);
        }

        [HttpGet]
        [Route("api/Scheduler/Run")]
        public async Task<IHttpActionResult> Run()
        {
            lock (_lock)
            {
                if (_isRuning)
                    return BadRequest("正在运行！");
                _isRuning = true;
                _isDo = true;
            }

            _task = Task.Run(async () =>
            {
                await Do();
            });

            return Ok("已启动");
        }

        private async Task Do()
        {
            while (_isDo)
            {
                Model.Url url;
                UrlCache.ForUrls.TryDequeue(out url);

                if (null == url)
                {
                    await Task.Delay(1000);
                    continue;
                }

                try
                {
                    var httpResponseMessage = await _httpClient.PostAsJsonAsync($"{DownloaderServersAddressCache.Urls.First()}api/Download", url);
                    if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        UrlCache.ErrorUrls.Add(url.HashCode);
                    }
                    UrlCache.DownloadedUrls.Add(url.HashCode);
                }
                catch (Exception e)
                {
                    SingleHttpClient.PostException(e);

                    UrlCache.ErrorUrls.Add(url.HashCode);
                }
            }
        }

        [HttpGet]
        [Route("api/Scheduler/Stop")]
        public async Task<IHttpActionResult> Stop()
        {
            _isRuning = false;
            _isDo = false;
            _task.Wait();

            UrlCache.CacheToDb(_dbContext);

            return Ok("Stopped");
        }

        [HttpGet]
        [Route("api/Scheduler/Reinitialization")]
        public async Task<IHttpActionResult> Reinitialization()
        {
            UrlCache.Reinitialization(_dbContext.Urls.ToList());
            return Ok("init");
        }
    }
}
