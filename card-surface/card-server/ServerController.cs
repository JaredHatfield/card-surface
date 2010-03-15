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
            this.gameController = new GameController();
            this.webController = new WebController(this.gameController);
            this.serverCommunicationController = new ServerCommunicationController(this.gameController);
            this.accountController = AccountController.Instance;
        }

        /// <summary>
        /// Gets the game controller.
        /// </summary>
        /// <value>The game controller.</value>
        public GameController GameController
        {
            get { return this.gameController; }
        }
    }
}
