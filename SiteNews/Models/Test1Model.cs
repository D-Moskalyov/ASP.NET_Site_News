using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteNews.Models
{
    public class Test1Model
    {
        public int t { get; set; }
        [AllowHtml]
        public string someSrt { get; set; }
        //public NewsModel someNM { get; set; }
    }
}