using Eticaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eticaret.Controllers
{
	public class HomeController : Controller
	{
		ETicaretEntities db= new ETicaretEntities();
		public ActionResult Index()
		{
			ViewBag.kategoriler=db.Kategoriler;
			ViewBag.urunler=db.Urunler;

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