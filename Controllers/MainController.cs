using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Programming.Models;
using Programming.Server;

namespace Programming.Controllers
{
    public class MainController : Controller
    {
        private readonly UserContext _usercontext;

        public MainController(UserContext userContext)
        {
            _usercontext = userContext;
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("SessionCode");
            return RedirectToAction("Index", "Main");
        }

        public IActionResult Index()
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
            ViewData["Controller"] = "Main";
            ViewData["Action"] = "Index";
            return View();
        }
    }
}
