// <copyright file="GameMessage.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A version of a Game used for communication.</summary>
namespace CardCommunication.GameObject
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardCommunication.CommunicationException;
    using CardGame;

    /// <summary>
    /// A version of a Game used for communication.
    /// </summary>
    [Serializable]
    public sealed class GameMessage : Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessage"/> class.
        /// </summary>
        /// <param name="game">The game to copy.</param>
        internal GameMessage(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Gets a value indicating the minimum amount of money required to join a game.
        /// </summary>
        /// <value>The minimum stake for the game.</value>
        public override int MinimumStake
        {
            get { throw new CardCommunicationException("GameMessage does not have a minimum stake"); }
        }

        /// <summary>
        /// Gets the name of the game.
        /// </summary>
        /// <value>The game's name.</value>
        public override string Name
        {
            get { throw new CardCommunicationException("GameMessage does not have a name."); }
        }

        // TODO: More methods should probably be prevented from being used, but that is low priority
    }
}
