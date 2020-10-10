using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programming.Models;
using Programming.Server;

namespace Programming.Controllers
{
    public class DownloadController : Controller
    {
        private readonly UserContext _usercontext;

        public DownloadController(UserContext userContext)
        {
            _usercontext = userContext;
        }

        public IActionResult Download()
        {
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["Nick"] = "";
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["Nick"] = user.Nick;
                }
            }
            ViewData["Controller"] = "Download";
            ViewData["Action"] = "Download";
            return View();
        }
    }
}
