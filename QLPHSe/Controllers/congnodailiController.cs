using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLPHSe.Models;
using System.Data.SqlClient;
namespace QLPHSe.Controllers
{
    public class congnodailiController : Controller
    {
        QuanLiPhatHanhSachv5Entities db = new QuanLiPhatHanhSachv5Entities();
        //
        // GET: /congnodaili/
        public ActionResult Index()
        {
            List<NoDLModel> hientai = dsnonxb(DateTime.Today);
            return View(hientai);
        }
        public List<NoDLModel> dsnonxb(DateTime? date)
        {
            NoDLModel empty = new NoDLModel();
            empty.SOTIEN = 0;
            object ngayxuat = new SqlParameter("@NGAYNHAP", date);
            object ngaytt = new SqlParameter("@NGAYTT", date);
            List<NoDLModel> mua = db.Database.SqlQuery<NoDLModel>("spdailimua @NGAYNHAP", ngayxuat).ToList();
            List<NoDLModel> tt = db.Database.SqlQuery<NoDLModel>("spdailithanhtoan @NGAYTT", ngaytt).ToList();
            var listno = (from n in mua
                          join dl in db.DAILies
                          on n.MADL equals dl.MADL into newjoin
                          from nj in newjoin
                          join t in tt
                          on nj.MADL equals t.MADL into ifempty
                          from e in ifempty.DefaultIfEmpty(empty)
                          select new NoDLModel()
                          {
                              MADL = n.MADL,
                              TENDL = nj.TENDL,
                              SOTIEN = n.SOTIEN - e.SOTIEN
                          }).ToList();
            return listno;
        }
        [HttpPost]
        public JsonResult thongke(DateTime date)
        {
            List<NoDLModel> hientai = dsnonxb(date);
            return Json(hientai, JsonRequestBehavior.AllowGet);
        }
    }
}