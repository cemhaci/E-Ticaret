using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using wep_Api_Project.Models;

namespace wep_Api_Project.Controllers
{
    [System.Web.Http.RoutePrefix("api/[Kategori]")]
    public class KategoriController : ApiController
    {
       
        ETicaretEntities db = new ETicaretEntities();
        //api de 4 method vardır
        public List<Kategoriler> Get()
        {

            db.Configuration.ProxyCreationEnabled = false;
            List<Kategoriler> liste= db.Kategoriler.ToList();
            return liste;
        }
        [System.Web.Http.HttpGet]
        //[System.Web.Http.Route("Get2/Kategori/{id}")]
        public IHttpActionResult Get2(int id)
        {
           
            Kategoriler kategori=db.Kategoriler.Find(id);
            Kategori kat = new Kategori()   //kategorilerin urunleri de olduğudan kategori id yi getirdiğimizden hata veriyor o yüzden kategori classı oluşturup içinde sadece kategori bilgileri atıp api de onu gösteriyoruz
            {
                id = kategori.KategoriID,
                name = kategori.KategoriAdi
            };
            return Ok(kat);
        }
        [System.Web.Http.HttpPost]
        public IHttpActionResult post([FromBody]Kategoriler kategori)
        {
            db.Kategoriler.Add(kategori);
            db.SaveChanges();
            return Ok();
        }
    }
}
