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
            this.AspNetImages = new HashSet<AspNetImage>();
            this.AspNetRatings = new HashSet<AspNetRating>();
        }
    
        public int Id_Product { get; set; }
        public int Id_Category { get; set; }
        public string ProductDescription { get; set; }
        public string ProductName { get; set; }
        public int Opinion_Counter { get; set; }
        public Nullable<int> AvarageTaste { get; set; }
        public Nullable<int> AvarageService { get; set; }
        public Nullable<int> AvarageIngredients { get; set; }
        public string Who { get; set; }
        public string UniqName { get; set; }
        public Nullable<int> Id_Place { get; set; }
    
        public virtual AspNetCategory AspNetCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetImage> AspNetImages { get; set; }
        public virtual AspNetPlace AspNetPlace { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRating> AspNetRatings { get; set; }
    }
}
