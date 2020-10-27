using Programming.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming.Server
{
    public static class UserServer
    {
        public static bool ExistUser(string mail, UserContext userContext)
        {
            return userContext.Users.AsEnumerable().FirstOrDefault<User>(u => u.Mail.Equals(mail.ToLower()) && !string.IsNullOrEmpty(u.Password)) != null;
        }

        public static bool ExistNick(string nick, UserContext userContext)
        {
            return userContext.Users.AsEnumerable().FirstOrDefault<User>(u => u.Nick != null && u.Nick.Equals(nick)) != null;
        }

        private static void UpdateSession(User user)
        {
            Random ran = new Random();
            StringBuilder code = new StringBuilder(user.Id.ToString());
            while(code.Length < 20)
            {
                code.Append(ran.Next(0, 9).ToString());
            }
            user.SessionCode = code.ToString();
            user.SessionEnd = DateTime.Now.AddDays(30);
        }

        private static void UpdateCode(User user)
        {
            Random ran = new Random();
            StringBuilder code = new StringBuilder();
            while (code.Length < 6)
            {
                code.Append(ran.Next(0, 9).ToString());
            }
            user.VerificationCode = code.ToString();
            user.VerificationEnd = DateTime.Now.AddMinutes(5);
        }

        public static string GetVerificationCode(string mail, UserContext userContext)
        {
            User user = userContext.Users.AsEnumerable().FirstOrDefault<User>(u => u.Mail.Equals(mail));
            if (user == null)
            {
                user = new User
                {
                    Mail = mail
                };
                UpdateCode(user);
                userContext.Users.Add(user);
                userContext.SaveChanges();
            }
            else
            {
                if(user.VerificationEnd < DateTime.Now)
                {
                    UpdateCode(user);
                    userContext.Update(user);
                    userContext.SaveChanges();
                }
            }
            return user.VerificationCode;
        }

        public static bool CheckVerificationCode(string code, UserContext userContext)
        {
            return userContext.Users.AsEnumerable().FirstOrDefault(u => u.VerificationCode != null && u.VerificationCode.Equals(code) && u.VerificationEnd > DateTime.Now) != null;
        }

        public static User GetLoginResult(string mail, string pwd, UserContext userContext)
        {
            User result = userContext.Users.AsEnumerable<User>().FirstOrDefault<User>(u => u.Mail.Equals(mail) && u.Password!=null && u.Password.Equals(pwd));
            if(result != null)
            {
                UpdateSession(result);
                userContext.Users.Update(result);
                userContext.SaveChanges();
            }
            return result;
        }

        public static User CheckSessionCode(string code, UserContext userContext)
        {
            if(code.Length != 20)
            {
                return null;
            }
            return userContext.Users.AsEnumerable<User>().FirstOrDefault<User>(u =>u.SessionCode != null && u.SessionCode.Equals(code) && u.SessionEnd.Ticks > DateTime.Now.Ticks);
        }
    }
}
