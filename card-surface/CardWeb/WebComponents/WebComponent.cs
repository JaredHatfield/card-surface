﻿// <copyright file="WebComponent.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Component for managing WebViews and WebActions.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using CardWeb.WebComponents.WebActions;
    using CardWeb.WebComponents.WebViews;

    /// <summary>
    /// WebComponent responsbile for processing HTTP requests.  HTTP responses should NOT be send from WebComponents.
    /// </summary>
    public abstract class WebComponent
    {
        /// <summary>
        /// Gets the component prefix.
        /// </summary>
        /// <value>The component prefix.</value>
        public abstract string ComponentPrefix { get; }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public abstract void PostRequest(WebRequest request);

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Determines if a WebComponent may be represented by a given prefix.
        /// </summary>
        /// <param name="prefix">The WebComponent prefix to be tested for equality.</param>
        /// <returns>True if the WebComponent contains a matching prefix; otherwise, false.</returns>
        public bool Equals(string prefix)
        {
            if (this.ComponentPrefix.Equals(prefix, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        } /* Equals() */
    }
}
