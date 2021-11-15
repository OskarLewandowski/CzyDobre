using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CzyDobre.Models
{
    public class RattingViewModel
    {
        public string ProductNameTop1 { get; set; }
        public Nullable<int> OpinionCounterTop1 { get; set; }
        public Nullable<int> ServiceRateTop1 { get; set; }
        public Nullable<int> TasteRateTop1 { get; set; }
        public Nullable<int> IngredientsRateTop1 { get; set; }
        public string Image1 { get; set; }
        public string ProductNameTop2 { get; set; }
        public Nullable<int> OpinionCounterTop2 { get; set; }
        public Nullable<int> ServiceRateTop2 { get; set; }
        public Nullable<int> TasteRateTop2 { get; set; }
        public Nullable<int> IngredientsRateTop2 { get; set; }
        public string Image2 { get; set; }
        public string ProductNameTop3 { get; set; }
        public Nullable<int> OpinionCounterTop3 { get; set; }
        public Nullable<int> ServiceRateTop3 { get; set; }
        public Nullable<int> TasteRateTop3 { get; set; }
        public Nullable<int> IngredientsRateTop3 { get; set; }
        public string Image3 { get; set; }
        public string ProductNameTop4 { get; set; }
        public Nullable<int> OpinionCounterTop4 { get; set; }
        public Nullable<int> TasteRateTop4 { get; set; }
        public Nullable<int> ServiceRateTop4 { get; set; }
        public Nullable<int> IngredientsRateTop4 { get; set; }
        public string Image4 { get; set; }
    }
 }