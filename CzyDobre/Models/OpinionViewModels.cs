using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class OpinionViewModels
    {
        public string ProductName { get; set; }
        public string RateService { get; set; }
        public string RateTaste { get; set; }
        public string RateComposition { get; set; }
        public string RateIngredients { get; set; }
        public string RateTotal { get; set; }
        public string RateAdcompliance { get; set; }
        public string ImageUrl { get; set; }
    }
}