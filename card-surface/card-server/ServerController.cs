// <copyright file="ServerController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The main controller for the server that manages all of the other controllers.</summary>
namespace CardServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardAccount;
    using CardCommunication;
    using CardWeb;
    using CardWeb.WebExceptions;

    /// <summary>
    /// The main controller for the server that manages all of the other controllers.
    /// </summary>
    public class ServerController
    {
        /// <summary>
        /// The GameController.
        /// </summary>
        private GameController gameController;

        /// <summary>
        /// The WebController
        /// </summary>
        private WebController webController;

        /// <summary>
        /// The ServerCommunicationController.
        /// </summary>
        private ServerCommunicationController serverCommunicationController;

        /// <summary>
        /// The AccountController.
        /// </summary>
        private AccountController accountController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerController"/> class.
        /// </summary>
        internal ServerController()
        {
            try
            {
                this.gameController = GameController.Instance();
                this.webController = new WebController(this.gameController);
                this.serverCommunicationController = new ServerCommunicationController(this.gameController);
                this.accountController = AccountController.Instance;
            }
            catch (WebServerCouldNotLaunchException wscnle)
            {
                Console.WriteLine("ServerController: " + wscnle.Message);
                Console.ReadKey();
                /* TODO: Exit codes for end-user applications (like CardGameCommandLine, CardServer, CardTable) should be abstracted as part of the exceptions. */
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Gets the server communication controller.
        /// </summary>
        /// <value>The server communication controller.</value>
        public ServerCommunicationController ServerCommunicationController
        {
            get { return this.serverCommunicationController; }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Close(object sender, EventArgs args)
        {
            this.serverCommunicationController.Close(this, args);
            this.gameController.Close(this, args);
            this.webController.Close(this, args);
            this.accountController.Close(this, args);
        }
    }
}
