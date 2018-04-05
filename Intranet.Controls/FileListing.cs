using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Intranet.Controls
{
    /// <summary>
    /// Represents a file and a path to retrieve it.
    /// </summary>
    public class FileListing : IListing
    {
        private string path = string.Empty;
        private string title = string.Empty;

        public FileListing(string path)
        {
            this.Path = path;
            this.Title = System.IO.Path.GetFileNameWithoutExtension(this.Path);
        }

        /// <summary>
        /// Represents the file's name without the extension.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            private set { this.title = value; }
        }

        /// <summary>
        /// Represents the file's path on the server.
        /// </summary>
        public string Path
        {
            get { return this.path; }

            private set
            {
                if (System.IO.File.Exists(value))
                {
                    this.path = value;
                }
            }
        }
    }
}
