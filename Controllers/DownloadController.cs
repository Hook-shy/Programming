using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programming.Models;
using Programming.Server;
using Programming.Utils;

namespace Programming.Controllers
{
    public class DownloadController : Controller
    {
        private readonly UserContext _usercontext;

        public DownloadController(UserContext userContext)
        {
            _usercontext = userContext;
        }

        [Route("[controller]/[action]/{page=all}")]
        public IActionResult Download([FromRoute] string page)
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
                    ViewData["Title"] = "一站式编程学习平台|下载";
                    ViewData["Controller"] = "Download";
                    ViewData["Action"] = "Download";
                    return View();
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult UploadFile([FromForm]UploadFileParameter parameter)
        {
            Debug.WriteLine(parameter.Title);
            Debug.WriteLine(parameter.Tag);
            Debug.WriteLine(parameter.Describe);
            Debug.WriteLine(parameter.Type);
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["User"] = user;
                    ViewData["Title"] = "一站式编程学习平台|下载";
                    ViewData["Controller"] = "Download";
                    ViewData["Action"] = "Download";
                    ViewData["Tip"] = new Tip("提示", "上传文件", "上传成功", 5000);
                    return View("Download");
                }
            }
            return RedirectToAction("Login", "Login");
        }
    }
}
