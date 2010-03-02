﻿// <copyright file="WebView.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract view for displaying a web page.</summary>
namespace CardWeb.WebComponents.WebViews
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
        /// Index position for the name of a token name in HTTP POST data
        /// </summary>
        public const int PostRequestTokenNameIndex = 0;

        /// <summary>
        /// Index position for the name of a token value in HTTP POST data
        /// </summary>
        public const int PostRequestTokenValueIndex = 1;

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebView's header.</returns>
        public abstract string GetHeader();

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        public abstract string GetContent();

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebView's content type.</returns>
        public abstract string GetContentType();
    }
}
