﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Eticaret.Models;
using Newtonsoft.Json;

namespace Eticaret.Controllers
{
    public class KategorilerController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
        HttpClient client = new HttpClient();
        List<Kategoriler> kategoriler=new List<Kategoriler>();
        // GET: Kategoriler
        public ActionResult Index()
        {
            client.BaseAddress=new Uri("https://localhost:44305/api/");
            var response= client.GetAsync("Kategori"); //api deki get i çağrmak için async yaptık
            response.Wait();  //async methodlarda genelde wait lememiz gerekir
            var result=response.Result;
            if(result.IsSuccessStatusCode) //sonuc başarılı ise
            {
                var data=result.Content.ReadAsStringAsync();  //gelen json bilgisini string olarak okuyor
                data.Wait();
               kategoriler= JsonConvert.DeserializeObject<List<Kategoriler>>(data.Result);  //string olaraka okunan bilgiyi jsona çeviriyor deserilize ile. ve bunu view de gösteriyor
            }
            return View(kategoriler);
        }

        // GET: Kategoriler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // GET: Kategoriler/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategoriler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Kategoriler kategoriler)
        {
            if (ModelState.IsValid)
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/");
                var response=HttpClientExtensions.PostAsJsonAsync
                //db.Kategoriler.Add(kategoriler);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategoriler);
        }

        // GET: Kategoriler/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // POST: Kategoriler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategoriID,KategoriAdi")] Kategoriler kategoriler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategoriler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategoriler);
        }

        // GET: Kategoriler/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return HttpNotFound();
            }
            return View(kategoriler);
        }

        // POST: Kategoriler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            db.Kategoriler.Remove(kategoriler);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
