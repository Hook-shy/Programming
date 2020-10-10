using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Programming.Models;
using Programming.Server;
using Programming.Utils;

namespace Programming.Controllers
{
    [Route("api/login/[action]")]
    [ApiController]
    public class APILoginController : ControllerBase
    {
        private readonly UserContext _usercontext;

        public APILoginController(UserContext userContext)
        {
            _usercontext = userContext;
        }

        [HttpGet("{mail}")]
        public IActionResult ExistMail(string mail)
        {
            return new JsonResult(ResultHelper.GetOkResult(new { mail, exist = UserServer.ExistUser(mail, _usercontext) }));
        }

        [HttpGet("{nick}")]
        public IActionResult ExistNick(string nick)
        {
            return new JsonResult(ResultHelper.GetOkResult(new { nick, exist = UserServer.ExistNick(nick, _usercontext) }));
        }

        [HttpGet("{mail}")]
        public IActionResult SendCode(string mail)
        {
            MailHelper mailHelper = new MailHelper(mail, _usercontext);
            return new JsonResult(ResultHelper.GetOkResult(new { mail, result = mailHelper.SendMail() }));
        }
    }
}
