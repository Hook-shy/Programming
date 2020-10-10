using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Programming.Models
{
    public enum UserType
    {
        General = 0,
        Admin = 1,
        Super = 2
    }

    public enum Sex
    {
        Man = 0,
        Woman = 1,
        Secret = 3,
        Unknown = 4
    }

    /// <summary>
    /// 用户模型
    /// </summary>
    public class User : Model
    {
        private string mail;

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(50)]
        public string Mail { get => mail; set => this.mail = value.ToLower(); }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(20)]
        [MinLength(2)]
        public string Nick { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(20)]
        [MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(10)]
        [MinLength(2)]
        public string Name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; } = Sex.Unknown;

        /// <summary>
        /// 简介
        /// </summary>
        [StringLength(100)]
        public string Synopsis { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        [StringLength(20)]
        public string SessionCode { get; set; }

        /// <summary>
        /// 会话过期时间
        /// </summary>
        public DateTime SessionEnd { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [StringLength(6)]
        public string VerificationCode { get; set; }

        /// <summary>
        /// 验证码过期时间
        /// </summary>
        public DateTime VerificationEnd { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [NotNull]
        public UserType UserType { get; set; } = UserType.General;

        /// <summary>
        /// 用户头像地址
        /// </summary>
        [MaxLength(100)]
        public string UserImgURL { get; set; }
    }
}
