using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Programming.Server
{
    public class CodeRun
    {
        public static string Start(string lang, string language, string code, string classname)
        {
            Debug.WriteLine(lang);
            Debug.WriteLine(language);
            Debug.WriteLine(code);
            Debug.WriteLine(classname);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://run.w3cschool.cn/tryit");
            request.Method = "Post";
            request.ContentType = "application/json;charset=utf-8";
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 Edg/85.0.564.68");
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            string json = JsonConvert.SerializeObject(new { lang, language, code, classname });

            myStreamWriter.Write(json);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            //JObject bupo = JsonConvert.DeserializeObject<JObject>(retString);
            
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
