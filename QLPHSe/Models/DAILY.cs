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
    
    public partial class DAILY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DAILY()
        {
            this.PHIEUXUATs = new HashSet<PHIEUXUAT>();
            this.PTTDAILIs = new HashSet<PTTDAILI>();
        }
    
        public int MADL { get; set; }
        public string TENDL { get; set; }
        public string DIACHI { get; set; }
        public Nullable<double> TIENNO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUXUAT> PHIEUXUATs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PTTDAILI> PTTDAILIs { get; set; }
    }
}
