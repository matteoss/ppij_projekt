//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ppij_web_aplikacija
{
    using System;
    using System.Collections.Generic;
    
    public partial class osoba_predmet
    {
        public int ID_osoba { get; set; }
        public int ID_predmet { get; set; }
        public Nullable<decimal> cijena { get; set; }
    
        public virtual Osoba Osoba { get; set; }
        public virtual Predmet Predmet { get; set; }
    }
}
