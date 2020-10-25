using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Programming.Models;
using Programming.Server;
using Programming.Utils;

namespace Programming.Controllers
{
    public class DownloadController : Controller
    {
        private IHostingEnvironment hostingEnvironment;
        private readonly UserContext _usercontext;
        private readonly DownloadItemContext _downloaditemcontext;

        public DownloadController(UserContext userContext, DownloadItemContext downloadItemContext, IHostingEnvironment environment)
        {
            _usercontext = userContext;
            _downloaditemcontext = downloadItemContext;
            hostingEnvironment = environment;
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
                    List<DownloadItem> items = _downloaditemcontext.DownloadItems.AsEnumerable().ToList();
                    ViewData["Items"] = items;
                    List<User> users = new List<User>();
                    foreach (var d in items)
                    {
                        users.Add(_usercontext.Users.AsEnumerable().FirstOrDefault(u => u.Id == d.UserId));
                    }
                    ViewData["Users"] = users;
                    return View();
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileAsync([FromForm] UploadFileParameter parameter)
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
                    List<DownloadItem> items = _downloaditemcontext.DownloadItems.AsEnumerable().ToList();
                    ViewData["Items"] = items;
                    List<User> users = new List<User>();
                    foreach (var d in items)
                    {
                        users.Add(_usercontext.Users.AsEnumerable().FirstOrDefault(u => u.Id == d.UserId));
                    }
                    ViewData["Users"] = users;

                    ViewData["Parameter"] = parameter;
                    if (parameter.Title == null || string.IsNullOrEmpty(parameter.Title.Trim()))
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "标题不能为空", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    if (parameter.Tag == null || string.IsNullOrEmpty(parameter.Tag.Trim()))
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "标签不能为空", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    if (parameter.Describe == null || string.IsNullOrEmpty(parameter.Describe.Trim()))
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "描述不能为空", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    var files = Request.Form.Files;
                    Debug.WriteLine($"文件数量{files.Count}");
                    if (files.Count != 1)
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "只能上传单个文件", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    var file = files[0];
                    long size = file.Length;
                    if (size > 50 * 1024 * 1024)
                    {
                        ViewData["Tip"] = new Tip("提示", "上传文件", "文件大小不能超过50MB", "text-danger", 5000);
                        ViewData["Tab"] = "upload-file";
                        return View("Download");
                    }
                    if (size > 0)
                    {
                        string dicPath = hostingEnvironment.WebRootPath + @$"\data\file\{user.Id}";
                        string filePath = $@"{dicPath}\{file.FileName}";
                        if (!Directory.Exists(dicPath))
                        {
                            Directory.CreateDirectory(dicPath);
                        }
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }
                        DownloadItem downloadItem = new DownloadItem
                        {
                            Describe = parameter.Describe,
                            Size = size,
                            Tag = parameter.Tag,
                            Title = parameter.Title,
                            Url = @$"\data\file\{user.Id}\{file.FileName}",
                            Type = parameter.Type,
                            UserId = user.Id,
                            ImgUrl = $"/image/resources_{parameter.Type.ToString().ToLower()}.png"
                        };
                        _downloaditemcontext.DownloadItems.Add(downloadItem);
                        _downloaditemcontext.SaveChanges();
                        ViewData["Tip"] = new Tip("提示", "上传文件", "上传成功", "text-success", 5000);
                        ViewData["Parameter"] = null;
                        items.Add(downloadItem);
                        users.Add(user);
                        return View("Download");
                    }
                    ViewData["Tip"] = new Tip("提示", "上传文件", "上传失败", "text-danger", 5000);
                    ViewData["Tab"] = "upload-file";
                    return View("Download");
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult DownloadFile([FromRoute]int id)
        {
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    DownloadItem downloadItem = _downloaditemcontext.DownloadItems.AsEnumerable().FirstOrDefault(d => d.Id == id);
                    if (downloadItem != null)
                    {
                        string path = hostingEnvironment.WebRootPath + downloadItem.Url;
                        FileStream stream = System.IO.File.OpenRead(path);
                        string fileExt = Path.GetExtension(path);
                        downloadItem.DownloadCount++;
                        _downloaditemcontext.DownloadItems.Update(downloadItem);
                        _downloaditemcontext.SaveChanges();
                        return File(stream, "application/octet-stream", Path.GetFileName(path));
                    }
                    return RedirectToAction("Download");
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult DeleteFile([FromRoute]int id)
        {
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    DownloadItem downloadItem = _downloaditemcontext.DownloadItems.AsEnumerable().FirstOrDefault(d => d.Id == id);
                    if(downloadItem != null)
                    {
                        _downloaditemcontext.DownloadItems.Remove(downloadItem);
                        _downloaditemcontext.SaveChanges();
                        string path = hostingEnvironment.WebRootPath + downloadItem.Url;
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    return RedirectToAction("Download");
                }
            }
            return RedirectToAction("Login", "Login");
        }
    }
}
