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
            return View();
        }
    }
}
