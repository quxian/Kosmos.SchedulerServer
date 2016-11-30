using Extended;
using Kosmos.SchedulerServer.DbContext;
using Kosmos.SchedulerServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kosmos.SchedulerServer.Controllers {
    public class UrlController : ApiController {
        private readonly AppDbContext _dbContext;

        public UrlController(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(string url, string parent, int depth = 0) {
            if (null == url || parent == null)
                return BadRequest("url 和 parent 不允许为空");

            try {
                if (new Uri(url).Host != new Uri(parent).Host)
                    return BadRequest("url 和 parent不在一个域下");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

            var urlHashCode = url.GetMD5HashCode();
            try {
                if (null != _dbContext.Urls?.AsParallel().FirstOrDefault(x => x.HashCode == urlHashCode))
                    return Ok(url);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

            _dbContext.Urls.Add(new Model.Url {
                IsDownloaded = false,
                HashCode = urlHashCode,
                Value = url,
                Depth = depth,
                Domain = new Uri(url).Host,
                Parent = parent
            });
            await _dbContext.SaveChangesAsync();

            return Ok(url);
        }

        public async Task<IHttpActionResult> Post(List<Model.Url> urls) {
            try {
                var dbUrls = urls
                    .AsParallel()
                    .Where(url => {
                        if (null == url.Value || null == url.Parent)
                            return false;
                        try {
                            if (new Uri(url.Value).Host != new Uri(url.Parent).Host)
                                return false;
                        } catch (Exception) {
                            return false;
                        }
                        return true;
                    })
                    .Select(url => new Model.Url {
                        IsDownloaded = false,
                        HashCode = url.Value.GetMD5HashCode(),
                        Value = url.Value,
                        Depth = url.Depth,
                        Domain = new Uri(url.Value).Host,
                        Parent = url.Parent
                    });
                var exceptUrls = dbUrls.Except(_dbContext.Urls.AsParallel()).ToList();
                _dbContext.Urls.AddRange(exceptUrls);
                await _dbContext.SaveChangesAsync();

                return Ok(exceptUrls);
            } catch (Exception e) {

                return BadRequest(e.Message);
            }
        }
    }
}
