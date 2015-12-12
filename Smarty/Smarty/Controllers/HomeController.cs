using System;
using System.Net;
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
            query = query.Replace(" ", "+");
            string str = web.DownloadString("https://www.google.ru/search?q="+query);
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
    }
}