using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.Discovery.Eureka;

namespace AdminSystem.Test_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DiscoveryHttpClientHandler _handler;
        private const string RoleUrl = "http://loop/api/role";
        private const string HiUrl = "http://service-hi/hi";
        public TestController(IDiscoveryClient client, ILoggerFactory logFactory)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        [HttpGet("tokenAuth")]
        [Authorize(Policy = "multiClaim")]
        //[Authorize(Policy = "Token")]
        public string GetString()
        {
            return "ttt验证成功";
        }

        [HttpGet("role")]
        public async Task<string> GoProductAsync()
        {
            var client = new HttpClient(_handler, false);
            return await client.GetStringAsync(RoleUrl);
        }

        [HttpGet("hi")]
        public async Task<string> GoHiAsync()
        {
            var client = new HttpClient(_handler, false);
            return await client.GetStringAsync(HiUrl);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            DiscoveryClient _discoveryClient = new DiscoveryClient(new EurekaClientConfig
            {
                EurekaServerServiceUrls = "http://120.79.250.60:8761/eureka/",
                ProxyHost = "http://120.79.250.60:8761/eureka/",
                ProxyPort = 8761,

            });
            //得到服务中心所有服务和它的Url地址
            foreach (var item in _discoveryClient.Applications.GetRegisteredApplications())
                yield return $"{item.Name}={item.Instances.FirstOrDefault().HomePageUrl}";
        }
    }
}