// <copyright file="CommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The base class for the communication controller.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The base class for the communication controller.
    /// </summary>
    public class CommunicationController
    {
        /// <summary>
        /// The GameController that the communication controller is able to interact with.
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        internal CommunicationController(IGameController gameController)
        {
            this.gameController = gameController;
        }
    }
}
