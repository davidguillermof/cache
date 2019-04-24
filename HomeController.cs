using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using StackExchange.Redis;

namespace TestRedisCache.Controllers
{
    public class HomeController : Controller
    {
        public Lazy<ConnectionMultiplexer> connection;
        public IDatabase cache;

        public HomeController()
        {
            connection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
        }

        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult Index(string txtCommand, string txtString, string txtStringSet, string txtStringGet)
        {
            cache = connection.Value.GetDatabase();
            if (txtCommand != null && txtCommand.Trim().Length > 0)
            {
                ViewBag.Result = cache.Execute(txtCommand).ToString();
            }
            else if (txtStringSet != null && txtStringSet.Trim().Length > 0)
            {
                ViewBag.Result = cache.StringSet(txtString, txtStringSet);
            }
            else if (txtStringGet != null && txtStringGet.Trim().Length > 0)
            {
                ViewBag.Result = cache.StringGet(txtStringGet);
            }

            connection.Value.Dispose();

            return View();
        }
    }
}