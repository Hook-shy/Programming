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
                    ViewData["Tab"] = page;
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
                    if(parameter.Title == null || string.IsNullOrEmpty(parameter.Title.Trim()))
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "名称不能为空", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    if(parameter.Tag == null || string.IsNullOrEmpty(parameter.Tag.Trim()))
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "标签不能为空", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    if(parameter.Describe == null || string.IsNullOrEmpty(parameter.Describe.Trim()))
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "描述不能为空", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    ViewData["Tip"] = new Tip("提示", "上传文件", "上传成功", "text-success", 5000);
                    return View("Download");
                }
            }
            return RedirectToAction("Login", "Login");
        }
    }
}
