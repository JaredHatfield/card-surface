﻿// <copyright file="ServerController.cs" company="University of Louisville Speed School of Engineering">
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
                this.gameController = new GameController();
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
        /// Gets the game controller.
        /// </summary>
        /// <value>The game controller.</value>
        public GameController GameController
        {
            /* TODO: REMOVE this property for release! */
            get { return this.gameController; }
        }
    }
}
