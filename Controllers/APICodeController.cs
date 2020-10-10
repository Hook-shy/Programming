using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Programming.Server;
using Programming.Utils;

namespace Programming.Controllers
{
    [Route("api/code/[action]")]
    [ApiController]
    public class APICodeController : ControllerBase
    {
        [HttpPost]
        public IActionResult Run([FromBody]CodeParameter parameter)
        {
            return new JsonResult(ResultHelper.GetOkResult(new { result = CodeRun.Start(parameter.Lang, parameter.Language, parameter.Code, parameter.Classname) }));
        }
    }
}
