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
            string kulID=User.Identity.GetUserId();
           
            return View(db.Sepet.Where(x=>x.KullaniciID==kulID).ToList());
        }
        public ActionResult sepetEkle(int urunid,int adet)
		{
            string kulID=User.Identity.GetUserId();  //login olan kullanıcının  idsini getir
            Sepet sepettekiurun=db.Sepet.FirstOrDefault(x=>x.UrunID==urunid&&x.KullaniciID==kulID);

            Urunler urun=db.Urunler.Find(urunid);
            if(sepettekiurun == null)
			{
                Sepet yeniurun=new Sepet()
				{
                    KullaniciID=kulID,
                    UrunID=urunid,
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

        public ActionResult sepetGuncelle(int? id, int adet)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Sepet sepet =db.Sepet.Find(id);
            if(sepet == null)
            {
                return HttpNotFound();
            }
            Urunler urun=db.Urunler.Find(sepet.UrunID);
            sepet.Adet=adet;
            sepet.ToplamTutar=sepet.Adet*urun.UrunFiyati;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult delete (int id)
        {
            Sepet sepet = db.Sepet.Find(id);
            db.Sepet.Remove(sepet); //sepetteki satırı siliyoruz
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}