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
        private String _Path = String.Empty;
        private String _Title = String.Empty;
        
        /// <summary>
        /// Represents the file's name without the extension.
        /// </summary>
        public String Title
        {
            get { return _Title; }
            private set { _Title = value; }
        }

        /// <summary>
        /// Represents the file's path on the server.
        /// </summary>
        public string Path
        {
            get { return _Path; }
            private set
            {
                if (System.IO.File.Exists(value))
                {
                    _Path = value;
                }
            }
        }

        public FileListing(String path)
        {
            Path = path;
            Title = System.IO.Path.GetFileNameWithoutExtension(this.Path);
        }
    }
}
