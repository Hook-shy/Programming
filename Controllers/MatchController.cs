using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programming.Models;
using Programming.Server;

namespace Programming.Controllers
{
    public class MatchController : Controller
    {
        private readonly UserContext _usercontext;

        public MatchController(UserContext userContext)
        {
            _usercontext = userContext;
        }

        [Route("[controller]/[action]/{id=0}")]
        public IActionResult Index()
        {
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["User"] = user;
                    ViewData["Title"] = "一站式编程学习平台|知识竞赛";
                    return View();

                }
            }
            return RedirectToAction("Login", "Login");
        }

        [Route("[controller]/[action]/{id=0}")]
        public IActionResult Match()
        {
            return View();
        }

        [Route("[controller]/[action]/{id=0}")]
        public IActionResult Practice()
        {
            return View();
        }
    }
}
