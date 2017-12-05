using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLPHSe.Models
{
    public class SACHDAILIModel
    {
        public int MASACH { get; set; }
        public string TENSACH { get; set; }
        public Double GIABAN { get; set; }
        public int SOLUONG { get; set; }
        public Double THANHTIEN { get { return GIABAN * SOLUONG; } }
    }
}