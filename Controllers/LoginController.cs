using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Programming.Models;
using Programming.Server;
using Programming.Utils;

namespace Programming.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserContext _usercontext;

        public LoginController(UserContext userContext)
        {
            _usercontext = userContext;
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginParameter parameter)
        {
            User user = UserServer.GetLoginResult(parameter.Mail, parameter.Password, _usercontext);
            if (user == null)
            {
                ViewData["info"] = "账号或密码错误";
                ViewData["Mail"] = parameter.Mail;
                ViewData["Pwd"] = parameter.Password;
                return View();
            }
            else
            {
                Response.Cookies.Delete("SessionCode");
                Response.Cookies.Append("SessionCode", user.SessionCode);
                return RedirectToAction("Index", "Main");
            }
        }

        public IActionResult Login()
        {
            string sessionCode;
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            if (!string.IsNullOrEmpty(sessionCode))
            {
                if (UserServer.CheckSessionCode(sessionCode, _usercontext) != null)
                {
                    return RedirectToAction("Index", "Main");
                }
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm]RegisterParameter parameter)
        {
            ViewData["Nick"] = parameter.Nick;
            ViewData["Mail"] = parameter.Mail;
            ViewData["Pwd"] = parameter.Password;
            ViewData["Code"] = parameter.Code;
            ViewData["NickMsg"] = null;
            ViewData["MailMsg"] = null;
            ViewData["PwdMsg"] = null;
            ViewData["CodeMsg"] = null;
            if (parameter.Nick.Length < 2 || parameter.Nick.Length > 20 || new Regex("[~#^$@%&!*()<>:;'\"{ }【】  ]").IsMatch(parameter.Nick))
            {
                ViewData["NickMsg"] = "长度2-20位，不含特殊符号";
                return View();
            }
            if (UserServer.ExistNick(parameter.Nick, _usercontext))
            {
                ViewData["NickMsg"] = "该昵称已存在";
                return View();
            }
            Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            if (r.IsMatch(parameter.Mail))
            {
                if(UserServer.ExistUser(parameter.Mail, _usercontext))
                {
                    ViewData["MailMsg"] = "该邮箱已被使用";
                    return View();
                }
            }
            else
            {
                ViewData["MailMsg"] = "邮箱格式不正确";
                return View();
            }
            Regex rePwd = new Regex("^[0-9a-zA-Z]{6,20}$");
            if (!rePwd.IsMatch(parameter.Password))
            {
                ViewData["PwdMsg"] = "密码格式不正确";
                return View();
            }
            if(!UserServer.CheckVerificationCode(parameter.Code, _usercontext))
            {
                ViewData["CodeMsg"] = "验证码不正确";
                return View();
            }
            User user = _usercontext.Users.AsEnumerable().FirstOrDefault(u => u.Mail.Equals(parameter.Mail) && u.CreateDate < new DateTime(2000, 1, 1));
            if(user != null)
            {
                user.Nick = parameter.Nick;
                user.CreateDate = DateTime.Now;
                user.Password = parameter.Password;
                _usercontext.Users.Update(user);
                _usercontext.SaveChanges();
            }
            else
            {
                return View();
            }
            return View("Login");
        }
    }
}
