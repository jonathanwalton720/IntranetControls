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
        protected string relativePath = string.Empty;
        private DirectoryListing directoryListing = null;
        private string path = string.Empty;
        private BoolEnum writeBreak = BoolEnum.No;
        private string serverPath = string.Empty;
        private string directoryTitle = string.Empty;
        private string delimiter = string.Empty;

        /// <summary>
        /// Gets or sets Css Class for the Directory Heading
        /// </summary>
        public string HeadingClass { get; set; }

        /// <summary>
        /// Gets or sets Css Class for the File Listing
        /// </summary>
        public string ListingClass { get; set; }

        /// <summary>
        /// Gets or sets the delimeter.
        /// </summary>
        public string Delimiter
        {
            get
            {
                return this.delimiter;
            }

            set
            {
                this.delimiter = value;
            }
        }

        public string Path
        {
            get { return this.path; }

            set
            {
                if (value.Contains('\\'))
                {
                    value = value.Replace('\\', '/');
                }

                this.path = value;
                value = VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Server.MapPath(value));
                if (System.IO.Directory.Exists(value) && !System.IO.Path.HasExtension(value))
                {
                    this.serverPath = value;
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

        protected override void Render(HtmlTextWriter output)
        {
            this.RenderContents(output);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            this.directoryListing = new DirectoryListing(this.serverPath);
            foreach (IListing listing in this.directoryListing.GetListings())
            {
                if (System.IO.Path.GetFileName(listing.Title).ToUpper() != "THUMBS")
                {
                    if (listing is DirectoryListing)
                    {
                        DirectoryListing dl = listing as DirectoryListing;
                        if (dl.FileCount > 0)
                        {
                            if (this.directoryTitle != string.Empty && this.WriteBreak == BoolEnum.Yes.ToString())
                            {
                                output.WriteBreak();
                            }

                            this.directoryTitle = listing.Title;
                            if (!string.IsNullOrEmpty(this.HeadingClass))
                            {
                                output.AddAttribute(HtmlTextWriterAttribute.Class, this.HeadingClass);
                            }

                            this.GenerateFormattedHeading(output, listing);
                            if (this.WriteBreak == BoolEnum.Yes.ToString())
                            {
                                output.WriteBreak();
                            }

                            output.WriteLine();
                        }
                    }
                    else
                    {
                        if (this.directoryTitle != string.Empty)
                        {
                            this.relativePath = VirtualPathUtility.AppendTrailingSlash(this.Path) + VirtualPathUtility.AppendTrailingSlash(this.directoryTitle) + System.IO.Path.GetFileName(listing.Path);
                        }
                        else
                        {
                            this.relativePath = VirtualPathUtility.AppendTrailingSlash(this.Path) + System.IO.Path.GetFileName(listing.Path);
                        }

                        if (!string.IsNullOrEmpty(this.ListingClass))
                        {
                            output.AddAttribute(HtmlTextWriterAttribute.Class, this.ListingClass);
                        }

                        this.GenerateFormattedListing(output, listing);
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
            output.AddAttribute(HtmlTextWriterAttribute.Href, this.relativePath);
            output.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write(listing.Title);
            output.RenderEndTag();
        }
    }
}
