//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CzyDobre.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetRestaurantCalendarDays
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetRestaurantCalendarDays()
        {
            this.AspNetRestaurantCalendars = new HashSet<AspNetRestaurantCalendars>();
            this.AspNetRestaurantOpenings = new HashSet<AspNetRestaurantOpenings>();
        }
    
        public int Id_RestaurantCalendarDay { get; set; }
        public string RestaurantCalendarDay { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRestaurantCalendars> AspNetRestaurantCalendars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRestaurantOpenings> AspNetRestaurantOpenings { get; set; }
    }
}