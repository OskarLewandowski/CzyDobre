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
    
    public partial class AspNetLocalization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetLocalization()
        {
            this.AspNetRestaurants = new HashSet<AspNetRestaurant>();
        }
    
        public int Id_Localization { get; set; }
        public string LocalizationAdress { get; set; }
        public string LocalizationCity { get; set; }
        public int Id_RestaurantType { get; set; }
    
        public virtual AspNetRestaurantsType AspNetRestaurantsType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRestaurant> AspNetRestaurants { get; set; }
    }
}
