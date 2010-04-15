// <copyright file="GameNetworkMenu.cs" company="University of Louisville Speed School of Engineering">
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
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameNetworkMenu"/> class.
        /// </summary>
        /// <param name="game">The local game.</param>
        /// <param name="tableCommunicationController">The table communication controller.</param>
        public GameNetworkMenu(Game game, TableCommunicationController tableCommunicationController)
            : base(game)
        {
            this.tableCommunicationController = tableCommunicationController;
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
            if (input.Equals("refresh", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    this.tableCommunicationController.SendRequestCurrentGameState(this.Game.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The communication library had some problem refreshing the game...");
                    Console.WriteLine(e.ToString());
                    this.PrompForEnter();
                }

                return true;
            }
            else
            {
                return base.CustomFunction(input);
            }
        }
    }
}
