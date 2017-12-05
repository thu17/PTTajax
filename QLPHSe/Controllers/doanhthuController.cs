using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLPHSe.Models;
namespace QLPHSe.Controllers
{
    public class doanhthuController : Controller
    {
        QuanLiPhatHanhSachv5Entities db = new QuanLiPhatHanhSachv5Entities();
        //
        // GET: /doanhthu/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult thongke(DateTime bd, DateTime kt)
        {
            List<object> item=new List<object>();
            object[] nhap ={
                              new SqlParameter("@NGAYBD",bd),
                              new SqlParameter("@NGAYKT",kt)
                          };
            var tongtienchi = db.Database.SqlQuery<double?>("spdoanhthu_nhap @NGAYBD, @NGAYKT", nhap).SingleOrDefault();
            //Double tongtienchi = Convert.ToDouble(db.Database.ExecuteSqlCommand("spdoanhthu_nhap @NGAYBD, @NGAYKT", nhap));
            item.Add(tongtienchi);
            object[] ban ={
                              new SqlParameter("@NGAYBD",bd),
                              new SqlParameter("@NGAYKT",kt)
                          };
            var tongtienthu = db.Database.SqlQuery<double?>("spdoanhthu_ban @NGAYBD, @NGAYKT", ban).SingleOrDefault();
            //Double tongtienthu = Convert.ToDouble(db.Database.ExecuteSqlCommand("spdoanhthu_ban @NGAYBD, @NGAYKT", ban));
            item.Add(tongtienthu);
            double doanhthu = Convert.ToDouble(tongtienthu) - Convert.ToDouble(tongtienchi);
            item.Add(doanhthu);
            return Json(item, JsonRequestBehavior.AllowGet);

        }
	}
}