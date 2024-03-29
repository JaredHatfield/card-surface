﻿// <copyright file="GameNetworkMenu.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The Game Menu that allows the player to actually play the game.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardCommunication;
    using CardGame;

    /// <summary>
    /// The Game Menu that allows the player to actually play the game.
    /// </summary>
    internal class GameNetworkMenu : GameMenu
    {
        /// <summary>
        /// The table communication controller.
        /// </summary>
        private ClientCommunicationController clientCommunicationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameNetworkMenu"/> class.
        /// </summary>
        /// <param name="game">The local game.</param>
        /// <param name="tableCommunicationController">The table communication controller.</param>
        public GameNetworkMenu(Game game, ClientCommunicationController tableCommunicationController)
            : base(game)
        {
            this.clientCommunicationController = tableCommunicationController;
        }

        /// <summary>
        /// Allows the execution of custom functions.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// True if the custom function executed code; otherwise false.
        /// </returns>
        protected override bool CustomFunction(string input)
        {
            return base.CustomFunction(input);
        }
    }
}
