using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hasco.Bot.Api.Controllers
{
    [ApiController]
    [Route("controller")]
    public class testController : ControllerBase
    {
        public ContentResult Index()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><body>Hello World</body></html>"
            };
        }
    }
}