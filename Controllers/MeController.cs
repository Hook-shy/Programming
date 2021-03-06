﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Programming.Models;
using Programming.Server;
using Programming.Utils;

namespace Programming.Controllers
{
    public class MeController : Controller
    {
        private IHostingEnvironment hostingEnvironment;
        private readonly UserContext _usercontext;
        private string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico" };

        public MeController(UserContext userContext, IHostingEnvironment environment)
        {
            _usercontext = userContext;
            hostingEnvironment = environment;
        }

        public IActionResult Info()
        {
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["Nick"] = "";
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["User"] = user;
                    ViewData["Title"] = "一站式编程学习平台|我";
                    ViewData["Controller"] = "Me";
                    ViewData["Action"] = "Info";
                    return View();
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult UpdateInfo([FromForm] UpdateInfoParameter parameter)
        {
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["Nick"] = "";
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    user.Nick = parameter.Nick == null ? "" : parameter.Nick;
                    user.Name = parameter.Name == null ? "" : parameter.Name;
                    user.Birthday = parameter.Birthday;
                    user.Sex = parameter.sex;
                    user.Synopsis = parameter.Synopsis == null ? "" : parameter.Synopsis;
                    ViewData["User"] = user;
                    ViewData["Title"] = "一站式编程学习平台|我";
                    ViewData["Controller"] = "Me";
                    ViewData["Action"] = "Info";
                    if (string.IsNullOrEmpty(parameter.Nick))
                    {
                        ViewData["Msg"] = "昵称不能为空";
                        return View("Info");
                    }
                    if (_usercontext.Users.Where(u => u.Nick != null && u.Nick.Equals(parameter.Nick) && u.Id != user.Id).Count() >= 1)
                    {
                        ViewData["Msg"] = "该昵称已经被使用了";
                        return View("Info");
                    }
                    _usercontext.Users.Update(user);
                    _usercontext.SaveChanges();
                    return View("Info");
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult UpdateImg()
        {
            StringValues file;
            Request.Form.TryGetValue("file", out file);
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    string filePath = hostingEnvironment.WebRootPath + @"\data\image";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var match = Regex.Match(HttpUtility.UrlDecode(file.ToString()), "data:image/(\\w{2,4});base64,([\\w\\W]*)$");
                    if (match.Success)
                    {
                        using (FileStream fs = System.IO.File.Create(filePath + $@"\{user.Id}.{match.Groups[1].Value}"))
                        {
                            fs.Write(Convert.FromBase64String(match.Groups[2].Value));
                            fs.Flush();
                        }
                        user.UserImgURL = $@"\data\image\{user.Id}.{match.Groups[1].Value}";
                        _usercontext.Users.Update(user);
                        _usercontext.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Info");
        }
    }
}
