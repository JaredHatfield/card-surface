// <copyright file="WebCookie.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Object that represents CardSurface cookie information.</summary>

namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An object that represents CardSurface cookie data found in HTTP requests
    /// </summary>
    public class WebCookie
    {
        /// <summary>
        /// The HTTP cookie section header search string
        /// </summary>
        public const string HttpCookieHeader = "Cookie:";

        /// <summary>
        /// CardSurface Session ID cookie key\value pair identifiers
        /// </summary>
        public const string CsidIdentifier = "csid";

        /// <summary>
        /// Guid representing the CardSurface Session ID
        /// </summary>
        private Guid csid;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebCookie"/> class.
        /// </summary>
        /// <param name="csid">The CardSurface Session ID as a GUID.</param>
        public WebCookie(Guid csid)
        {
            this.csid = csid;
        }

        /// <summary>
        /// Gets the csid.
        /// </summary>
        /// <value>The CardSurface Session ID as a GUID.</value>
        public Guid Csid
        {
            get { return this.csid; }
        }
    }
}
