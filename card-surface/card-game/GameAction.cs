﻿// <copyright file="GameAction.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract class that implements an action for the game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// An abstract class that implements an action for the game.
    /// </summary>
    [Serializable]
    public abstract class GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Perform the action on the specified game.
        /// </summary>
        /// <param name="game">The game to modify.</param>
        /// <param name="player">The player.</param>
        /// <returns>True if the action was successful; otherwise false.</returns>
        public abstract bool Action(Game game, string player);

        /// <summary>
        /// Determines whether [is executable by player] [the specified game].
        /// </summary>
        /// <param name="game">The game to check.</param>
        /// <param name="player">The player to modify.</param>
        /// <returns>
        /// <c>true</c> if [is executable by player] [the specified game]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsExecutableByPlayer(Game game, Player player);

        /// <summary>
        /// Tests if the Player can execute this action.
        /// This test references the local GameAction name.
        /// This does not actually perform the test using the IsExecutableByPlayer function, rather depends on the Player.Actions lists to perform the test.
        /// </summary>
        /// <param name="player">The Player to test.</param>
        /// <returns>True if the Player can execute the GameAction; otherwise false.</returns>
        protected bool PlayerCanExecuteAction(Player player)
        {
            if (!player.Actions.Contains(this.Name))
            {
                throw new CardGameActionAccessDeniedException();
            }
            else
            {
                return true;
            }
        }
    }
}
