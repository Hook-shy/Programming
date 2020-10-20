using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Programming.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "一站式编程学习平台|管理";
            return View();
        }
    }
}
