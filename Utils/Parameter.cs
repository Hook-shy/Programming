using Programming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Utils
{
    public class LoginParameter
    {
        private string mail;
        public string Mail { get => this.mail; set => this.mail = value.ToLower(); }
        public string Password { get; set; }
    }

    public class RegisterParameter:LoginParameter
    {
        public string Nick { get; set; }
        public string Code { get; set; }
    }

    public class CodeParameter
    {
        public string Lang { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
        public string Classname { get; set; }
    }

    public class ArticleEditSubmitParameter
    {
        public string Title { get; set; }
        public string KeyWord { get; set; }
        public string Family { get; set; }
        public string IsOriginal { get; set; }
        public string Content { get; set; }
    }

    public class CommentSubmitParameter
    {
        public string Comment { get; set; }
        public long ArticleId { get; set; }
    }

    public class UpdateInfoParameter
    {
        public string Nick { get; set; }
        public string Name { get; set; }
        public Sex sex { get; set; }
        public DateTime Birthday { get; set; }
        public string Synopsis { get; set; }
    }

    public class UploadFileParameter
    {
        public string Title { get; set; }
        public string Describe { get; set; }
        public string Tag { get; set; }
        public DownloadItemType Type { get; set; }
    }
}
