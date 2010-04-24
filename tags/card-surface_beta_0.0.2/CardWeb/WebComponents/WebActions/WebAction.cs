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
        /// Executes this action.
        /// </summary>
        public abstract void Execute();
    }
}
