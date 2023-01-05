using Eticaret.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eticaret.Controllers
{
    public class SiparisController : Controller
    {
        ETicaretEntities db=new ETicaretEntities();
        // GET: Siparis
        public ActionResult Index()
        {
            string userID=User.Identity.GetUserId();
            return View(db.Siparis.Where(x=>x.KullaniciID==userID));
           
        }
        public ActionResult SiparisTamamla()
        {
            //    ClientID: Bankadan alınan mağaza kodu
            //    Amount:Sepetteki ürünlerin toplam tutar
            //    Oid:SiparişID
            //    OnayUrl:Ödeme başarılı olduğunda gelen verilerin gösterileceği url
            //    HataUrl:Ödeme sırasında hata olduysa gelen hatanın gösterileceği url
            //    RDN:Hash karşılaştırılıması için kullanılan bilgi
            //        StoreKEy:Güvenlik anahtarı.Bankanın sanal pos sayfasından alınır.
            //        TransactionType:"Auth"
            //        Instalment:""
            //        HashStr:HashSet oluşturulurken bankanın istediği bilgiler birleştirilir.
            //        Hash:Farklı değerler oluşturulup birleştirilir.

            string userID = User.Identity.GetUserId();

            List<Sepet> sepetUrunleri = db.Sepet.Where(x => x.KullaniciID == userID).ToList();

            string ClientId = "1003001";//Bankanın verdiği magaza kodu
            string ToplamTutar = sepetUrunleri.Sum(x => x.ToplamTutar).ToString();

            string sipId = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

            string onayURL = "https://localhost:44317/Siparis/Tamamlandi&quot"; //projeye sağ tıklayıp properties den web e girince localhost bilgisi mevcut o bilgiyi buraya yazıyoruz

            string hataURL = "https://localhost:44317/Siparis/Hatali&quot";

            string RDN = "asdf";
            string StoreKey = "123456";

            string TransActionType = "Auth";
            string Instalment = "";

            string HashStr = ClientId + sipId + ToplamTutar + onayURL + hataURL + TransActionType + Instalment + RDN + StoreKey;//Bankanın istediği bilgiler

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] HashBytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(HashStr);
            byte[] InputBytes = sha.ComputeHash(HashBytes);
            string Hash = Convert.ToBase64String(InputBytes);

            ViewBag.ClientId = ClientId;
            ViewBag.Oid = sipId;
            ViewBag.okUrl = onayURL;
            ViewBag.failUrl = hataURL;
            ViewBag.TransActionType = TransActionType;
            ViewBag.RDN = RDN;
            ViewBag.Hash = Hash;
            ViewBag.Amount = ToplamTutar;
            ViewBag.StoreType = "3d_pay_hosting"; // Ödeme modelimiz
            ViewBag.Description = "";
            ViewBag.XID = "";
            ViewBag.Lang = "tr";
            ViewBag.EMail = "cenelif@gmail.com";
            ViewBag.UserID = "ElifCengiz"; // bu id yi bankanın sanala pos ekranında biz oluşturuyoruz.
            ViewBag.PostURL = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate&quot"; //ödeneme ekranına gelince bankanın kendi ödeme ekranına gönderildiği url

            return View();
        }
        public ActionResult tamamlandi()
        {
            string userID=User.Identity.GetUserId();
            Siparis siparis=new Siparis()
            {
                Ad=Request.Form.Get("Ad"),
                Soyad=Request.Form.Get("Soyad"),
                Telefon= Request.Form.Get("Telefon"),
                Adres= Request.Form.Get("Adres"),
                TCKimlikNo=Request.Form.Get("TCKimlikNo"),
                Tarih=DateTime.Now,
                KullaniciID=userID
            };

            List<Sepet> sepettekiurunler=db.Sepet.Where(x=>x.KullaniciID==userID).ToList();
            foreach(Sepet item in sepettekiurunler)
            {
                SiparisDetay sd=new SiparisDetay()
                {
                    Adet=item.Adet,
                    UrunID=item.UrunID,
                    Toplam=item.ToplamTutar
            };
                siparis.SiparisDetay.Add(sd);
                db.Sepet.Remove(item);  //sepettekileri boşaltmak için 
            }
            db.Siparis.Add(siparis);
            db.SaveChanges();
                return View();
        }
        public ActionResult Hatali()
        {
            ViewBag.hata=Request.Form; //bankanın url i 
            return View();
        }
    }
}