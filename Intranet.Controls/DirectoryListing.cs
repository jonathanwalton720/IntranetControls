using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Intranet.Controls
{
    /// <summary>
    /// Represents a directory and a path to locate it.
    /// </summary>
    public class DirectoryListing : IListing
    {
        #region fields
        private int? _FileCount = null;
        private String _Path = String.Empty;
        private String _Title = String.Empty;
        private List<IListing> _Listings = new List<IListing>();
        #endregion fields

        #region properties
        /// <summary>
        /// Represents the name of the directory.
        /// </summary>
        public String Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Represents the directory's path on the server.
        /// </summary>
        public string Path
        {
            get { return _Path; }
            private set
            {
                if (value.Contains('\\'))
                {
                    value = value.Replace('\\', '/');
                }
                if (System.IO.Directory.Exists(value) && !System.IO.Path.HasExtension(value))
                {
                    _Path = value;
                }
            }
        }

        /// <summary>
        /// Represents the number of FileListings found in the directory.
        /// </summary>
        public int FileCount
        {
            get 
            {
                if (!_FileCount.HasValue)
                {
                    _FileCount = 0;
                    foreach (IListing listing in this.GetListings())
                    {
                        if (listing is FileListing) _FileCount++;
                    }
                }
                return _FileCount.Value;
            }
        }
        #endregion properties

        #region constructors
        public DirectoryListing(String path) : this(path, true)
        {

        }

        public DirectoryListing(String path, bool includeSubDirectories)
        {
            this.Path = path;

            if (this.Path != String.Empty)
            {
                this.Title = System.IO.Path.GetFileNameWithoutExtension(this.Path);

                foreach (String filePath in System.IO.Directory.EnumerateFiles(this.Path))
                {
                    FileListing listing = new FileListing(filePath);
                    _Listings.Add(listing);
                    _Listings = _Listings.OrderBy(o => o.Title).ToList();
                }

                if (includeSubDirectories)
                {
                    foreach (String directoryPath in System.IO.Directory.EnumerateDirectories(this.Path))
                    {
                        DirectoryListing listing = new DirectoryListing(directoryPath, false);
                        _Listings.Add(listing);
                    }
                }
            }
        }
        #endregion constructors

        #region methods
        // TODO: Improve refactor _Listings and GetListings()
        public IListing[] GetListings()
        {
            List<IListing> sortedListings = new List<IListing>();
            foreach (IListing listing in _Listings)
            {
                sortedListings.Add(listing);
                if (listing is DirectoryListing)
                {
                    DirectoryListing directory = listing as DirectoryListing;
                    foreach (IListing subListing in directory.GetListings())
                    {
                        if (subListing is FileListing)
                            sortedListings.Add(subListing);
                    }
                }
            }
            return sortedListings.ToArray();
        }

        public void Add(IListing listing)
        {
            _Listings.Add(listing);
        }
        #endregion methods

    }

}
