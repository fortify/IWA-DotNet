using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicroFocus.InsecureWebApp.Models
{
    public class Product : TagHelper
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "On sale")]
        public bool OnSale { get; set; }

        [Display(Name = "Sale price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalePrice { get; set; }

        [Display(Name = "In Stock")]
        public bool InStock { get; set; }

        [Display(Name = "Time to stock")]
        public int TimeToStock { get; set; }

        public int Rating { get; set; }

        public bool Available { get; set; }

        [NotMapped]
        public string HtmlContent { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var sHTML = new TagBuilder("div");
            sHTML.Attributes.Add("class", "col-sm-6 col-lg-4 text-center item mb-4");
            sHTML.InnerHtml.AppendHtml(
                string.Format("({0})", this.Name));

            var title = new TagBuilder("div");
            title.Attributes.Add("class", "text-dark");
            title.InnerHtml.AppendHtml(
                string.Format("{0}", this.Summary));
            title.InnerHtml.AppendHtml(sHTML);

        }
    }
}
