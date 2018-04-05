using System;
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
    public enum BoolEnum
    {
        No,
        Yes
    }

    [DefaultProperty("Path")]
    [ToolboxData("<{0}:HyperLinkDirectory runat=server />")]
    public class HyperLinkDirectory : WebControl
    {
        private string serverPath = string.Empty;
        private BoolEnum writeBreak = BoolEnum.No;

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

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(BoolEnum.No)]
        [Localizable(true)]
        public string WriteBreak
        {
            get
            {
                return this.writeBreak.ToString();
            }

            set
            {
                BoolEnum b;
                if (BoolEnum.TryParse(value, true, out b))
                {
                    this.writeBreak = b;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                this.serverPath = HttpContext.Current.Server.MapPath(this.Path);
            }
            catch
            {
                this.serverPath = string.Empty;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.RenderContents(output);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.serverPath))
                {
                    List<string> filePaths = Directory.EnumerateFiles(this.serverPath).ToList();
                    foreach (string filePath in filePaths)
                    {
                        if (System.IO.Path.GetFileName(filePath).ToUpper() != "THUMBS.DB")
                        {
                            string relativePath = VirtualPathUtility.AppendTrailingSlash(this.Path) + System.IO.Path.GetFileName(filePath);
                            //output.RenderBeginTag(HtmlTextWriterTag.Span);
                            output.AddAttribute(HtmlTextWriterAttribute.Href, relativePath);
                            output.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                            output.RenderBeginTag(HtmlTextWriterTag.A);
                            output.Write(System.IO.Path.GetFileNameWithoutExtension(filePath));
                            output.RenderEndTag();
                            //output.RenderEndTag();
                            if (this.WriteBreak.ToString() == BoolEnum.Yes.ToString())
                            {
                                output.WriteBreak();
                            }

                            output.WriteLine();
                        }
                    }
                }
            }
            catch
            {
                // TODO: need to do something here
            }
        }
    }
}
