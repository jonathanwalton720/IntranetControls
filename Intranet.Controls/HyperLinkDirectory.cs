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

        private string _ServerPath = String.Empty;
        private BoolEnum _WriteBreak = BoolEnum.No;

        #region properties
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Path
        {
            get
            {
                String s = (String)ViewState["Path"];
                return ((s == null) ? "[" + this.ID + "]" : s);
            }

            set
            {
                ViewState["Path"] = value;
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
                return _WriteBreak.ToString();
            }

            set
            {
                BoolEnum b;
                if (BoolEnum.TryParse(value, true, out b))
                {
                    _WriteBreak = b;
                }
            } 
        }
        #endregion properties

        #region methods
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                _ServerPath = HttpContext.Current.Server.MapPath(this.Path);
            }
            catch
            {
                _ServerPath = String.Empty;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            RenderContents(output);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            try
            {
                if (!String.IsNullOrEmpty(_ServerPath))
                {
                    List<string> filePaths = Directory.EnumerateFiles(_ServerPath).ToList();
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

            }

        } 
        #endregion methods
    }
}
