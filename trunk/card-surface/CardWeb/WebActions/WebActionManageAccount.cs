// <copyright file="WebActionManageAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action to manage an account on the server.</summary>
namespace CardWeb.WebActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Action to manage an account on the server.
    /// </summary>
    public class WebActionManageAccount : WebAction
    {
        /// <summary>
        /// A string representation of this WebAction
        /// </summary>
        private string webActionName = "ManageAccount";

        /// <summary>
        /// Gets the name of the web action.
        /// </summary>
        /// <value>The name of the web action.</value>
        public override string WebActionName
        {
            get { return this.webActionName; }
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
