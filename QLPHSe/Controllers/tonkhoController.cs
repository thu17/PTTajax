using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLPHSe.Models;
namespace QLPHSe.Controllers
{
    public class tonkhoController : Controller
    {
        //
        // GET: /tonkho/
        QuanLiPhatHanhSachv5Entities db = new QuanLiPhatHanhSachv5Entities();
        public ActionResult Index()
        {
            List<TONKHO> tonkhodenhientai = tonkho(DateTime.Today);
            var newlist = (from l in tonkhodenhientai
                           join s in db.SACHes
                           on l.MASACH equals s.MASACH
                           select new SachTonkhoModel()
                           {
                               MASACH = l.MASACH,
                               TENSACH = s.TENSACH,
                               SOLUONG = l.SOLUONG
                           }).ToList();
            return View(newlist);
        }
        public List<TONKHO> tonkho(DateTime? date)
        {
            List<TONKHO> all = db.TONKHOes.ToList();
            //List<int?> masach = db.Database.SqlQuery<int?>("splaymasachtrongtokho").ToList();
            List<int?> masach = (from a in all
                                 select a.MASACH).Distinct().OrderBy(a => a).ToList();
            List<TONKHO> thongke = new List<TONKHO>();
            foreach (var item in masach)
            {
                List<TONKHO> tonkhotheomasach = all.Where(m => m.MASACH.Equals(item)).ToList();
                List<TONKHO> sachtonkhotheongay = tonkhotheomasach.Where(t => t.NGAYCAPNHAT.Equals(date)).ToList();
                int rowcout = sachtonkhotheongay.Count;
                if(rowcout>1)
                       {
                           thongke.Add(sachtonkhotheongay.ElementAt(sachtonkhotheongay.Count - 1));
                       }
                else if (rowcout==1)
                        {
                            TONKHO tk = new TONKHO();
                            tk = sachtonkhotheongay.SingleOrDefault();
                            thongke.Add(tk);
                        }                
                else if(rowcout==0)
                        {
                            int count = tonkhotheomasach.Count;
                            if ((tonkhotheomasach.ElementAt(0).NGAYCAPNHAT) > date)
                            {
                                continue;
                            }
                            else if (date > tonkhotheomasach.ElementAt(count - 1).NGAYCAPNHAT)
                            {
                                thongke.Add(tonkhotheomasach.ElementAt(count - 1));
                            }

                            else
                            {
                                for (int i = count - 1; i >= 1; i--)
                                {
                                    if (tonkhotheomasach.ElementAt(i).NGAYCAPNHAT > date && tonkhotheomasach.ElementAt(i - 1).NGAYCAPNHAT < date)

                                        thongke.Add(tonkhotheomasach.ElementAt(i - 1));

                                }
                            }
                    
                }

            }
            return thongke;
        }
        [HttpPost]
        public JsonResult thongke(DateTime? date)
        {
            List<TONKHO> list = tonkho(date);
            var newlist = (from l in list
                           join s in db.SACHes
                           on l.MASACH equals s.MASACH
                           select new SachTonkhoModel()
                           {
                               MASACH = l.MASACH,
                               TENSACH = s.TENSACH,
                               SOLUONG = l.SOLUONG
                           }).ToList();
            return Json(newlist,JsonRequestBehavior.AllowGet);
        }
	}
}