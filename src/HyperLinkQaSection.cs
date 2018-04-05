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
    [ToolboxData("<{0}:HyperLinkQaSection runat=server></{0}:HyperLinkQaSection>")]
    public class HyperLinkQaSection : HyperLinkSection
    {
        protected override void GenerateFormattedHeading(HtmlTextWriter output, IListing listing)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Strong);

            if (listing.Title.Contains(this.Delimiter) && this.Delimiter != string.Empty)
            {
                output.Write(listing.Title.Split(this.Delimiter.ToCharArray())[1]);
            }
            else
            {
                output.Write(listing.Title);
            }

            output.RenderEndTag();
        }
    }
}
