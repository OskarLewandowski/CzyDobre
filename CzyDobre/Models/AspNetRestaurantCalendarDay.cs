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
    
    public partial class AspNetRestaurantCalendarDay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetRestaurantCalendarDay()
        {
            this.AspNetRestaurantCalendars = new HashSet<AspNetRestaurantCalendar>();
            this.AspNetRestaurantOpenings = new HashSet<AspNetRestaurantOpening>();
        }
    
        public int Id_RestaurantCalendarDay { get; set; }
        public string RestaurantCalendarDay { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRestaurantCalendar> AspNetRestaurantCalendars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRestaurantOpening> AspNetRestaurantOpenings { get; set; }
    }
}
