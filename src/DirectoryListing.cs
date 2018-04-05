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
        private int? fileCount = null;
        private string path = string.Empty;
        private string title = string.Empty;
        private List<IListing> listings = new List<IListing>();

        public DirectoryListing(string path)
            : this(path, true)
        {
        }

        public DirectoryListing(string path, bool includeSubDirectories)
        {
            this.Path = path;

            if (this.Path != string.Empty)
            {
                this.Title = System.IO.Path.GetFileNameWithoutExtension(this.Path);

                foreach (string filePath in System.IO.Directory.EnumerateFiles(this.Path))
                {
                    FileListing listing = new FileListing(filePath);
                    this.listings.Add(listing);
                    this.listings = this.listings.OrderBy(o => o.Title).ToList();
                }

                if (includeSubDirectories)
                {
                    foreach (string directoryPath in System.IO.Directory.EnumerateDirectories(this.Path))
                    {
                        DirectoryListing listing = new DirectoryListing(directoryPath, false);
                        this.listings.Add(listing);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the title of the directory.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Gets the directory's path on the server.
        /// </summary>
        public string Path
        {
            get { return this.path; }

            private set
            {
                if (value.Contains('\\'))
                {
                    value = value.Replace('\\', '/');
                }

                if (System.IO.Directory.Exists(value) && !System.IO.Path.HasExtension(value))
                {
                    this.path = value;
                }
            }
        }

        /// <summary>
        /// Gets the total count of FileListings found in the directory.
        /// </summary>
        public int FileCount
        {
            get
            {
                if (!this.fileCount.HasValue)
                {
                    this.fileCount = 0;
                    foreach (IListing listing in this.GetListings())
                    {
                        if (listing is FileListing)
                        {
                            this.fileCount++;
                        }
                    }
                }

                return this.fileCount.Value;
            }
        }

        // TODO: Improve refactor _Listings and GetListings()
        public IListing[] GetListings()
        {
            List<IListing> sortedListings = new List<IListing>();
            foreach (var listing in this.listings)
            {
                sortedListings.Add(listing);
                if (listing is DirectoryListing)
                {
                    DirectoryListing directory = listing as DirectoryListing;
                    foreach (IListing subListing in directory.GetListings())
                    {
                        if (subListing is FileListing)
                        {
                            sortedListings.Add(subListing);
                        }
                    }
                }
            }

            return sortedListings.ToArray();
        }

        public void Add(IListing listing)
        {
            this.listings.Add(listing);
        }
    }
}
