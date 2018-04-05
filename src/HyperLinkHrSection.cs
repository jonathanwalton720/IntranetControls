using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Intranet.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:HyperLinkHrSection runat=server></{0}:HyperLinkHrSection>")]
    public class HyperLinkHrSection : HyperLinkSection
    {
        protected override void GenerateFormattedListing(HtmlTextWriter output, IListing listing)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Href, this.relativePath);
            output.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
            output.RenderBeginTag(HtmlTextWriterTag.A);

            if (listing.Title.Split(this.Delimiter.ToCharArray()).Count() == 2 && this.Delimiter != string.Empty)
            {
                output.Write(listing.Title.Split(this.Delimiter.ToCharArray())[0]);
            }
            else
            {
                output.Write(listing.Title);
            }

            output.RenderEndTag();
            output.AddAttribute(HtmlTextWriterAttribute.Class, "alignRight");
            output.RenderBeginTag(HtmlTextWriterTag.Span);

            if (listing.Title.Split(this.Delimiter.ToCharArray()).Count() == 2 && this.Delimiter != string.Empty)
            {
                output.Write(listing.Title.Split(this.Delimiter.ToCharArray())[1]);
            }
            else
            {
                output.Write(string.Empty);
            }

            output.RenderEndTag();
        }
    }
}
