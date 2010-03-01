// <copyright file="WebAction.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract action for processing information sent to the server.</summary>
namespace CardWeb.WebActions
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
        /// Gets the name of the web action.
        /// </summary>
        /// <value>The name of the web action.</value>
        public abstract string WebActionName { get; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Determines if a WebAction is equal in name to another WebAction.
        /// </summary>
        /// <param name="action">The WebAction to test for equality.</param>
        /// <returns>True if the two WebActions are thes ame; otherwise, false.</returns>
        public bool Equals(WebAction action)
        {
            /* No need to ignore case; WebActionNames are private and not changeable in the instance. */
            if (this.WebActionName.Equals(action.WebActionName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a WebAction may be represented by a given string.
        /// </summary>
        /// <param name="action">A string representation of the WebAction to be tested for equality.</param>
        /// <returns>True if the WebAction contains a matching name; otherwise, false.</returns>
        public bool Equals(string action)
        {
            if (this.WebActionName.Equals(action, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
