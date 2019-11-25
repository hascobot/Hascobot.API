using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hasco.Bot.Api.Controllers
{
    [Route("[controller]")]
    [EnableCors("angular")]
    public class ApiBaseController : Controller
    {
        protected int UserId => User?.Identity?.IsAuthenticated == true ?
            int.Parse(User.Identity.Name) : 0;
    }
}