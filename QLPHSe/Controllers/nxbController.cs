using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLPHSe.Models;

namespace QLPHSe.Controllers
{
    public class nxbController : Controller
    {
        private QuanLiPhatHanhSachv5Entities db = new QuanLiPhatHanhSachv5Entities();

        // GET: /nxb/
        public ActionResult Index()
        {
            return View(db.NXBs.ToList());
        }

        // GET: /nxb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NXB nxb = db.NXBs.Find(id);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        // GET: /nxb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /nxb/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MANXB,TENNXB,DIACHI,SDT,STK")] NXB nxb)
        {
            if (ModelState.IsValid)
            {
                db.NXBs.Add(nxb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nxb);
        }

        // GET: /nxb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NXB nxb = db.NXBs.Find(id);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        // POST: /nxb/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MANXB,TENNXB,DIACHI,SDT,STK")] NXB nxb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nxb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nxb);
        }

        // GET: /nxb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NXB nxb = db.NXBs.Find(id);
            if (nxb == null)
            {
                return HttpNotFound();
            }
            return View(nxb);
        }

        // POST: /nxb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NXB nxb = db.NXBs.Find(id);
            db.NXBs.Remove(nxb);
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
