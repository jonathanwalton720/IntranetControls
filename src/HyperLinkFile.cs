﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Intranet.Controls
{
    [DefaultProperty("Path")]
    [ToolboxData("<{0}:HyperLinkFile runat=server />")]
    public class HyperLinkFile : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Path
        {
            get
            {
                string s = (string)this.ViewState["Path"];
                return (s == null) ? "[" + this.ID + "]" : s;
            }

            set
            {
                this.ViewState["Path"] = value;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.RenderContents(output);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Href, this.Path);
            output.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write(this.Path);
            output.RenderEndTag();
        }
    }
}