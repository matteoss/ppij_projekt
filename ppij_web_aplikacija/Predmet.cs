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
    
    public partial class Predmet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Predmet()
        {
            this.osoba_predmet = new HashSet<osoba_predmet>();
        }
    
        public int ID_predmet { get; set; }
        public string naziv_predmet { get; set; }
        public string kratica_predmet { get; set; }
        public Nullable<int> ID_kategorija { get; set; }
        public Nullable<int> ID_ustanova { get; set; }
        public string opis_predmet { get; set; }
    
        public virtual Kategorija Kategorija { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<osoba_predmet> osoba_predmet { get; set; }
        public virtual Ustanova Ustanova { get; set; }
    }
}
