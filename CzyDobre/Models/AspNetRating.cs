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
    
    public partial class AspNetRating
    {
        public int Id_Rating { get; set; }
        public short RateService { get; set; }
        public short RateTaste { get; set; }
        public short RateIngredients { get; set; }
        public short RateComposition { get; set; }
        public Nullable<int> Id_Review { get; set; }
        public bool RateAdcompliance { get; set; }
        public Nullable<double> RateTotal { get; set; }
        public Nullable<int> Id_Product { get; set; }
    
        public virtual AspNetProduct AspNetProduct { get; set; }
        public virtual AspNetReview AspNetReview { get; set; }
    }
}
