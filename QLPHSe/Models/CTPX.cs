//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLPHSe.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CTPX
    {
        public int CTPX1 { get; set; }
        public Nullable<int> MAPX { get; set; }
        public Nullable<int> MASACH { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public Nullable<double> THANHTIEN { get; set; }
    
        public virtual PHIEUXUAT PHIEUXUAT { get; set; }
        public virtual SACH SACH { get; set; }
    }
}