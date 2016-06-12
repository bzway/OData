using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using OpenData.MyWeb.Models;
using System.Net;
using System.Web.Security;
using OpenData.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenData.Web;

namespace OpenData.MyWeb.Controllers
{
    [BzwayAuthorize]
    public class ManageController : Controller
    {
        IDatabase db
        {
            get
            {
                return DefaultDatabase.GetDatabase();
            }
        }
        public ActionResult Index()
        {
            var list = db.Entity<MyOrderEntity>().Query().ToList();
            var model = new List<MyOrder>();
            foreach (var item in list)
            {
                var m = new MyOrder()
                {
                    BackDate = item.BackDate,
                    BigNumber = item.BigNumber,
                    ChangePrice = item.ChangePrice,
                    ChangeToDate = item.ChangeToDate,
                    ExternalNumber = item.ExternalNumber,
                    FlightNumber = item.FlightNumber,
                    FlightTime = item.FlightTime,
                    FligthLine = item.FligthLine,
                    FullName = item.FullName,
                    GoDate = item.GoDate,
                    Operator = item.Operator,
                    OrderNumber = item.OrderNumber,
                    PassportNumber = item.PassportNumber,
                    Price = item.Price,
                    Remark = item.Remark,
                    Return = item.Return,
                    ReturnNumber = item.ReturnNumber,
                    SmallNumber = item.SmallNumber,
                    Source = item.Source,
                    Status = item.Status,
                    TicketMode = item.TicketMode,
                    TicketNumber = item.TicketNumber,
                    UUID = item.UUID,

                };
                model.Add(m);
            }
            return View(model);
        }

        // GET: MyOrders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyOrderEntity item = db.Entity<MyOrderEntity>().Query().Where(m => m.UUID, id).First();
            if (item == null)
            {
                return HttpNotFound();
            }
            var model = new MyOrder()
            {
                BackDate = item.BackDate,
                BigNumber = item.BigNumber,
                ChangePrice = item.ChangePrice,
                ChangeToDate = item.ChangeToDate,
                ExternalNumber = item.ExternalNumber,
                FlightNumber = item.FlightNumber,
                FlightTime = item.FlightTime,
                FligthLine = item.FligthLine,
                FullName = item.FullName,
                GoDate = item.GoDate,
                Operator = item.Operator,
                OrderNumber = item.OrderNumber,
                PassportNumber = item.PassportNumber,
                Price = item.Price,
                Remark = item.Remark,
                Return = item.Return,
                ReturnNumber = item.ReturnNumber,
                SmallNumber = item.SmallNumber,
                Source = item.Source,
                Status = item.Status,
                TicketMode = item.TicketMode,
                TicketNumber = item.TicketNumber,
                UUID = item.UUID,

            };
            return View(model);
        }

        // GET: MyOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyOrder item)
        {
            if (ModelState.IsValid)
            {
                var entity = new MyOrderEntity()
                {
                    BackDate = item.BackDate,
                    BigNumber = item.BigNumber,
                    ChangePrice = item.ChangePrice,
                    ChangeToDate = item.ChangeToDate,
                    ExternalNumber = item.ExternalNumber,
                    FlightNumber = item.FlightNumber,
                    FlightTime = item.FlightTime,
                    FligthLine = item.FligthLine,
                    FullName = item.FullName,
                    GoDate = item.GoDate,
                    Operator = item.Operator,
                    OrderNumber = item.OrderNumber,
                    PassportNumber = item.PassportNumber,
                    Price = item.Price,
                    Remark = item.Remark,
                    Return = item.Return,
                    ReturnNumber = item.ReturnNumber,
                    SmallNumber = item.SmallNumber,
                    Source = item.Source,
                    Status = item.Status,
                    TicketMode = item.TicketMode,
                    TicketNumber = item.TicketNumber,
                    UUID = item.UUID,
                };
                db.Entity<MyOrderEntity>().Insert(entity);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: MyOrders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyOrderEntity item = db.Entity<MyOrderEntity>().Query().Where(m => m.UUID, id).First();
            if (item == null)
            {
                return HttpNotFound();
            }
            var model = new MyOrder()
            {
                BackDate = item.BackDate,
                BigNumber = item.BigNumber,
                ChangePrice = item.ChangePrice,
                ChangeToDate = item.ChangeToDate,
                ExternalNumber = item.ExternalNumber,
                FlightNumber = item.FlightNumber,
                FlightTime = item.FlightTime,
                FligthLine = item.FligthLine,
                FullName = item.FullName,
                GoDate = item.GoDate,
                Operator = item.Operator,
                OrderNumber = item.OrderNumber,
                PassportNumber = item.PassportNumber,
                Price = item.Price,
                Remark = item.Remark,
                Return = item.Return,
                ReturnNumber = item.ReturnNumber,
                SmallNumber = item.SmallNumber,
                Source = item.Source,
                Status = item.Status,
                TicketMode = item.TicketMode,
                TicketNumber = item.TicketNumber,
                UUID = item.UUID,

            };
            return View(model);
        }

        // POST: MyOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyOrder item)
        {
            if (ModelState.IsValid)
            {
                var entity = new MyOrderEntity()
                {
                    BackDate = item.BackDate,
                    BigNumber = item.BigNumber,
                    ChangePrice = item.ChangePrice,
                    ChangeToDate = item.ChangeToDate,
                    ExternalNumber = item.ExternalNumber,
                    FlightNumber = item.FlightNumber,
                    FlightTime = item.FlightTime,
                    FligthLine = item.FligthLine,
                    FullName = item.FullName,
                    GoDate = item.GoDate,
                    Operator = item.Operator,
                    OrderNumber = item.OrderNumber,
                    PassportNumber = item.PassportNumber,
                    Price = item.Price,
                    Remark = item.Remark,
                    Return = item.Return,
                    ReturnNumber = item.ReturnNumber,
                    SmallNumber = item.SmallNumber,
                    Source = item.Source,
                    Status = item.Status,
                    TicketMode = item.TicketMode,
                    TicketNumber = item.TicketNumber,
                    UUID = item.UUID,
                };


                db.Entity<MyOrderEntity>().Update(entity);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: MyOrders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyOrderEntity item = db.Entity<MyOrderEntity>().Query().Where(m => m.UUID, id).First();

            if (item == null)
            {
                return HttpNotFound();
            }
            var model = new MyOrder()
            {
                BackDate = item.BackDate,
                BigNumber = item.BigNumber,
                ChangePrice = item.ChangePrice,
                ChangeToDate = item.ChangeToDate,
                ExternalNumber = item.ExternalNumber,
                FlightNumber = item.FlightNumber,
                FlightTime = item.FlightTime,
                FligthLine = item.FligthLine,
                FullName = item.FullName,
                GoDate = item.GoDate,
                Operator = item.Operator,
                OrderNumber = item.OrderNumber,
                PassportNumber = item.PassportNumber,
                Price = item.Price,
                Remark = item.Remark,
                Return = item.Return,
                ReturnNumber = item.ReturnNumber,
                SmallNumber = item.SmallNumber,
                Source = item.Source,
                Status = item.Status,
                TicketMode = item.TicketMode,
                TicketNumber = item.TicketNumber,
                UUID = item.UUID,

            };
            return View(model);
        }

        // POST: MyOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MyOrderEntity myOrder = db.Entity<MyOrderEntity>().Query().Where(m => m.UUID, id).First();

            db.Entity<MyOrderEntity>().Delete(myOrder);

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

        public ActionResult Excel()
        {
            var list = db.Entity<MyOrderEntity>().Query().ToList();
            var model = new List<MyOrder>();
            foreach (var item in list)
            {
                var m = new MyOrder()
                {
                    BackDate = item.BackDate,
                    BigNumber = item.BigNumber,
                    ChangePrice = item.ChangePrice,
                    ChangeToDate = item.ChangeToDate,
                    ExternalNumber = item.ExternalNumber,
                    FlightNumber = item.FlightNumber,
                    FlightTime = item.FlightTime,
                    FligthLine = item.FligthLine,
                    FullName = item.FullName,
                    GoDate = item.GoDate,
                    Operator = item.Operator,
                    OrderNumber = item.OrderNumber,
                    PassportNumber = item.PassportNumber,
                    Price = item.Price,
                    Remark = item.Remark,
                    Return = item.Return,
                    ReturnNumber = item.ReturnNumber,
                    SmallNumber = item.SmallNumber,
                    Source = item.Source,
                    Status = item.Status,
                    TicketMode = item.TicketMode,
                    TicketNumber = item.TicketNumber,
                    UUID = item.UUID,

                };
                model.Add(m);
            }
            return this.Excel(model, "证件资料.xls");
        }
    }

    public class BzwayAuthorizeAttribute : AuthorizeAttribute
    {

        public BzwayAuthorizeAttribute()
        {
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Response.Redirect("/Account/Login?returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                return false;
            }
            if (httpContext.Session["UserName"] == null)
            {
                httpContext.Response.Redirect("/Account/Login?message=登录已经失效&returnUrl=" + WebUtility.UrlEncode(httpContext.Request.RawUrl));
                return false;
            }
            return true;
        }
    }
}