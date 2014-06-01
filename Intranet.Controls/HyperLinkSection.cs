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

    [DefaultProperty("Path")]
    [ToolboxData("<{0}:HyperLinkSection runat=server />")]
    public class HyperLinkSection : WebControl
    {
        private DirectoryListing _directoryListing = null;
        private string _path = String.Empty;
        private BoolEnum _writeBreak = BoolEnum.No;
        private string _serverPath = String.Empty;
        private string _directoryTitle = String.Empty;
        protected string _relativePath = String.Empty;
        private string _delimiter = String.Empty;

        #region properties
        /// <summary>
        /// Css Class for the Directory Heading
        /// </summary>
        public String HeadingClass { get; set; }

        /// <summary>
        /// Css Class for the File Listing
        /// </summary>
        public String ListingClass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Delimiter
        {
            get
            {
                return _delimiter;
            }
            set
            {
                _delimiter = value;
            }
        }

        public String Path
        {
            get { return _path; }
            set
            {
                if (value.Contains('\\'))
                {
                    value = value.Replace('\\', '/');
                }
                _path = value;
                value = VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Server.MapPath(value));
                if (System.IO.Directory.Exists(value) && !System.IO.Path.HasExtension(value))
                {
                    _serverPath = value;
                }
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
                return _writeBreak.ToString();
            }

            set
            {
                BoolEnum b;
                if (BoolEnum.TryParse(value, true, out b))
                {
                    _writeBreak = b;
                }
            }
        }
        
        #endregion properties
        protected override void Render(HtmlTextWriter output)
        {
            RenderContents(output);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            _directoryListing = new DirectoryListing(_serverPath);
            foreach (IListing listing in _directoryListing.GetListings())
            {
                if (System.IO.Path.GetFileName(listing.Title).ToUpper() != "THUMBS")
                {
                    if (listing is DirectoryListing)
                    {
                        DirectoryListing dl = listing as DirectoryListing;
                        if (dl.FileCount > 0 )
                        {
                            if (_directoryTitle != String.Empty && this.WriteBreak == BoolEnum.Yes.ToString())
                                output.WriteBreak();
                            _directoryTitle = listing.Title;
                            if (!String.IsNullOrEmpty(this.HeadingClass))
                            {
                                output.AddAttribute(HtmlTextWriterAttribute.Class, this.HeadingClass);
                            }
                            GenerateFormattedHeading(output, listing);
                            if (this.WriteBreak == BoolEnum.Yes.ToString())
                            {
                                output.WriteBreak();
                            }
                            output.WriteLine();
                        }
                    }
                    else
                    {
                        if (_directoryTitle != String.Empty)
                            _relativePath = VirtualPathUtility.AppendTrailingSlash(this.Path) + VirtualPathUtility.AppendTrailingSlash(_directoryTitle) + System.IO.Path.GetFileName(listing.Path);
                        else
                            _relativePath = VirtualPathUtility.AppendTrailingSlash(this.Path) + System.IO.Path.GetFileName(listing.Path);
                        if (!String.IsNullOrEmpty(this.ListingClass))
                        {
                            output.AddAttribute(HtmlTextWriterAttribute.Class, this.ListingClass);
                        }
                        GenerateFormattedListing(output, listing);
                        if (this.WriteBreak.ToString() == BoolEnum.Yes.ToString())
                        {
                            output.WriteBreak();
                        }
                        output.WriteLine();
                    }
                }
            }
        }

        protected virtual void GenerateFormattedHeading(HtmlTextWriter output, IListing listing)
        {

            output.RenderBeginTag(HtmlTextWriterTag.Strong);
            output.Write(listing.Title);
            output.RenderEndTag();
        }

        protected virtual void GenerateFormattedListing(HtmlTextWriter output, IListing listing)
        {

            output.AddAttribute(HtmlTextWriterAttribute.Href, _relativePath);
            output.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write(listing.Title);
            output.RenderEndTag();
        }


    }
}
