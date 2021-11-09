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
        public string ProductNameTop2 { get; set; }
        public Nullable<int> OpinionCounterTop2 { get; set; }
        public string ProductNameTop3 { get; set; }
        public Nullable<int> OpinionCounterTop3 { get; set; }
        public string ProductNameTop4 { get; set; }
        public Nullable<int> OpinionCounterTop4 { get; set; }
    }
 }