using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class MuonTraSachController : Controller
    {
        private CNCFContext db = new CNCFContext();

        // GET: Muon_Sach
        public ActionResult Index()
        {
            var muon_Sach = db.MuonTraSach.Include(m => m.HocSinh).Include(m => m.Sach);

            return View(muon_Sach.ToList());
        }

        // GET: Muon_Sach/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MuonTraSach muon_Sach = db.MuonTraSach.Find(id);
            if (muon_Sach == null)
            {
                return HttpNotFound();
            }
            return View(muon_Sach);
        }

        // GET: Muon_Sach/Create
        [HttpPost]
        [Authorize]
        public ActionResult TimHocSinh_Sach(string tenHS, string tenSach, string confirmed)
        {
            ViewBag.tenHS = tenHS;
            ViewBag.tenSach = tenSach;
            ViewBag.confirmed = confirmed;
            return RedirectToAction("Create", "Muon_Sach", new { tenHS = ViewBag.tenHS, tenSach = ViewBag.tenSach, confirmed = ViewBag.confirmed });
        }

        [Authorize]
        public ActionResult Create(string tenHS, string tenSach, string confirmed, string sortOrder)
        {
            string confirmed1;
            string temp_tenHS = tenHS;
            string temp_tenSach = tenSach;
            var hoc_sinh = from h in db.HocSinh
                           select h;

            var sach = from s in db.Sach
                       select s;

            var muon_Sach = db.MuonTraSach.Include(m => m.HocSinh).Include(m => m.Sach);

            if (!string.IsNullOrEmpty(tenHS))
            {
                hoc_sinh = hoc_sinh.Where(h => h.TenHS.Contains(temp_tenHS));
                muon_Sach = muon_Sach.Where(h => h.HocSinh.TenHS.Contains(temp_tenHS)); ;

            }

            if (!string.IsNullOrEmpty(tenSach))
            {
                sach = sach.Where(s => s.TenSach.Contains(temp_tenSach));
            }


            if (confirmed == null)
            {
                confirmed1 = "no";
            }
            else
            {
                confirmed1 = confirmed;
            }


            ViewBag.Hoc_SinhID = new SelectList(hoc_sinh, "ID", "TenHS");
            ViewBag.SachID = new SelectList(sach, "ID", "TenSach");
            ViewBag.confirm = confirmed1;
            ViewBag.view_muon_sach = muon_Sach;

            return View();
        }

        // POST: Muon_Sach/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SachID,Hoc_SinhID,NgayMuon,HanTra,NgayTra,TrangThai")] MuonTraSach muon_Sach, string tenHS, string tenSach)
        {
            string temp_tenHS = tenHS;
            string temp_tenSach = tenSach;
            var hoc_sinh = from h in db.HocSinh
                           select h;

            var sach = from s in db.Sach
                       select s;

            if (!string.IsNullOrEmpty(tenHS))
            {
                hoc_sinh = hoc_sinh.Where(h => h.TenHS.Contains(temp_tenHS));
            }

            if (!string.IsNullOrEmpty(tenSach))
            {
                sach = sach.Where(s => s.TenSach.Contains(temp_tenSach));
            }

            if (ModelState.IsValid)
            {
                db.MuonTraSach.Add(muon_Sach);
                db.SaveChanges();
                return RedirectToAction("Create", new { tenHS = temp_tenHS, confirmed = "yes" });
            }

            ViewBag.Hoc_SinhID = new SelectList(hoc_sinh, "ID", "TenHS", muon_Sach.HocSinhID);
            ViewBag.SachID = new SelectList(sach, "ID", "TenSach", muon_Sach.SachID);
            return View(muon_Sach);
        }


        // GET: Muon_Sach/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MuonTraSach muon_Sach = db.MuonTraSach.Find(id);
            ViewBag.tim_HS = muon_Sach.HocSinh.TenHS;
            var hoc_sinh = from h in db.HocSinh
                           where h.ID == muon_Sach.HocSinhID
                           select h;

            var sach = from s in db.Sach
                       where s.ID == muon_Sach.SachID
                       select s;

            if (muon_Sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hoc_SinhID = new SelectList(hoc_sinh, "ID", "TenHS", muon_Sach.HocSinhID);
            ViewBag.SachID = new SelectList(sach, "ID", "TenSach", muon_Sach.SachID);
            return View(muon_Sach);
        }

        // POST: Muon_Sach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SachID,Hoc_SinhID,NgayMuon,HanTra,NgayTra,TrangThai")] MuonTraSach muon_Sach, string HS_TraSach)
        {

            if (ModelState.IsValid)
            {
                db.Entry(muon_Sach).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Create");
                //var hoc_sinh = from h in db.Hoc_Sinh
                //               where h.ID == muon_Sach.Hoc_SinhID
                //               select h;
                //string tenhs = hoc_sinh.Select(h => h.TenHS).ToString() ;
                return RedirectToAction("Create", "Muon_Sach", new { tenHS = HS_TraSach, confirmed = "yes" });
            }
            ViewBag.Hoc_SinhID = new SelectList(db.HocSinh, "ID", "TenHS", muon_Sach.HocSinhID);
            ViewBag.SachID = new SelectList(db.Sach, "ID", "TenSach", muon_Sach.SachID);
            return View(muon_Sach);

        }

        // GET: Muon_Sach/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MuonTraSach muon_Sach = db.MuonTraSach.Find(id);
            if (muon_Sach == null)
            {
                return HttpNotFound();
            }
            return View(muon_Sach);
        }

        // POST: Muon_Sach/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MuonTraSach muon_Sach = db.MuonTraSach.Find(id);
            db.MuonTraSach.Remove(muon_Sach);
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
