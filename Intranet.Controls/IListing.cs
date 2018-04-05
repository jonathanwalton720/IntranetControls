using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Intranet.Controls
{
    /// <summary>
    /// Provides a friendly Title and Path for location
    /// </summary>
    public interface IListing
    {
        string Path { get; }

        string Title { get; }
    }
}
