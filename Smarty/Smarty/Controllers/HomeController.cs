using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public ActionResult Index(string query)
        {

            WebClient web = new System.Net.WebClient();
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
                result = "К сожалению, по Вашему запросу ничего не найдено :(";
            }
//result = doc.DocumentNode.SelectSingleNode("//div[@class='kno-rdesc']//*").InnerText;
            ViewBag.Message = result;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}