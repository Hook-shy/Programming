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

        [Route("[controller]/[action]/{id=0}")]
        public IActionResult Index()
        {
            ViewData["Title"] = "一站式编程学习平台|知识竞赛";
            return View();
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
