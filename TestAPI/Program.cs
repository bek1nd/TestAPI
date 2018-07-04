using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace TestAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string staticInfo;
            //2.1获取酒店清单
            staticInfo = SearchStaticInfo(24);

            //2.2获取酒店静态信息
           // staticInfo = SearchStaticInfo(22);

            //2.3房型静态信息Room Descriptive Info
            //staticInfo = SearchStaticInfo(23);

            //2.4酒店及房型静态信息增量接口新增GetChangeInfo
            Console.WriteLine(staticInfo);
            Console.ReadKey();
        }

        static string SearchStaticInfo(int num)
        {
            string repJson;
            string urlBase = "http://openservice.open.uat.ctripqa.com/openservice/serviceproxy.ashx?" +
                "AID=1&SID=50&UUID=f71dee19-9e09-42d9-a39c-40757d9d76e4&mode=1&format=JSON&Token=";
            string pathBase = "C:\\Users\\qian\\Source\\Repos\\TestAPI\\TestAPI\\";
            JObject icodeJson = (JObject)JsonConvert.DeserializeObject(GetFileJson(pathBase + "ICODE.json"));
            switch(num)
            {
                case 21:
                    repJson = Post(GetFileJson(pathBase + "json21.json"), urlBase + GetToken() + "&ICODE=" + icodeJson["21"].ToString());
                    break;
                case 22:
                    repJson = Post(GetFileJson(pathBase + "json22.json"), urlBase + GetToken()+ "&ICODE=" + icodeJson["22"].ToString());
                    break;
                case 23:
                    repJson = Post(GetFileJson(pathBase + "json23.json"), urlBase + GetToken()+ "&ICODE=" + icodeJson["23"].ToString());
                    break;
                case 24:
                    repJson = Post(GetFileJson(pathBase + "json24.json"), urlBase + GetToken()+ "&ICODE=" + icodeJson["24"].ToString());
                    break;
                case 25:
                    repJson = Post(GetFileJson(pathBase + "json25.json"), urlBase + GetToken()+ "&ICODE=" + icodeJson["25"].ToString());
                    break;
                default:
                    repJson = null;
                    break;
            }
            return repJson;
        }

        static string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }

        public static string GetToken()
        {
            HttpWebRequest request = WebRequest.Create("http://openservice.open.uat.ctripqa.com/openserviceauth/authorize.ashx?AID=1&SID=50&KEY=123456789") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream s = (request.GetResponse() as HttpWebResponse).GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                JObject json = (JObject)JsonConvert.DeserializeObject(reader.ReadToEnd());
                return json["Access_Token"].ToString();
            }
        }

        public static string Post(string postData,string url )
        {
            string result = "";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.Method = "POST";

            req.ContentType = "application/json";

            byte[] data = Encoding.UTF8.GetBytes(postData);

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);

                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            Stream stream = resp.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
