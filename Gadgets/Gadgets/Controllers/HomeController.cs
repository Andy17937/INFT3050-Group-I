using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using System.Data.Entity;
using System.Net;
using System.Drawing.Printing;
using System.IO;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ShopDBContext db = new ShopDBContext();
        public ActionResult Index(int pageIndex = 1, int pageSize = 8)
        {
            var key = Request.QueryString["keywords"];
            var id = Request.QueryString["id"];
            var query = db.Products.Include(p => p.Category).AsQueryable();
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(x => x.Title.Contains(key));
            }
            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(x => x.CategoryId.ToString()==id);
            }
            ViewBag.total = query.Count();
            ViewBag.pageIndex = pageIndex;
            ViewBag.categoryList = db.Categories.ToList();
            return View(query.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }
        public ActionResult ProduntsInfo(int id)
        {
           
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.counts = db.OrderDetails.Where(x => x.ProductId == id).Select(x=>x.OrderId).Distinct().Count();
            return View(product);
        }
        public ActionResult AddCart(int Counts=1)
        {
            var id = Convert.ToInt32(Request["id"]);
            var proinfo = db.Products.FirstOrDefault(p => p.Id == id);
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            var cart = db.ShoppingCarts.FirstOrDefault(s => s.UserId == FrontLoginUserInfo.User.Id && s.ProductId == proinfo.Id);
            if (cart == null)
            {
                var model = new ShoppingCart
                {
                    CreateTime = DateTime.Now,
                    UserId = FrontLoginUserInfo.User.Id,
                    Price = proinfo.Price,
                    ProductId = proinfo.Id,
                    Counts = Counts
                };
                db.ShoppingCarts.Add(model);
            }
            else
            {
                cart.Counts += Counts;
            }
            db.SaveChanges();

            return Content("<script>alert('Successfully added shopping cart！');history.go(-1);</script>");


        }
        public ActionResult Cart()
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            
            var cartList = db.ShoppingCarts.Include(x=>x.Product).Where(s => s.UserId == FrontLoginUserInfo.User.Id);
            if(db.Addresses.Any(x=>x.UserId == FrontLoginUserInfo.User.Id))
            {
                ViewBag.Addresses = db.Addresses.Where(x => x.UserId == FrontLoginUserInfo.User.Id).ToList();
            }
            else
            {
                ViewBag.Addresses=new List<Address>();
            }
            return View(cartList);
        }
        public ActionResult DelCart(int id)
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            var model = db.ShoppingCarts.FirstOrDefault(l => l.Id == id);
            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Content("<script>alert('Delete successful！');location.href='/Home/Cart';</script>");

        }
        public ActionResult ClearCart()
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            var cartList = db.ShoppingCarts.Where(s => s.UserId==FrontLoginUserInfo.User.Id);
            db.ShoppingCarts.RemoveRange(cartList);
            db.SaveChanges();
            return Redirect("Cart");
        }
        public ActionResult min(int id)
        {
            var entity = db.ShoppingCarts.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                if (entity.Counts > 1)
                {
                    entity.Counts -= 1;
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

            }
            return Redirect("Cart");
        }
        public ActionResult plus(int id)
        {
            var entity = db.ShoppingCarts.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                entity.Counts += 1;
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Redirect("Cart");
        }
        public ActionResult SubmitOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitOrder(int AddressId)
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            int uid = FrontLoginUserInfo.User.Id;
            var cartList = db.ShoppingCarts.Include(x=>x.Product).Where(s => s.UserId == uid);
            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            decimal totalMoney = 0;
            foreach (var item in cartList)
            {
                orderDetailList.Add(new OrderDetail
                {
                    Price = item.Product.Price,
                    ProductId = item.ProductId,
                    Counts = item.Counts,
                });
                totalMoney += item.Counts * item.Product.Price;
            }
            Order entity=new Order();
            entity.CreateTime = DateTime.Now;
            entity.UserId = uid;
            entity.TotalPrice = totalMoney;
            entity.Status = "Ordered";

            var addr = db.Addresses.Find(AddressId);

            entity.Address = addr.Addr;
            entity.Name= addr.Name;
            entity.PostCode = addr.PostCode;
            entity.Mobile = addr.Phone;

            db.Entry(entity).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            orderDetailList.ForEach(d => d.OrderId = entity.Id);

            db.OrderDetails.AddRange(orderDetailList);
            db.SaveChanges();

            db.ShoppingCarts.RemoveRange(cartList);
            db.SaveChanges();

            return Content("<script>alert('Order submitted successfully！');location.href='/Home/Pay?id="+entity.Id+"';</script>");


        }
        public ActionResult OrderList(int pageIndex = 1, int pageSize = 10)
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            
            var query = db.Orders.Where(l => l.UserId == FrontLoginUserInfo.User.Id).AsQueryable();
            
            ViewBag.total = query.Count();
            ViewBag.pageIndex = pageIndex;
            return View(query.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }
        public ActionResult OrderDetailList(int id)
        {
            return View(db.OrderDetails.Include(x=>x.Product).Where(x=>x.OrderId == id).ToList());
        }
        public ActionResult Pay(int id)
        {
            return View(db.Orders.Find(id));
        }
        [HttpPost]
        public ActionResult Pay(Order order)
        {
            var entity = db.Orders.Find(order.Id);
            entity.Card = order.Card;
            entity.CardPwd = order.CardPwd;
            entity.PayTime = DateTime.Now;
            entity.Status = "Paid";
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }
        public ActionResult queren(int id)
        {
            Order order = db.Orders.Find(id);
            order.Status = "Received goods";
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }
        public ActionResult Login()
        {
            if (Session["frontuser"] != null)
            {
                Session.Remove("frontuser");
            }
            return View(new User());
        }
        [HttpPost]
        public ActionResult Login([Bind(Include = "Id,UserName,UserPwd")] User users)
        {
            if (db.Users.Any(x => x.UserName == users.UserName && x.UserPwd == users.UserPwd))
            {
                var user = db.Users.First(x => x.UserName == users.UserName && x.UserPwd == users.UserPwd);
                Session["frontuser"] = user;
                return Redirect("/Home/Index");
            }
            else
            {
                ModelState.AddModelError("UserName", "Incorrect username or password");

                return View(users);
            }


        }
        public ActionResult Register()
        {
            return View(new User());
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,UserName,UserPwd,StudentNumber,TrueName,Sex,Birthday,Speciality,College,CreateTime")] User user)
        {
            user.CreateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (db.Users.FirstOrDefault(x => x.UserName == user.UserName) != null)
                {
                    ModelState.AddModelError("UserName", "Account duplication");

                    return View(user);
                }
                db.Users.Add(user);
                db.SaveChanges();
                return Content("<script>alert('Registration successful, please log in');location.href='/Home/Login'</script>");
            }

            return View(user);
        }
        
        public ActionResult ViewInfo()
        {
            if(FrontLoginUserInfo.User==null)
            {
                return Redirect("/Home/Login");
            }
            User user = db.Users.Find(FrontLoginUserInfo.User.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult UpdateInfo()
        {
            if (FrontLoginUserInfo.User == null)
            {
                return Redirect("/Home/Login");
            }
            User user = db.Users.Find(FrontLoginUserInfo.User.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInfo([Bind(Include = "Id,UserName,UserPwd,StudentNumber,TrueName,Sex,Birthday,Speciality,College,CreateTime")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Id != user.Id) != null)
                {
                    ModelState.AddModelError("UserName", "Account duplication");
                    return View(user);
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Content("<script>alert('Edit data successfully！');location.href='/Home/UpdateInfo'</script>");
            }
            return View(user);
        }
         
    }
}