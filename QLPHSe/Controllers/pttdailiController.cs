using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLPHSe.Models;
using System.Data.SqlClient;
using System.Data.Entity;
namespace QLPHSe.Controllers
{
    public class pttdailiController : Controller
    {
        QuanLiPhatHanhSachv5Entities db = new QuanLiPhatHanhSachv5Entities();
        //
        // GET: /pttdaili/
        public ActionResult Index()
        {
            ViewBag.MADL = new SelectList(db.DAILies.ToList(), "MADL", "TENDL");
            DAILITHANHTOANModle model = new DAILITHANHTOANModle();
            model.ct = new List<CTPTTDAILI>();
            model.ct.Add(new CTPTTDAILI());
            return View(model);
        }
        public ActionResult dsdaili()
        {
            using (db)
            {
                var list = db.DAILies.ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }            
        }
        public List<SACHDAILIModel> laysach_tuphieuxuat_daili_damua(int madl)
        {
            SACHDAILIModel model = new SACHDAILIModel();
            model.SOLUONG = 0;
            List<SACHDAILIModel> ds_sach_sachconlai = null;
            object madailimua = new SqlParameter("@MADL", madl);
            object madailitra = new SqlParameter("@MADL", madl);
            List<SACHDAILIModel> ds_sach_daili_damua = db.Database.SqlQuery<SACHDAILIModel>("splaydanhsachdamuacuamotdaili @MADL", madailimua).ToList();
            List<SACHDAILIModel> ds_sach_daili_datra = db.Database.SqlQuery<SACHDAILIModel>("splaydanhsachsachdatracuamotdaili @MADL", madailitra).ToList();
            ds_sach_sachconlai = (from mua in ds_sach_daili_damua
                                  join tra in ds_sach_daili_datra
                                  on mua.MASACH equals tra.MASACH into leftjoin
                                  from l in leftjoin.DefaultIfEmpty(model)
                                  select new SACHDAILIModel()
                                  {
                                      MASACH = mua.MASACH,
                                      TENSACH = mua.TENSACH,
                                      GIABAN = mua.GIABAN,
                                      SOLUONG = mua.SOLUONG - l.SOLUONG//
                                  }).ToList();
            return ds_sach_sachconlai;
        }
        public JsonResult sachmuaboidaili(int madl)
        {
            var list = laysach_tuphieuxuat_daili_damua(madl);
            List<SelectListItem> selectlist = new List<SelectListItem>();
            foreach (var item in list)
            {
                selectlist.Add(new SelectListItem {Text=item.TENSACH,Value=item.MASACH.ToString()});
            }
            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult thongtindaili(int madl)
        {
            var no = (from dl in db.DAILies
                     where dl.MADL == madl
                     select dl.TIENNO).SingleOrDefault();
            return Json(no,JsonRequestBehavior.AllowGet);
        }
        public JsonResult thongtinsach(int madl, int masach)
        {
            SACHDAILIModel sach=new SACHDAILIModel();
            var list = laysach_tuphieuxuat_daili_damua(madl);
            sach=(from l in list
                 where l.MASACH == masach
                 select l).SingleOrDefault();
            return Json(sach, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult themsach(CTPTTDAILI ct,int max)
        {
            if(ct.SOLUONG>max)
                return Json(-1);
            var sach=db.SACHes.Find(ct.MASACH);
            DAILITHANHTOANModle ptt = new DAILITHANHTOANModle();
            if(Session["ptt"]!=null)
            {
                ptt = (DAILITHANHTOANModle)Session["ptt"];
            }
            else
            {
                ptt.ct = new List<CTPTTDAILI>();
            }
            var sachdaco = (from c in ptt.ct
                       where c.MASACH == ct.MASACH
                       select c).SingleOrDefault();
            if(sachdaco!=null)
            {
                    
                    CTPTTDAILI capnhatsachdaco = new CTPTTDAILI();
                    capnhatsachdaco.MASACH = ct.MASACH;
                    int? soluong = sachdaco.SOLUONG + ct.SOLUONG;
                    if (soluong <= max)
                    {
                        ptt.ct.Remove(sachdaco);
                        capnhatsachdaco.SOLUONG = sachdaco.SOLUONG + ct.SOLUONG;
                        capnhatsachdaco.THANHTIEN = capnhatsachdaco.SOLUONG * sach.GIABAN;
                        ptt.ct.Add(capnhatsachdaco);
                    }
                    else return Json(-2);
            }else
                {
                    ct.THANHTIEN = ct.SOLUONG * sach.GIABAN;
                    ptt.ct.Add(ct);
                }
            Session["ptt"] = ptt;
            var join = from p in ptt.ct
                       join s in db.SACHes
                       on p.MASACH equals s.MASACH into info
                       from i in info
                       select new { i.MASACH, i.TENSACH,p.SOLUONG, i.GIABAN, p.THANHTIEN};
            return Json(join.ToList(),JsonRequestBehavior.AllowGet);//
        }
        //[HttpPost]
        //public ActionResult luuphieu(DAILITHANHTOANModle tt)
        //{
        //    DAILITHANHTOANModle temp = new DAILITHANHTOANModle();
        //    CTPTTDAILI ct = new CTPTTDAILI();
        //    if(Session["ptt"]!=null)
        //    {
        //        temp = (DAILITHANHTOANModle)Session["ptt"];
        //        Session["ptt"] = null;
        //        tt.ct = temp.ct;
        //    }
        //    if(ModelState.IsValid)
        //    {
        //        tt.ptt.TONGTIEN = 0;
        //        foreach (CTPTTDAILI item in tt.ct)
        //        {
        //            tt.ptt.TONGTIEN += item.THANHTIEN;
        //        }
        //        PTTDAILI p = new PTTDAILI
        //        {
        //            MADL=tt.ptt.MADL,
        //            NGAYTT=tt.ptt.NGAYTT,
        //            TONGTIEN=tt.ptt.TONGTIEN
        //        };
        //        db.PTTDAILIs.Add(p);
        //        db.SaveChanges();
        //        int id=p.MPTTDAILI;
        //        foreach (CTPTTDAILI item in tt.ct)
        //        {
        //            item.MPTTDAILI = id;
        //            db.CTPTTDAILIs.Add(item);
        //        }
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(tt);
        //}
        //public ActionResult dsctphieutrongsession()
        //{
        //    DAILITHANHTOANModle ss = new DAILITHANHTOANModle();
        //    if (Session["ptt"] != null)
        //    {
        //        ss = (DAILITHANHTOANModle)Session["ptt"];
        //    }
        //    return Json(ss.ct.ToList(), JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult xoasach(int id)
        {
            DAILITHANHTOANModle ptt = new DAILITHANHTOANModle();
            ptt = (DAILITHANHTOANModle)Session["ptt"];
            var s=(from ct in ptt.ct
                   where ct.MASACH==id
                   select ct).SingleOrDefault();
            ptt.ct.Remove(s);
            Session["ptt"] = ptt;
            var join = from ct in ptt.ct
                       join sach in db.SACHes
                       on ct.MASACH equals sach.MASACH into info
                       from i in info
                       select new { i.MASACH, i.TENSACH, ct.SOLUONG, i.GIABAN, ct.THANHTIEN };
            return Json(join.ToList(),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult luuphieu(int MADL,DateTime NGAYTT,double TONGTIEN)
        {
            //int nochophep = (from hs in db.HANGSOes
            //                   select hs.notoida).SingleOrDefault();
            //double? nodaili = (from dl in db.DAILies
            //                  where dl.MADL == MADL
            //                  select dl.TIENNO).SingleOrDefault();
            //if(nodaili+TONGTIEN>nochophep)
            //{
            //    return Json(-1);
            //}
                DAILITHANHTOANModle tt = new DAILITHANHTOANModle();
                DAILITHANHTOANModle temp = new DAILITHANHTOANModle();
                CTPTTDAILI ct = new CTPTTDAILI();
                if (Session["ptt"] != null)
                {
                    temp = (DAILITHANHTOANModle)Session["ptt"];
                    Session["ptt"] = null;
                    tt.ct = temp.ct;
                }
                if (ModelState.IsValid)
                {
                    PTTDAILI p = new PTTDAILI
                    {
                        MADL = MADL,
                        NGAYTT = NGAYTT,
                        TONGTIEN = TONGTIEN
                    };
                    db.PTTDAILIs.Add(p);
                    db.SaveChanges();                    
                    int id = p.MPTTDAILI;
                    foreach (var item in tt.ct)
                    {
                        item.MPTTDAILI = id;
                        db.CTPTTDAILIs.Add(item);
                    }
                    db.SaveChanges();
                    var daili = db.DAILies.Find(MADL);
                    daili.TIENNO=daili.TIENNO-TONGTIEN;
                    db.Entry(daili).State = EntityState.Modified;
                    db.SaveChanges();
                }
            return RedirectToAction("Index");
            //return View(tt);
        }
    }
}