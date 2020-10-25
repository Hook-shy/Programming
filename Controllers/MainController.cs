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
        private readonly ArticleContext _articlecontext;
        private readonly DownloadItemContext _downloaditemcontext;

        public MainController(UserContext userContext, ArticleContext articleContext, DownloadItemContext downloadItemContext)
        {
            _usercontext = userContext;
            _articlecontext = articleContext;
            _downloaditemcontext = downloadItemContext;
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
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["User"] = user;
                }
            }
            ViewData["Title"] = "一站式编程学习平台|首页";
            int count = _articlecontext.Articles.AsEnumerable().Count();
            ViewData["NewArticleList"] = _articlecontext.Articles.AsEnumerable().OrderBy(a => a.CreateDate).Reverse().ToList().GetRange(0, count > 10 ? 10 : count);
            ViewData["HotArticleList"] = _articlecontext.Articles.AsEnumerable().OrderBy(a => a.LookCount).Reverse().ToList().GetRange(0, count > 10 ? 10 : count);
            count = _downloaditemcontext.DownloadItems.AsEnumerable().Count();
            ViewData["DownloadItem"] = _downloaditemcontext.DownloadItems.AsEnumerable().OrderBy(d => d.CreateDate).Reverse().ToList().GetRange(0, count > 10 ? 10 : count);
            ViewData["Controller"] = "Main";
            ViewData["Action"] = "Index";
            return View();
        }
    }
}
