// <copyright file="WebView.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract view for displaying a web page.</summary>
namespace CardWeb.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An abstract view for displaying a web page.
    /// </summary>
    public abstract class WebView
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        public abstract void GetContent();
    }
}
