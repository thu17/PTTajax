using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLPHSe.Models;
using System.Data.SqlClient;
namespace QLPHSe.Controllers
{
    public class nonxbController : Controller
    {
        //
        // GET: /nonxb/
        QuanLiPhatHanhSachv5Entities db = new QuanLiPhatHanhSachv5Entities();
        public ActionResult Index()
        {
            List<NoNXBModel> hientai=dsnonxb(DateTime.Today);
            return View(hientai);
        }
        public List<NoNXBModel> dsnonxb(DateTime? date) 
        {
            NoNXBModel empty = new NoNXBModel();
            empty.SOTIEN = 0;
            object ngaynhap = new SqlParameter("@NGAYNHAP", date);
            object ngaytt = new SqlParameter("@NGAYTT", date);
            List<NoNXBModel> nhap = db.Database.SqlQuery<NoNXBModel>("sptenmua @NGAYNHAP",ngaynhap).ToList();
            List<NoNXBModel> tra = db.Database.SqlQuery<NoNXBModel>("sptentra @NGAYTT", ngaytt).ToList();
            var listno = (from n in nhap
                          join nxb in db.NXBs
                          on n.MANXB equals nxb.MANXB into newjoin
                          from nj in newjoin
                          join t in tra
                          on nj.MANXB equals t.MANXB into ifempty
                          from e in ifempty.DefaultIfEmpty(empty)
                          select new NoNXBModel()
                          {
                              MANXB = n.MANXB,
                              TENNXB = nj.TENNXB,
                              SOTIEN = n.SOTIEN - e.SOTIEN
                          }).ToList();
            return listno;
        }
        [HttpPost]
        public JsonResult thongke(DateTime date)
        {
            List<NoNXBModel> hientai = dsnonxb(date);
            return Json(hientai, JsonRequestBehavior.AllowGet);
        }
	}
}