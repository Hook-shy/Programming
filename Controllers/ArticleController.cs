using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programming.Models;
using Programming.Server;
using Programming.Utils;

namespace Programming.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserContext _usercontext;
        private readonly ArticleContext _articlecontext;
        private readonly CommentContext _commentcontext;

        public ArticleController(UserContext userContext, ArticleContext articleContext, CommentContext commentContext)
        {
            _usercontext = userContext;
            _articlecontext = articleContext;
            _commentcontext = commentContext;
        }

        [Route("[controller]/[action]/{key=new}")]
        public IActionResult Article([FromRoute] string key)
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
            int count = _articlecontext.Articles.AsEnumerable().Count();
            ViewData["NewArticleList"] = _articlecontext.Articles.AsEnumerable().OrderBy(a => a.CreateDate).Reverse().ToList().GetRange(0, count > 10 ? 10 : count);
            ViewData["HotArticleList"] = _articlecontext.Articles.AsEnumerable().OrderBy(a => a.LookCount).Reverse().ToList().GetRange(0, count > 10 ? 10 : count);
            if (key.Equals("new"))
            {
                ViewData["ArticleList"] = ViewData["NewArticleList"];
            }
            else if (key.Equals("hot"))
            {
                count = _articlecontext.Articles.AsEnumerable().Count();
                ViewData["ArticleList"] = ViewData["HotArticleList"];
            }
            else
            {
                List<Article> articles = _articlecontext.Articles.AsEnumerable().Where(a => a.Title.IndexOf(key) >= 0 || a.KeyWord.IndexOf(key) >= 0 || a.Family.IndexOf(key) >= 0 || a.Content.IndexOf(key) >= 0).OrderBy(a => a.LookCount).Reverse().ToList();
                ViewData["ArticleList"] = articles.GetRange(0, articles.Count > 10 ? 10 : articles.Count);
            }
            ViewData["Title"] = "一站式编程学习平台|文章";
            ViewData["Controller"] = "Article";
            ViewData["Action"] = "Article";
            return View();
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult Read([FromRoute] int id)
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
            Article article = _articlecontext.Articles.AsEnumerable().FirstOrDefault(a => a.Id == id);
            article.LookCount++;
            _articlecontext.Articles.Update(article);
            _articlecontext.SaveChanges();
            ViewData["Article"] = article;
            int commentCount = _commentcontext.Comments.AsEnumerable().Where(c => c.ArticleId == article.Id).Count();
            ViewData["CommentCount"] = commentCount;
            List<Comment> commments = _commentcontext.Comments.AsEnumerable().Where(c => c.ArticleId == article.Id).OrderBy(c => c.CreateDate).Reverse().ToList().GetRange(0, commentCount > 10 ? 10 : commentCount);
            ViewData["Comments"] = commments;
            List<User> users = new List<User>();
            foreach (Comment comment in commments)
            {
                users.Add(_usercontext.Users.AsEnumerable().FirstOrDefault(u => u.Id == comment.UserId));
            }
            ViewData["Users"] = users;
            ViewData["Title"] = "一站式编程学习平台|文章";
            ViewData["Controller"] = "Article";
            ViewData["Action"] = "Read";
            return View();
        }

        public IActionResult Editor()
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
            ViewData["Title"] = "一站式编程学习平台|写文章";
            ViewData["Controller"] = "Article";
            ViewData["Action"] = "Editor";
            return View();
        }

        [HttpPost]
        public IActionResult ArticleSubmit([FromForm] ArticleEditSubmitParameter parameter)
        {
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["Nick"] = user.Nick;
                    ViewData["ATitle"] = parameter.Title;
                    ViewData["KeyWord"] = parameter.KeyWord;
                    ViewData["IsOriginal"] = parameter.IsOriginal == null ? false : parameter.IsOriginal.Equals("on");
                    ViewData["Content"] = parameter.Content;
                    ViewData["Family"] = parameter.Family;
                    if (parameter.Title == null || String.IsNullOrEmpty(parameter.Title.Trim()))
                    {
                        ViewData["Msg"] = "标题不能为空";
                        return View("Editor");
                    }
                    if (parameter.KeyWord == null || String.IsNullOrEmpty(parameter.KeyWord.Trim()))
                    {
                        ViewData["Msg"] = "关键词不能为空";
                        return View("Editor");
                    }
                    if (parameter.Content == null || String.IsNullOrEmpty(parameter.Content.Trim()))
                    {
                        ViewData["Msg"] = "内容不能为空";
                        return View("Editor");
                    }
                    Article article = new Article
                    {
                        IsOriginal = parameter.IsOriginal != null && parameter.IsOriginal.Equals("on"),
                        Title = parameter.Title,
                        Content = parameter.Content,
                        KeyWord = parameter.KeyWord,
                        Family = parameter.Family,
                        User = user.Nick,
                        UserId = user.Id,
                    };
                    _articlecontext.Articles.Add(article);
                    _articlecontext.SaveChanges();
                    return RedirectToAction("Article");
                }
            }
            ViewData["Controller"] = "Article";
            ViewData["Action"] = "Editor";
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult CommentSubmit([FromForm] CommentSubmitParameter parameter)
        {
            Debug.WriteLine(parameter.ArticleId);
            Debug.WriteLine(parameter.Comment);
            string sessionCode = "";
            Request.Cookies.TryGetValue("SessionCode", out sessionCode);
            ViewData["User"] = null;
            if (!string.IsNullOrEmpty(sessionCode))
            {
                User user = UserServer.CheckSessionCode(sessionCode, _usercontext);
                if (user != null)
                {
                    ViewData["User"] = user;
                    if (parameter.Comment != null && !String.IsNullOrEmpty(parameter.Comment))
                    {
                        Comment comment = new Comment
                        {
                            ArticleId = parameter.ArticleId,
                            Content = parameter.Comment,
                            UserId = user.Id
                        };
                        _commentcontext.Comments.Add(comment);
                        _commentcontext.SaveChanges();
                        Article article = _articlecontext.Articles.FirstOrDefault(a => a.Id == parameter.ArticleId);
                        article.CommentCount = _commentcontext.Comments.Where(c => c.ArticleId == article.Id).Count();
                        _articlecontext.Articles.Update(article);
                        _articlecontext.SaveChanges();
                    }
                }
            }
            ViewData["Controller"] = "Article";
            ViewData["Action"] = "Read";
            return RedirectToAction("Read", new { id = parameter.ArticleId });
        }
    }
}
