using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using EntityState = System.Data.Entity.EntityState;

namespace WebApplication.Controllers
{
    public class AddressesController : Controller
    {
        private ShopDBContext db = new ShopDBContext();

        
        public ActionResult Index()
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            return View(db.Addresses.Where(x=>x.UserId==FrontLoginUserInfo.User.Id).ToList());
        }

        
        public ActionResult Create()
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            return View(new Address());
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Name,Phone,Addr,PostCode")] Address address)
        {
            address.CreateTime = DateTime.Now;
            address.UserId=FrontLoginUserInfo.User.Id;
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(address);
        }

        
        public ActionResult Edit(int? id)
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Name,Phone,Addr,PostCode,CreateTime")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(address);
        }

        
        public ActionResult Delete(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
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
