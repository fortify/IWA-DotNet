

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MicroFocus.InsecureWebApp.Models
{
    public static class MyHTMLHelpers
    {
        public static IHtmlContent DivInjectionHTMLString(this IHtmlHelper htmlHelper, string ProductDesc)
        {
            string htmlOutput = string.Empty;
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("xsinject");
            tagBuilder.InnerHtml.AppendHtml("<a href = 'javascript:swal(\" "+ ProductDesc +" \")' > " + ProductDesc.Substring(0,20) + " </a>");
            
            using (var writer = new StringWriter())
            {
                tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                htmlOutput = writer.ToString();
            }


            return new HtmlString(htmlOutput);
        }
        //=> new HtmlString("<div class=\"xsinject\"> <a href='javascript:alert(\" "+ ProductDesc +" \")'>" + ProductDesc.Substring(0,20) + "</a></div>");
    }

    public class DivTagHelper : TagHelper
    {
        const string appreciationText = "Great work";
        public bool IsAdded
        {
            get;
            set;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            if (IsAdded) {
                output.TagName = "Div";
                string message = appreciationText;
                var attribute = new TagHelperAttribute(name: "span", value: message);
                output.Attributes.Add(attribute);
                output.Content.SetHtmlContent(message);
            }
        }
    }

    //[HtmlTargetElement("label", Attributes = "asp-for")]
    //public class InsecureHtmlHelpers : LabelTagHelper
    //{
    //    public InsecureHtmlHelpers(IHtmlGenerator htmlGenerator)
    //        : base(htmlGenerator)
    //    {
    //    }


    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        int carouselWidth = 300;

    //        TagBuilder div = new TagBuilder("div");

    //        div.MergeAttribute(
    //            "style",
    //            $"width: { carouselWidth }px; height: { carouselWidth }px; background-color: green;");

    //        output.Content.SetHtmlContent(div);
    //    }
    //    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    //    {
    //        await base.ProcessAsync(context, output);

    //        var metadata = For.Metadata as DefaultModelMetadata;
    //        bool hasRequiredAttribute = metadata
    //                                    ?.Attributes
    //                                    .PropertyAttributes
    //                                    .Any(i => i.GetType() == typeof(RequiredAttribute)) ?? false;
    //        if (hasRequiredAttribute)
    //        {
    //            output.PostContent.AppendHtml("(required)");
    //        }
    //    }
    //}

    //public static class InsecureHtmlHelpers
    //{
    //    public static MvcHtmlString FormatStringHtml(this HtmlHelper html, string sText)
    //    {


    //        var tagBuilder = new TagBuilder("div");
    //        tagBuilder.InnerHtml = sText;
    //        return MvcHtmlString.Create(tagBuilder.ToString());
    //    }
    //}
}
