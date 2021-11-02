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
    
    public partial class AspNetProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetProduct()
        {
            this.AspNetRatings = new HashSet<AspNetRating>();
            this.AspNetReviews = new HashSet<AspNetReview>();
        }
    
        public int Id_Product { get; set; }
        public Nullable<int> Id_Category { get; set; }
        public string ProductDescription { get; set; }
        public Nullable<int> Id_Ingredients { get; set; }
        public Nullable<int> Id_Localization { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public Nullable<int> Opinion_Counter { get; set; }
    
        public virtual AspNetCategory AspNetCategory { get; set; }
        public virtual AspNetLocalization AspNetLocalization { get; set; }
        public virtual AspNetIngredient AspNetIngredient { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRating> AspNetRatings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetReview> AspNetReviews { get; set; }
    }
}
