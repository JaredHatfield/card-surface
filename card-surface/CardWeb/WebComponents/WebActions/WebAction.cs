// <copyright file="WebAction.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract action for processing information sent to the server.</summary>
namespace CardWeb.WebComponents.WebActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An abstract action for processing information sent to the server.
    /// </summary>
    public abstract class WebAction
    {
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebAction's header.</returns>
        public abstract string GetHeader();

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebAction's content.</returns>
        public abstract string GetContent();

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebAction's content type.</returns>
        public abstract string GetContentType();

        /// <summary>
        /// Gets the length of the content.
        /// </summary>
        /// <returns>An integer representing the number of bytes in the reponse content.</returns>
        public abstract int GetContentLength();

        /// <summary>
        /// Executes this action.
        /// </summary>
        public abstract void Execute();
    }
}
