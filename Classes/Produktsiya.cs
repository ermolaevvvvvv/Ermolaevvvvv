//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ermolaev.Classes
{
    using System;
    using System.Collections.Generic;
    
    public partial class Produktsiya
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produktsiya()
        {
            this.Postavki = new HashSet<Postavki>();
        }
    
        public int id_produkta { get; set; }
        public string nazvanie_produktsii { get; set; }
        public string edinitsa_izmereniya { get; set; }
        public Nullable<decimal> zakupochnaya_tsena { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Postavki> Postavki { get; set; }
    }
}
