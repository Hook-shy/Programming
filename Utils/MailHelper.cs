using Programming.Models;
using Programming.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Programming.Utils
{
    /// <summary>
    /// 邮件操作类
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// 实例化邮件操作对象
        /// </summary>
        /// <param name="mail">收件人</param>
        public MailHelper(string mail, UserContext userContext)
        {
            To = mail;
            MailBody = $"验证码：{UserServer.GetVerificationCode(mail, userContext)}";
        }

        /// <summary>
        /// 发送人
        /// </summary>
        public string From { get; set; } = "s2206034346@163.com";

        public string FromName { get; set; } = "一站式编程学习平台";

        /// <summary>
        /// 收件人
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string MailTitle { get; set; } = "一站式编程学习平台验证码";

        /// <summary>
        /// 正文
        /// </summary>
        public string MailBody { get; set; }

        /// <summary>
        /// 客户端授权码
        /// </summary>
        public string Code { get; set; } = "IAVEGDPLRCJFEVON";//"MWKHTPVQUQXOAZKO";

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string Host { get; set; } = "smtp.163.com";

        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool Ishtml { get; set; } = false;

        public string SendMail()
        {
            //实例化Smtp客户端
            SmtpClient smtp = new SmtpClient
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Host = Host,
                Credentials = new System.Net.NetworkCredential(From, Code)
            };

            MailMessage mailMessage = new MailMessage($"\"{FromName}\"{From}", To)
            {

                Subject = MailTitle,
                SubjectEncoding = Encoding.UTF8,
                Body = MailBody,
                BodyEncoding = Encoding.Default,
                //邮件优先级
                Priority = MailPriority.Normal,
                IsBodyHtml = Ishtml
            };

            System.Diagnostics.Debug.WriteLine($"from:{From}\nto:{To}\npwd:{Code}\nhost:{Host}\ntitle:{MailTitle}\nbody:{MailBody}");
            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                return $"Message:{ex.Message}\nStackTrace:{ex.StackTrace}\nInnerException:{ex.InnerException}\nSource:{ex.Source}\nData:{ex.Data}\nHResult:{ex.HResult}\nTargetSite:{ex.TargetSite}";
            }
            return "true";
        }
    }
}
