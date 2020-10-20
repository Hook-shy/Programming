using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programming.Models;

namespace Programming.Controllers
{
    public class MatchController : Controller
    {
        private readonly UserContext _usercontext;

        public MatchController(UserContext userContext)
        {
            _usercontext = userContext;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "一站式编程学习平台|知识竞赛";
            return View();
        }
    }
}
