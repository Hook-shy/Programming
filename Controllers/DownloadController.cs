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
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["User"] = user;
                }
            }
            ViewData["Title"] = "一站式编程学习平台|下载";
            ViewData["Controller"] = "Download";
            ViewData["Action"] = "Download";
            return View();
        }
    }
}
