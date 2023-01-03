using Eticaret.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eticaret.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        ETicaretEntities db= new ETicaretEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult sepetEkle(int id,int adet)
		{
            string kulID=User.Identity.GetUserId();  //login olan kullanıcının  idsini getir
            Sepet sepettekiurun=db.Sepet.FirstOrDefault(x=>x.UrunID==id&&x.KullaniciID==kulID);

            Urunler urun=db.Urunler.Find(id);
            if(sepettekiurun == null)
			{
                Sepet yeniurun=new Sepet()
				{
                    KullaniciID=kulID,
                    UrunID=id,
                    Adet=adet,
                    ToplamTutar=urun.UrunFiyati* adet

				};
                db.Sepet.Add(yeniurun);
			}
			else
			{
                sepettekiurun.Adet=sepettekiurun.Adet+adet;
                sepettekiurun.ToplamTutar=sepettekiurun.Adet*urun.UrunFiyati;
			}
            db.SaveChanges();
            return RedirectToAction("Index");
		}
    }
}