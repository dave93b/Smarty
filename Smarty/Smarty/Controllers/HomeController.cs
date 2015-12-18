using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;

namespace Smarty.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(string query)
        {

            WebClient web = new WebClient();
            query = query.Replace("+", "%2B");
            query = query.Replace(" ", "+");
            string str;
            try
            {
                str = web.DownloadString("https://www.google.ru/search?q="+query);
            }
            catch (Exception)
            {
                return Json((object)"Ошибка! Проверьте подключение к интернету.");
            }
            
            str = HttpUtility.HtmlDecode(str);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);
            string result;
            try
            {
                result = doc.DocumentNode.SelectSingleNode("//div[@class='_sPg']").InnerText;
            }
            catch (Exception)
            {
                try
                {
                    result = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'_tXc')]").InnerText;
                }
                catch (Exception)
                {
                    try
                    {
                        result = doc.DocumentNode.SelectSingleNode("//h2[contains(@class,'r')]").InnerText;
                    }
                    catch (Exception)
                    {
                        result = "К сожалению, по Вашему запросу ничего не найдено :(";
                    }
                    
                }
                
            }
            return Json(result.Replace("Википедия",""));
        }

        [HttpPost]
        public ActionResult FileData(HttpPostedFileBase upload, string searchString)
        {
            string text = "";
            using (StreamReader reader = new StreamReader(upload.InputStream,Encoding.Default))
            {
                text = reader.ReadToEnd();
            }
            string resultString = "";
            try
            {
                int resultStringIndex = text.IndexOf(searchString, StringComparison.OrdinalIgnoreCase);

                while (/*resultStringIndex != 0 || */text[resultStringIndex] != '.')
                {
                    resultStringIndex--;
                }

                resultString = text.Substring(resultStringIndex+1);

                int resultStringLastIndex = resultString.IndexOf(".");
                resultString = resultString.Remove(resultStringLastIndex + 1);
            }
            catch (Exception)
            {
                resultString = "К сожалению, по Вашему запросу ничего не найдено :(";
            }

            return View((object)resultString);
        }
    }
}