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
    
    public partial class dogovor_termin
    {
        public int ID_dogovor_termin { get; set; }
        public Nullable<int> dogovor_status { get; set; }
        public Nullable<int> dogovor_ocijena { get; set; }
        public Nullable<int> ID_instruktor { get; set; }
        public Nullable<int> ID_klijent { get; set; }
        public Nullable<System.DateTime> datum_dogovor { get; set; }
        public Nullable<int> ID_predmet { get; set; }
        public Nullable<int> trajanje { get; set; }
        public Nullable<int> ID_lokacija { get; set; }
    
        public virtual Osoba Osoba { get; set; }
        public virtual Osoba Osoba1 { get; set; }
        public virtual Lokacija Lokacija { get; set; }
    }
}
