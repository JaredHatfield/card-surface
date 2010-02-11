// <copyright file="WebController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The WebController that hosts the internal web server.</summary>
namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The WebController that hosts the internal web server.
    /// </summary>
    public class WebController
    {
        /// <summary>
        /// The GameController that the web server is able to interact with.
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebController(IGameController gameController)
        {
            this.gameController = gameController;
        }
    }
}
