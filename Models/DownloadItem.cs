using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public enum DownloadItemType
    {
        Code = 0,
        Media = 1,
        Tool = 2,
        Doc = 3,
        Other = 4
    }

    public class DownloadItem : Model
    {
        public string UserId { get; set; }
        public bool IsPass { get; set; }
        public DateTime PassDate { get; set; }
        public DownloadItemType Type { get; set; }
        public string Describe { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public long Size { get; set; }
        public int DownloadCount { get; set; }
        public string ImgUrl { get; set; }
        public string Url { get; set; }
    }
}
